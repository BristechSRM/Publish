module Config

open System
open System.Configuration

let getConfigValue (key : string) = 
    let value = ConfigurationManager.AppSettings.Item(key)
    if String.IsNullOrWhiteSpace value then
        failwith <| sprintf "Missing configuration value: %s" key
    else 
        value

let getUriConfigValue (key : string) = getConfigValue key |> Uri

let meetupApiKey = getConfigValue "MeetupApiKey"
let sessionsServiceUri = getUriConfigValue "SessionsServiceUrl"
let eventsUri = Uri (sessionsServiceUri, "events/")
