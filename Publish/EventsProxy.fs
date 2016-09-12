module EventsProxy

open Config
open Dtos
open JsonHttpClient
open System

let getEvent (eventId : Guid) = get<Event> <| new Uri(eventsUri, eventId.ToString())
