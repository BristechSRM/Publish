module Config

open System
open System.Configuration

let getConfigValue (key : string) = 
    let value = ConfigurationManager.AppSettings.Item(key)
    if String.IsNullOrWhiteSpace value then
        failwith <| sprintf "Missing configuration value: %s" key
    else 
        value

let meetupApiKey = getConfigValue "MeetupApiKey"
