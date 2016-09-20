module Proxy

open RestModels
open Config
open Dtos
open JsonHttpClient
open System

module Events =
    let get (eventId : Guid) = get<Event> <| new Uri(eventsUri, eventId.ToString())

    let patch (eventId : Guid) (op : PatchOp) = patch eventsUri eventId op

module MeetupEvents = 
    let post (meetup: MeetupEvent) = postAndGetGuid meetupEventsUri meetup

module Sessions = 
    let getByEventIds (eventId : Guid) = get<Session []> <| new Uri(sessionsUri, "?eventId=" + eventId.ToString())

module Speakers = 
    let get (profileId : Guid) = get<Speaker> <| new Uri(profilesUri, profileId.ToString())