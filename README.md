# Overview
This document contains an architectural overview of my solution to the technical test, an outline of features I did not have time to implement and instructions on how to run use it. 

# Architectural Overview
This application has been designed with a domain-driven, clean architecture approach. I've focused on extensibility and each layer has been created as it's own project to help organise the code, and to make it easier to switch out services should that be required. Dependency injection has been used thought the solution to increase decoupling and testability.

## Presentation Layer
I have built the presentation layer as a simple API. This was done as an easy way to run the application and send test requests. For a full application, this wouldn't make sense as an API, as it's making the application more talky than it needs to be. If I were building it as a web app, I would contact the services directly from the frontend, only having a backend if I needed to store any keys. If I were building it out as a desktop app, the application layer would be replaced with a WPF project. 

## Application Layer
The application layer has been built using a vertical slice. This allows the request to be completely decoupled from any other request if more were to be created (See the potential extensions section for examples of what these would be). I've used dependency inversion to access the MusicBrainz and lyrics.ovh apis. This allows these services to be replaced if required. 

## Domain Layer
The domain layer is used to create a data contract between the artist service and the 
 
# Potential Extensions
Due to time constraints, I've left out a few features that would enhance the functionality of the application. I've given a brief description of them here to show how they would be implemented. 

## Advanced search
- Currently returns first artist found
- Add a second endpoint that would return a list of artists, including there IDs
- This could be displayed in a table in UI for a user to select the correct artist
- GetLyricStats would then accept an artist ID rather than an artist name

## Sample Stats
- For artists with a large catalogue, this takes a long time
- Give users the option to get statistics from a sample of the data, rather than the full set

## Handling lyrics not found
- Currently just ignored
- Return not found lyrics to user
- Have option to include in calculation as zero

## Options More query options (Type, years, include duplicates, ect)

## Rate limit from headers

## Retry limit on Lyric Requests

# Running
## Requirements
## Commands
## Api Definition
## Appsettings