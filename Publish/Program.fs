open System
open System.Collections.Generic
open System.Net.Http
open System.Web
open Config

let publishEvent() = 
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
let main argv = 
    printfn "%A" <| publishEvent()
    Console.ReadLine() |> ignore
    0 // return an integer exit code
