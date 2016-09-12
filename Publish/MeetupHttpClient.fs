module MeetupHttpClient

open Config
open System
open System.Net.Http

//http://www.meetup.com/meetup_api/docs/:urlname/events/#create
let publishEvent meetupData = 
    use client = new HttpClient()
    let apikey = meetupApiKey
    let uri = Uri <| sprintf "https://api.meetup.com/Bristech-Biztalk/events?&sign=true&key=%s" apikey
    use content = new FormUrlEncodedContent(meetupData)
    use response = client.PostAsync(uri, content).Result
    response.Content.ReadAsStringAsync().Result
