# ASP.NET-Core-and-Angular-2-Quickstart-Project
This project is based on the Angular 2 tutorial project- Tour of Heroes (https://angular.io/docs/ts/latest/tutorial/). In addition to the client app supplied by the tutorial I added an ASP.NET Core Web API backend to handle the application data and configured the two to work together. The backend uses a sqlite database (by default) behind the Entity Framework ORM. 

This project can be used as the starting point of an Angular 2 - ASP.NET Core web application. In order to run the project, clone the repository, complete the prerequisites, configure the backend, configure the client app and start developing. If there are any confusions along the way I will be happy to assist. Shoot me a quick message and I will try to answer as soon as possible.  

## Prerequisites

### Editor
I recommend using VS Code for this project. Find it here: https://code.visualstudio.com/

### .NET Core SDK
Install the .NET Core SDK. Find it here: https://www.microsoft.com/net/core

### Node JS
Install Node. Find it here: https://nodejs.org/en/


## Configure the Web API

configuring the Web API will require two steps: restoring the necessary dependencies and using the initial migration to set up the database. In addition to the default dependencies of a web api project I added the following:

**dotnet watch**: this add-on will track any changes done to the code and recompile the backend solution automatically. It allows smoother development since there is no need to restart the web server for every change done to the code. For more information on dotnet watch: https://docs.microsoft.com/en-us/aspnet/core/tutorials/dotnet-watch

**Entity Framework**: will serve as the ORM of the web api

**CORS (Cross Origin Resource Sharing)**: the API accepts requests that originated at the client domain (see Startup.cs, clientOrigin). The API will add to the preflight response an Access-Control-Allow-Origin so that it can communicate with the client.  

**Identity Framework**: added the Identity Framework for user registration. For more information: https://docs.microsoft.com/en-us/aspnet/identity/

**JWT Token Authentication**: the API generates a JWT token for registered users. This token will be used by the client in every request that needs authentiaction. The implementation of the JWT token is taken from Stormpath: https://stormpath.com/blog/token-authentication-asp-net-core. For more information on JWT in the HeroAPI see the [create an anchor](#Using-the-JWT) section.

---

__Restore the packages__

Open the terminal, cd into the folder HeroAPI and type: `dotnet restore`

__Migrate the initial database__

In the HeroAPI folder type: `dotnet ef database update`

This will create the sqlite database in the folder `HeroAPI/bin/Debug/HeroAPIDb.db`

__Start the web API web server__

In the HeroAPI directory type: `dotnet watch run`

__Test the server__

Open your browser and put the address: `localhost:5000/api/heroes`
you should see an empty list returned to you. Note: if the [Authorize] attribute is decorating a controller or an action then without a token it will return a 401 unauthorized response. See the Get JWT Token for more information.  


## Configure the Client App

To configure the client app we have to install of all the dependencies in the package.json file. After that we start the client server.

__Restore the packages__

In the terminal, cd into the ClientApp folder and type: `npm install`

__Start the client__

In the ClientApp folder type: `npm start`

Open the browser and type: `localhost:3000`. This should bring you to the main page of the application. 

__*Note__: make sure the backend is configured and running before the client 


## Using the JWT
