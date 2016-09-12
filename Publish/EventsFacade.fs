module EventsFacade

open DataTransform
open System

let getEventDetail (eventId : Guid) = 
    let record = EventsProxy.getEvent eventId

    let eventSessions = 
        SessionsProxy.getSessionsByEventId eventId
        |> Array.map (fun session -> 
            let speaker = SpeakersProxy.getSpeaker session.SpeakerId
            Session.toEventSession speaker session)
    Event.toDetail eventSessions record
