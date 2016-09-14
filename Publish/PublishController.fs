namespace Controllers

open System
open System.Net
open System.Net.Http
open System.Web.Http

type PublishController() = 
    inherit ApiController()

    member x.Post(eventId : Guid) = 
        let event = EventsFacade.getEventDetail eventId
        let meetupData = DataTransform.MeetupData.fromEventDetail event
        let publishResult = MeetupHttpClient.publishEvent meetupData
        //TODO respond with useful information from meetup response
        x.Request.CreateResponse(HttpStatusCode.NoContent)

