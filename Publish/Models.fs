module Models

open System

type EventSession = 
    { Id : Guid
      Title : string
      Description : string
      SpeakerId : Guid
      SpeakerForename : string
      SpeakerSurname : string
      SpeakerBio : string
      SpeakerImageUri : string }

type EventDetail = 
    { Id : Guid
      Date : DateTime option
      Description : string
      MeetupEventId : Guid option
      Sessions : EventSession [] }
