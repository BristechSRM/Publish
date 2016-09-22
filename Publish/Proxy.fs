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
    let get (meetupEventId : Guid) = get<MeetupEvent> <| new Uri(meetupEventsUri, meetupEventId.ToString())

    let post (meetup: MeetupEvent) = postAndGetGuid meetupEventsUri meetup

    let delete (meetupEventId : Guid) = delete <| new Uri(meetupEventsUri, meetupEventId.ToString())

module Sessions = 
    let getByEventIds (eventId : Guid) = get<Session []> <| new Uri(sessionsUri, "?eventId=" + eventId.ToString())

module Speakers = 
    let get (profileId : Guid) = get<Speaker> <| new Uri(profilesUri, profileId.ToString())