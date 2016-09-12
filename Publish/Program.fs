open System
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

module EventsProxy = 
    open JsonHttpClient
    open Dtos
    let getEvent (eventId : Guid) = get<Event> <| new Uri(eventsUri, eventId.ToString())

module SessionsProxy = 
    open JsonHttpClient
    open Dtos
    let getSessionsByEventId (eventId : Guid) = get<Session []> <| new Uri(sessionsUri, "?eventId=" + eventId.ToString())

module EventsFacade = 
    let getEventDetail (eventId : Guid) = 
        let record = EventsProxy.getEvent eventId
        let sessions = SessionsProxy.getSessionsByEventId eventId
        printfn "Found Event %A" record
        printfn "Found Sessions %A" sessions
        record

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
        let content = new FormUrlEncodedContent(contentPairs)
        let response = client.PostAsync(uri,content).Result
        let responseContent = response.Content.ReadAsStringAsync().Result
        responseContent    

[<EntryPoint>]
let main _ = 
    JsonSettings.setDefaults()
    printfn "Enter event id to publish"
    let eventId = Guid(Console.ReadLine())
    printfn "Getting Event Detail"
    let event = EventsFacade.getEventDetail eventId

    //Temporarily not publishing event
    //printfn "%A" <| publishEvent()
    Console.ReadLine() |> ignore
    0
