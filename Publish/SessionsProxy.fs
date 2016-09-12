module SessionsProxy

open Config
open Dtos
open JsonHttpClient
open System

let getSessionsByEventId (eventId : Guid) = get<Session []> <| new Uri(sessionsUri, "?eventId=" + eventId.ToString())