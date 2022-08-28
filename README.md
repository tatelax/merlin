![merlin-logo](https://github.com/tatelax/merlin/blob/master/assets/DALL%C2%B7E%202022-08-27%2022.40.51%20-%20Merlin%20the%20Wizard%20writing%20code%2C%20digital%20art.png)

<p align="center">
  <img src="https://github.com/tatelax/merlin/blob/master/assets/DALL%C2%B7E%202022-08-27%2022.40.51%20-%20Merlin%20the%20Wizard%20writing%20code%2C%20digital%20art.png">
</p>

# Merlin
This is the monorepo for Merlin. Merlin is an SDK for Unity that allows developers to easily create an application using Data Oriented Design while providing additional functions automatically such as real-time state read/write, analytics, improved overall code architecture, more testable code, and more.

## Repository Structure

### cloud-functions
Firebase cloud functions that automatically handle things such as generating an API key for users when their account is created.

### notes
Various markdown based notes about the project

### unity-demo
A demo project in Unity showing how to use Merlin.

### unity-sdk
The Unity SDK that users can install via UPM to use Merlin in their projects.

### web
The vue.js based web app users use to interact with their application.

### websocket-client-testing
A test application for the websocket client

### websocket-server
The websocket server that users applications will connect to which sends and receives data to/from their app to the web app.
