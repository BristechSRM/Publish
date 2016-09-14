module MeetupHttpClient

open Config
open System
open System.Net
open System.Net.Http

//http://www.meetup.com/meetup_api/docs/:urlname/events/#create
let publishEvent meetupData = 
    use client = new HttpClient()
    let apikey = meetupApiKey
    let uri = Uri <| sprintf "https://api.meetup.com/Bristech-Biztalk/events?&sign=true&key=%s" apikey
    use content = new FormUrlEncodedContent(meetupData)
    use response = client.PostAsync(uri, content).Result
    match response.StatusCode with
    | HttpStatusCode.Created -> 
        //TODO choose what we want from response and JsonConvert
        response.Content.ReadAsStringAsync().Result
    | errorCode -> 
        let errorResponse = response.Content.ReadAsStringAsync().Result
        let message = sprintf "Error in post request to publish event to Meetup. Status code: %i. Reason phrase: %s. Error Message: %s" (int (errorCode)) response.ReasonPhrase errorResponse
        failwith message

