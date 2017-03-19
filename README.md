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


## Configure Web API

configuring the Web API will require two steps: restoring the necessary dependencies and using the initial migration to set up the database. In addition to the default dependencies of a web api project I added the following:

-**dotnet watch**: this add-on will track any changes done to the code and recompile the backend solution automatically. It allows smoother development since there is no need to restart the web server for every change done to the code. For more information on dotnet watch: https://docs.microsoft.com/en-us/aspnet/core/tutorials/dotnet-watch

-**Entity Framework**: will serve as the ORM of the web api

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
you should see an empty list returned to you.


