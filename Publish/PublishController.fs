namespace Controllers

open Proxy
open Dtos
open RestModels
open System
open System.Net
open System.Net.Http
open System.Web.Http

type PublishController() = 
    inherit ApiController()
    let convertToISO8601 (datetime : DateTime) = datetime.ToString("yyyy-MM-ddTHH\:mm\:ss\Z")

    member x.Post(eventId : Guid) = 
        let event = EventsFacade.getEventDetail eventId
        let meetupData = DataTransform.MeetupData.fromEventDetail event
        let publishResult = MeetupHttpClient.publishEvent meetupData
        let meetupEvent = 
                { Id = Guid.Empty
                  EventId = event.Id
                  MeetupId = "" 
                  PublishedDate = Some DateTime.UtcNow
                  MeetupUrl = "" }
        let meetupEventId = MeetupEvents.post meetupEvent
        x.Request.CreateResponse(HttpStatusCode.Created,meetupEventId)
