module EventsFacade

open DataTransform
open Proxy
open System

let getEventDetail (eventId : Guid) = 
    let record = Events.getEvent eventId

    let eventSessions = 
        Sessions.getSessionsByEventId eventId
        |> Array.map (fun session -> 
            let speaker = Speakers.getSpeaker session.SpeakerId
            Session.toEventSession speaker session)
    Event.toDetail eventSessions record
