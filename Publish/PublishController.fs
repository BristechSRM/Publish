namespace Controllers

open Dtos
open RestModels
open Proxy
open System
open System.Net
open System.Net.Http
open System.Web.Http

type PublishController() = 
    inherit ApiController()

    member x.Post(eventId : Guid) = 
        let event = EventsFacade.getEventDetail eventId
        match event.MeetupEventId with
        | Some _ -> 
            x.Request.CreateErrorResponse(HttpStatusCode.Conflict, "Event has already been published")
        | None -> 
            let result = MeetupHttpClient.publishEvent event
        
            let meetupEvent = 
                { Id = Guid.Empty
                  EventId = event.Id
                  MeetupId = result.Id
                  PublishedDate = Some DateTime.UtcNow
                  MeetupUrl = result.Link }
        
            let meetupEventId = MeetupEvents.post meetupEvent
            x.Request.CreateResponse(HttpStatusCode.Created, meetupEventId)

    member x.Delete(meetupEventId : Guid) = 
        let meetupEvent = MeetupEvents.get meetupEventId
        let remoteMeetupEvent = MeetupHttpClient.getEvent meetupEvent.MeetupId
        match remoteMeetupEvent.Status.ToLowerInvariant() with
        | "draft" -> 
            MeetupHttpClient.deleteEvent meetupEvent.MeetupId
            MeetupEvents.delete meetupEventId
            x.Request.CreateResponse(HttpStatusCode.NoContent)
        | _ -> 
            x.Request.CreateErrorResponse(HttpStatusCode.Conflict, sprintf "Event \"%s\" is not a draft. It's status is: \"%s\".  It cannot be deleted from SRM." remoteMeetupEvent.Name remoteMeetupEvent.Status)

    member x.Put(meetupEventId : Guid) = 
        let meetupEvent = MeetupEvents.get meetupEventId
        let event = EventsFacade.getEventDetail meetupEvent.EventId
        let result = MeetupHttpClient.updateEvent meetupEvent.MeetupId event
        MeetupEvents.patch meetupEventId { Path = "PublishedDate"; Value = DateTime.UtcNow.ToString ( "o", System.Globalization.CultureInfo.InvariantCulture ) }
        x.Request.CreateResponse(HttpStatusCode.NoContent)
