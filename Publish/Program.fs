open System

[<EntryPoint>]
let main _ = 
    JsonSettings.setDefaults()
    printfn "Enter event id to publish"
    let eventId = Guid(Console.ReadLine())
    printfn "Getting Event Detail"
    let event = EventsFacade.getEventDetail eventId
    printfn "Event: %A" event

    let meetupData = DataTransform.MeetupData.fromEventDetail event

    let publishResult = MeetupHttpClient.publishEvent meetupData
    printfn "%A" publishResult
    Console.ReadLine() |> ignore
    0
