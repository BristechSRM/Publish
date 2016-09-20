module MeetupHttpClient

open Config
open System
open System.Net
open System.Net.Http
open System.Collections.Generic

//http://www.meetup.com/meetup_api/docs/:urlname/events/#create
let publishEvent meetupData = 
    use client = new HttpClient()
    let uri = Uri <| sprintf "https://api.meetup.com/%s/events?&sign=true&key=%s" meetupTargetGroup meetupApiKey
    let eventCreationData = 
        [| new KeyValuePair<string,string>("announce","false")
           new KeyValuePair<string,string>("publish_status","draft")
           new KeyValuePair<string,string>("self_rsvp","false")|] |> Array.append meetupData
    use content = new FormUrlEncodedContent(eventCreationData)
    use response = client.PostAsync(uri, content).Result
    match response.StatusCode with
    | HttpStatusCode.Created -> 
        //TODO choose what we want from response and JsonConvert
        //Do todo here or in PublishController?
        response.Content.ReadAsStringAsync().Result
    | errorCode -> 
        let errorResponse = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in post request to publish event to Meetup. Status code: %i. Reason phrase: %s. Error Message: %s" (int (errorCode)) response.ReasonPhrase errorResponse
        failwith message
