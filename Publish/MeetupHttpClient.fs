﻿module MeetupHttpClient

open Config
open Newtonsoft.Json
open MeetupModels
open System
open System.Net
open System.Net.Http
open System.Collections.Generic

let getEvent remoteMeetupEventId = 
    use client = new HttpClient()
    let uri = Uri <| sprintf "https://api.meetup.com/%s/events/%s?sign=true&key=%s" meetupTargetGroup remoteMeetupEventId meetupApiKey
    use response = client.GetAsync(uri).Result
    match response.StatusCode with
    | HttpStatusCode.OK -> 
        let json = response.Content.ReadAsStringAsync().Result
        JsonConvert.DeserializeObject<MeetupEventData>(json)
    | errorCode -> 
        let errorMessage = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in get request for event.on Meetup. MeetupEventId: %s. Status code: %i. Reason phrase: %s. Error Message: %s" remoteMeetupEventId (int (errorCode)) response.ReasonPhrase errorMessage
        failwith message

//http://www.meetup.com/meetup_api/docs/:urlname/events/#create
let publishEvent event = 
    use client = new HttpClient()
    let uri = Uri <| sprintf "https://api.meetup.com/%s/events?sign=true" meetupTargetGroup
    let meetupData = DataTransform.MeetupData.fromEventDetail event
    let eventCreationData = 
        [| new KeyValuePair<string,string>("key", meetupApiKey)
           new KeyValuePair<string,string>("announce","false")
           new KeyValuePair<string,string>("publish_status","draft")
           new KeyValuePair<string,string>("self_rsvp","false")|] |> Array.append meetupData
    use content = new FormUrlEncodedContent(eventCreationData)
    use response = client.PostAsync(uri, content).Result
    match response.StatusCode with
    | HttpStatusCode.Created -> 
        let json = response.Content.ReadAsStringAsync().Result
        JsonConvert.DeserializeObject<MeetupEventData>(json)
    | errorCode -> 
        let errorMessage = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in post request to publish event to Meetup. Status code: %i. Reason phrase: %s. Error Message: %s" (int (errorCode)) response.ReasonPhrase errorMessage
        failwith message

let deleteEvent remoteMeetupEventId =
    use client = new HttpClient()
    let uri = Uri <| sprintf "https://api.meetup.com/%s/events/%s?sign=true&key=%s" meetupTargetGroup remoteMeetupEventId meetupApiKey
    use response = client.DeleteAsync(uri).Result
    match response.StatusCode with
    | HttpStatusCode.NoContent -> ()
    | errorCode -> 
        let errorMessage = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in request to delete event to Meetup. Status code: %i. Reason phrase: %s. Error Message: %s" (int (errorCode)) response.ReasonPhrase errorMessage
        failwith message

let updateEvent remoteMeetupEventId event = 
    use client = new HttpClient()
    let uri = Uri <| sprintf "https://api.meetup.com/%s/events/%s?sign=true&key=%s" meetupTargetGroup remoteMeetupEventId meetupApiKey
    let meetupData = DataTransform.MeetupData.fromEventDetail event
    let content = new FormUrlEncodedContent(meetupData)
    let message = new HttpRequestMessage(new HttpMethod("PATCH"), uri, Content = content)
    use response = client.SendAsync(message).Result
    match response.StatusCode with
    | HttpStatusCode.OK -> response.Content.ReadAsStringAsync().Result
    | errorCode -> 
        let errorMessage = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in request to patch event on Meetup. Status code: %i. Reason phrase: %s. Error Message: %s" (int (errorCode)) response.ReasonPhrase errorMessage
        failwith message
