module Dtos

open System

type Event =
    { Id: Guid
      Date: DateTime option
      Name: string 
      PublishedDate : DateTime option } 

type Session =
    { Id : Guid
      Title : string
      Description : string
      Status : string
      Date : DateTime option
      DateAdded : string
      SpeakerId : Guid
      AdminId : Guid option 
      EventId : Guid option }

type Speaker =
    { Id : Guid
      Forename : string
      Surname : string
      Rating : int
      ImageUri : string
      Bio : string }

type MeetupEvent = 
    { Id : Guid
      EventId : Guid
      MeetupId : string
      PublishedDate : DateTime option
      MeetupUrl : string }
