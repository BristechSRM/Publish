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

module Models = 
    [<CLIMutable>]
    type Event =
        { Id: Guid
          Date: DateTime option
          Name: string 
          PublishedDate : DateTime option } 

module EventsProxy = 
    open JsonHttpClient
    open Models
    let getEvent(eventId : Guid) = get<Event> <| new Uri(eventsUri, eventId.ToString())

module MeetupHttpClient = 
    let publishEvent() = 
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
    printfn "Getting Event"
    let event = EventsProxy.getEvent(eventId)
    printfn "Found Event"
    printfn "%A" event

    //Temporarily not publishing event
    //printfn "%A" <| publishEvent()
    Console.ReadLine() |> ignore
    0
