module Proxy

open Config
open Dtos
open JsonHttpClient
open System

module Events = 
    let getEvent (eventId : Guid) = get<Event> <| new Uri(eventsUri, eventId.ToString())

module Sessions = 
    let getSessionsByEventId (eventId : Guid) = get<Session []> <| new Uri(sessionsUri, "?eventId=" + eventId.ToString())

module Speakers = 
    let getSpeaker (profileId : Guid) = get<Speaker> <| new Uri(profilesUri, profileId.ToString())