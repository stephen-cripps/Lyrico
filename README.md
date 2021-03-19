# Overview
This document contains an architectural overview of my solution to the technical test, an outline of features I did not have time to implement and instructions on how to run use it. 

# Architectural Overview
This application has been designed with a domain-driven, clean architecture approach. I've focused on extensibility with each layer created as it's own project to help organise the code and to make it easier to switch out services should that be required. Dependency injection has been used through the solution to increase decoupling and testability.

## Presentation Layer
I have built the presentation layer as a simple API. This was done as an easy way to run the application and send test requests. For a full application, this wouldn't make sense as its making the application more talky than it needs to be. If I were building it as a web app, I would contact the services directly from the frontend, only having a backend if required for authorisation. If I were building it out as a desktop app, the application layer would be replaced with a WPF project. 

## Application Layer
The application layer has been built using vertical slice architecture. This allows the request to be completely decoupled from any other request if more were to be created (See the potential extensions section for examples of what these would be). I've used dependency inversion to access the MusicBrainz and lyrics.ovh apis. This allows these services to be replaced if required. 

## Domain Layer
The domain layer defines the artist aggregate, creating a data contract between the external services and the application.

## External Services
Each external service uses an options pattern to define information required to access it. The MusicBrainz API has a 1 request per second limit, so there are some simple 1000ms delays to ensure this isn't exceeded. If an unsuccessful request is made, this is thrown as an exception and handled by the application layer. The lyrics.ovh service just logs exceptions. This is due to the fact it is an unreliable service, and always returns some errors. 

## Tests
I have used two kinds of tests, unit and subcutaneous tests. The unit tests test individual classes in the domain and application layer. Subcutaneous tests cover endpoints end to end.
 
# Potential Extensions
Due to time constraints, I've left out a few features that would enhance the functionality of the application. I've given a brief description of them here to show how they would be implemented. 

## Advanced search
Currently, the artist search returns a single artist that must match exactly the input. With a UI, we could separate the requests to search for an artist and get the artist's lyric stats. This would be done by returning all search results to the user, allowing them to select the correct artist. *GetLyricStats* would then use the artist ID as an input, rather than the artist name

## Sample Stats
For artists with a large catalogue, this application can take a while to run, due to external restraints. A second endpoint could be created that takes a sample of artist's tracks or releases and performs statistical analysis on those. 

## Handling lyrics service errors
Currently, any lyric service errors, including not found songs, are simply logged. There is scope to record this information and return it to the user. 

## More query options (Type, years, include duplicates, etc.)
Currently, the artist release search looks at all official albums. There are more options to query this, which could be input by the user.

## Rate limit from headers
To avoid rate limiting on the artist service, I've included a basic 1000ms wait. Information about the rate limit is returned in the response headers, and this could be utilised to make this wait as small as possible. 

## Retry/Timeout on Lyric Requests
As mentioned, the lyric service is unreliable and slow. There is scope to let users opt in to retrying failed lyric fetches, and also to allow users to set a timeout that would just perform the statistical analysis on the data it's managed to find within the allotted time. 

## A different Lyric API!
A different lyric service could be used, such as the genius API. This would require authentication. 

# Running
### Requirements
[.net Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)
### Commands
To run the program run the following commands from the Lyrico.Api folder.

`dotnet build`

`dotnet run`

The endpoints will be running on localhost:5000
## Api Definition
### Get LyricStats
`GET /Artists/LyricStats`
#### Query Parameters
Name | Type 
 ---|---
artist |string|

#### Response Structure
```
{
    "songsWithLyricsFound": 0,
    "mean": 0.0,
    "median": 0.0,
    "standardDeviation": 0.0,
    "variance": 0.0,
    "meanByRelease": {
        "" : 0.0
    }
}
```
#### Response Codes
 - 200
 - 404 
 - 503
 
 ## Appsettings
 The following app settings are used for the external services. They are defined in the Lyrico.Api/appsettings.json file.

 Name|Description|Default
 ---|---|---
 MusicBrainz.baseUrl | Base Url for musicbrainz api
 MusicBrainz.userAgent | Required by musicbrainz to identify who is making a request
  LyricsObh.baseUrl | Base Url for lyrics.ovh api
 LyricsObh.timeout | Time in seconds to wait before cancelling a request to the lyric api | 60