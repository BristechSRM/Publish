﻿namespace Controllers

open Dtos
open Proxy
open System
open System.Net
open System.Net.Http
open System.Web.Http

type PublishController() = 
    inherit ApiController()
    member x.Post(eventId : Guid) = 
        let event = EventsFacade.getEventDetail eventId
        let meetupData = DataTransform.MeetupData.fromEventDetail event
        let result = MeetupHttpClient.publishEvent meetupData
        
        let meetupEvent = 
            { Id = Guid.Empty
              EventId = event.Id
              MeetupId = result.Id
              PublishedDate = Some DateTime.UtcNow
              MeetupUrl = result.Link }
        
        let meetupEventId = MeetupEvents.post meetupEvent
        x.Request.CreateResponse(HttpStatusCode.Created, meetupEventId)

