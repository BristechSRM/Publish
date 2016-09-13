open Microsoft.Owin.Hosting
open System
open System.Configuration
open System.Threading
open Serilog
open Startup
open Config

[<EntryPoint>]
let main _ = 
    JsonSettings.setDefaults()

    //TODO meetupPublish controller
    //    printfn "Enter event id to publish"
    //    let eventId = Guid(Console.ReadLine())
    //    printfn "Getting Event Detail"
    //    let event = EventsFacade.getEventDetail eventId
    //    printfn "Event: %A" event
    //
    //    let meetupData = DataTransform.MeetupData.fromEventDetail event
    //
    //    let publishResult = MeetupHttpClient.publishEvent meetupData
    //    printfn "%A" publishResult

    use server = WebApp.Start<Startup>(baseUrl)
    Log.Information("Listening on {0}", baseUrl)

    let waitIndefinitelyWithToken = 
        let cancelSource = new CancellationTokenSource()
        cancelSource.Token.WaitHandle.WaitOne() |> ignore
    0
