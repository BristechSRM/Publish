module Config

open System
open System.Configuration

let getString (key : string) = 
    let value = ConfigurationManager.AppSettings.Item(key)
    if String.IsNullOrWhiteSpace value then
        failwith <| sprintf "Missing configuration value: %s" key
    else 
        value

let getUri (key : string) = getString key |> Uri

let meetupApiKey = getString "MeetupApiKey"

let sessionsServiceUri = getUri "SessionsServiceUrl"

let eventsUri = Uri (sessionsServiceUri, "events/")
let profilesUri = Uri (sessionsServiceUri, "profiles/")
let sessionsUri = Uri (sessionsServiceUri, "sessions/")
