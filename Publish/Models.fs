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
      SpeakerImageUri : string
      SpeakerRating : int
      StartDate : DateTime option
      EndDate : DateTime option }

type EventDetail = 
    { Id : Guid
      Date : DateTime option
      Description : string
      Location : string
      Sessions : EventSession [] }
