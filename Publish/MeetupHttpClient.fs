module MeetupHttpClient

open Config
open Newtonsoft.Json
open MeetupModels
open System
open System.Net
open System.Net.Http
open System.Collections.Generic

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
        JsonConvert.DeserializeObject<EventCreateResponse>(json)
    | errorCode -> 
        let errorMessage = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in post request to publish event to Meetup. Status code: %i. Reason phrase: %s. Error Message: %s" (int (errorCode)) response.ReasonPhrase errorMessage
        failwith message

let deleteEvent meetupEventId =
    use client = new HttpClient()
    let uri = Uri <| sprintf "https://api.meetup.com/%s/events/%s?sign=true&key=%s" meetupTargetGroup meetupEventId meetupApiKey
    use response = client.DeleteAsync(uri).Result
    match response.StatusCode with
    | HttpStatusCode.NoContent -> ()
    | errorCode -> 
        let errorMessage = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in request to delete event to Meetup. Status code: %i. Reason phrase: %s. Error Message: %s" (int (errorCode)) response.ReasonPhrase errorMessage
        failwith message
