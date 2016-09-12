module SpeakersProxy

open Config
open Dtos
open JsonHttpClient
open System

let getSpeaker (profileId : Guid) = get<Speaker> <| new Uri(profilesUri, profileId.ToString())
