module EventsFacade

open DataTransform
open Proxy
open System

let getEventDetail (eventId : Guid) = 
    let record = Events.get eventId

    let eventSessions = 
        Sessions.getByEventIds eventId
        |> Array.map (fun session -> 
            let speaker = Speakers.get session.SpeakerId
            Session.toEventSession speaker session)
    Event.toDetail eventSessions record
