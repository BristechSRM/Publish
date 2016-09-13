namespace Controllers

open Proxy
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
        Events.patchEvent event.Id { Path = "publisheddate"
                                     Value = DateTime.UtcNow |> convertToISO8601 }
        //TODO respond with useful information from meetup response
        x.Request.CreateResponse(HttpStatusCode.NoContent)
