# Overview
This document contains an architectural overview of my solution to the technical test. 

## Presentation Layer
I have built the presentation layer as a simple API. This was done as an easy way to run the application and send test requests. For a full application, this wouldn't make sense as an API, as it's making the application more talky than it needs to be. If I were building it as a web app, I would contact the services directly from the frontend, only having a backend if I needed to store any keys. If I were building it out as a desktop app, the application layer would be replaced with a WPF project. 
 
## Potential Extensions
Due to time constraints, I've left out a few features that would enhance the functionality of the application. I've given a brief description of them here to show how they would be implemented. 

### Advanced search
- Currently returns first artist found
- Add a second endpoint that would return a list of artists, including there IDs
- This could be displayed in a table in UI for a user to select the correct artist
- GetLyricStats would then accept an artist ID rather than an artist name

### Sample Stats
- For artists with a large catalogue, this takes a long time
- Give users the option to get statistics from a sample of the data, rather than the full set