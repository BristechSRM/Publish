﻿open System
open System.Collections.Generic
open System.Net
open System.Net.Http
open Config
open Newtonsoft.Json

module JsonHttpClient = 
    let get<'Model> (uri : Uri) = 
        use client = new HttpClient()
        use response = client.GetAsync(uri).Result
        let modelName = typeof<'Model>.Name
        match response.StatusCode with
        | HttpStatusCode.OK -> 
            let json = response.Content.ReadAsStringAsync().Result
            JsonConvert.DeserializeObject<'Model>(json)
        | errorCode -> 
            let errorResponse = response.Content.ReadAsStringAsync().Result
            let message = sprintf "Error in get request for %s. Status code: %i. Reason phrase: %s. Error Message: %s" modelName (int (errorCode)) response.ReasonPhrase errorResponse
            failwith message

module Dtos = 
    type Event =
        { Id: Guid
          Date: DateTime option
          Name: string 
          PublishedDate : DateTime option } 

    type Session =
        { Id : Guid
          Title : string
          Description : string
          Status : string
          Date : DateTime option
          DateAdded : string
          SpeakerId : Guid
          AdminId : Guid option 
          EventId : Guid option }

    type Speaker =
        { Id : Guid
          Forename : string
          Surname : string
          Rating : int
          ImageUri : string
          Bio : string }

module Models =
    type EventSession =
        { Id : Guid
          Title : string
          Description : string
          SpeakerId : Guid
          SpeakerForename : string
          SpeakerSurname : string
          SpeakerBio : string
          SpeakerImageUri : string
          SpeakerRating : int
          StartDate : DateTime option
          EndDate : DateTime option }

    type EventDetail =
        { Id : Guid
          Date : DateTime
          Description : string
          Location : string
          PublishedDate : DateTime option 
          Sessions : EventSession[] }

module DataTransform = 
    open Dtos
    open Models

    module Session = 
        let toEventSession (speaker : Dtos.Speaker) (session : Dtos.Session) : Models.EventSession = 
            { Id = session.Id
              Title = session.Title
              Description = session.Description
              SpeakerId = speaker.Id
              SpeakerForename = speaker.Forename
              SpeakerSurname = speaker.Surname
              SpeakerBio = speaker.Bio
              SpeakerImageUri = speaker.ImageUri
              SpeakerRating = speaker.Rating
              StartDate = session.Date
              EndDate = session.Date |> Option.map (fun date -> date.AddHours(1.0)) }

    module Event = 
        let toDetail eventSessions (event: Dtos.Event) : Models.EventDetail =
            { Id = event.Id
              Date = 
                  match event.Date with
                  | None -> DateTime.Today
                  | Some date -> date
              Description = event.Name
              Location = ""
              PublishedDate = event.PublishedDate
              Sessions = eventSessions }

module EventsProxy = 
    open Dtos
    open JsonHttpClient
    
    let getEvent (eventId : Guid) = get<Event> <| new Uri(eventsUri, eventId.ToString())

module SessionsProxy = 
    open JsonHttpClient
    open Dtos

    let getSessionsByEventId (eventId : Guid) = get<Session []> <| new Uri(sessionsUri, "?eventId=" + eventId.ToString())

module SpeakersProxy = 
    open JsonHttpClient
    open Dtos

    let getSpeaker (profileId : Guid) = get<Speaker> <| new Uri(profilesUri, profileId.ToString())

module EventsFacade = 
    open DataTransform

    let getEventDetail (eventId : Guid) = 
        let record = EventsProxy.getEvent eventId
        let eventSessions = 
            SessionsProxy.getSessionsByEventId eventId
            |> Array.map (fun session -> 
                let speaker = SpeakersProxy.getSpeaker session.SpeakerId
                Session.toEventSession speaker session)
        Event.toDetail eventSessions record

module MeetupHttpClient = 
    let publishEvent () = 
        use client = new HttpClient()
        let apikey = meetupApiKey
        let uri = Uri <| sprintf "https://api.meetup.com/Bristech-Biztalk/events?&sign=true&key=%s" apikey
        let contentPairs = 
            [|
                new KeyValuePair<string, string>("name","Test event from Publish 1")
                new KeyValuePair<string, string>("description","Test event from Publish 1")
            |]
        use content = new FormUrlEncodedContent(contentPairs)
        use response = client.PostAsync(uri,content).Result
        response.Content.ReadAsStringAsync().Result

[<EntryPoint>]
let main _ = 
    JsonSettings.setDefaults()
    printfn "Enter event id to publish"
    let eventId = Guid(Console.ReadLine())
    printfn "Getting Event Detail"
    let event = EventsFacade.getEventDetail eventId
    printfn "Event: %A" event

    //Temporarily not publishing event
    //printfn "%A" <| publishEvent()
    Console.ReadLine() |> ignore
    0
