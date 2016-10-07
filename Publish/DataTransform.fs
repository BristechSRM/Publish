module DataTransform

open Dtos
open Models
open System.Collections.Generic
open System

module Session = 
    let toEventSession (speaker : Dtos.Speaker) (session : Dtos.Session) : Models.EventSession = 
        { Id = session.Id
          Title = session.Title
          Description = session.Description
          SpeakerId = speaker.Id
          SpeakerForename = speaker.Forename
          SpeakerSurname = speaker.Surname
          SpeakerBio = speaker.Bio
          SpeakerImageUri = speaker.ImageUri }
    
module Event = 
    let toDetail eventSessions (event : Dtos.Event) : Models.EventDetail = 
        { Id = event.Id
          Date = event.Date
          Description = event.Name
          MeetupEventId = event.MeetupEventId
          Sessions = eventSessions }

module MeetupData = 
    //Note: Meetup has 80 character limit on title. 

    let getEventDescription (detail : EventDetail) = 
        detail.Sessions
        |> Array.map (fun session -> 
            sprintf "<b>%s %s - %s</b>\n\n%s\n\n<b>About %s:</b>\n\n%s" 
                session.SpeakerForename session.SpeakerSurname session.Title 
                session.Description
                session.SpeakerForename
                session.SpeakerBio)
        |> String.concat "\n\n"  
        
    let private epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    let private toUnixTime (dateTime : DateTime) = (dateTime.ToUniversalTime() - epoch).TotalMilliseconds          

    let fromEventDetail (detail : EventDetail) = 
        let coreDetails = 
            [| new KeyValuePair<string, string>("name", detail.Description)
               new KeyValuePair<string, string>("description", getEventDescription detail) |] 
        match detail.Date with 
        | None -> coreDetails
        | Some meetupDate -> 
            let unixMilisecondDate = meetupDate |> toUnixTime |> int64
            [| new KeyValuePair<string, string>("time", unixMilisecondDate.ToString()) |] |> Array.append coreDetails
