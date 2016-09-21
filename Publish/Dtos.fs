module Dtos

open System

type Event =
    { Id: Guid
      Date: DateTime option
      Name: string 
      MeetupEventId : Guid option} 

type Session =
    { Id : Guid
      Title : string
      Description : string
      Date : DateTime option
      SpeakerId : Guid
      EventId : Guid option }

type Speaker =
    { Id : Guid
      Forename : string
      Surname : string
      ImageUri : string
      Bio : string }

type MeetupEvent = 
    { Id : Guid
      EventId : Guid
      MeetupId : string
      PublishedDate : DateTime option
      MeetupUrl : string }
