# Template for the .NET Core + PostgreSQL Database w/ Docker

This is a template project that utilizes a combination of tools and solutions such as ASP.NET Core, PostgreSQL & PgAdmin on the Docker. The idea of the project was to provide an easy to start base for a very basic project that can be expanded further. Here main components were meant to be a simple backend that has connection to a Database system that can be additionally managed. For simplicity, not requiring the user to install everything on their computer - the systems work on Docker containers.

## Contents

Essentially this template has few of the following components created with it, to show how some operations can be utilized or modified for other purposes...

### Database

With this project, a database is initialized and stores 2 tables that depend one on another. One is a table with songs and the other with music genres. They are both generated through a `seed.sql` file in the __DBScripts__ folder that gets executed when a PostgreSQL Database is created with the `docker-compose.yaml` file.

### Interface to the Database

To allow for easier interactivity with the Database, a PgAdmin 4 tool is utilized. The tool gets initialized with the execution of the `docker-compose.yaml` file and can be accessed by navigating through your browser as such: _localhost:5432_. To know what username, password and e-mail that you need to enter when starting the tool, please refer to the `docker-compose.yaml` file, as it might be subject to change compare to what is written in this README.

### ASP.NET Web API

A very basic web app has been created that has few endpoints with which you can interact, which themselves interact with the Database further. At the current stage in the _Dockerfile_ file a line 6 has been added: `ENV ASPNETCORE_ENVIRONMENT=Development` that also allows for the swagger UI to be present for an easier interactivity with the API end-points, while you experiment with it.

The idea naturally is that you could see how several queries with default and modifiable parameters are being sent by the application to the database and how information gets retrieved from it. This template should provide a good reference point if you would like to move further with creating of your own application or expanding it.

## API Endpoints

- GET METHODS:
  - Available through: http://localhost:5000/Database
    - Should return information on what kind of connection with PostgreSQL Database do you have. You should see the following with the current build: `PostgreSQL version: PostgreSQL 13.3 (Debian 13.3-1.pgdg100+1) on x86_64-pc-linux-gnu, compiled by gcc (Debian 8.3.0-6) 8.3.0, 64-bit`
  - Available through: http://localhost:5000/Music/random
    - A request that will retrieve a random song from the database. The initialized database has initially only 4 songs in the music table. An example of a return you might get: `{"band": "Nirvana", "song": "Come as You Are", "style": "Grunge"}`
- PUT METHOD:
  - Available through: http://localhost:5000/​Music​/style​/{bandname}​/{songname}​/{genretype}
    - You can indicate a name of the band, name of a song and the genre that will be added to the database. There are not many user inputs sanitization steps that are implemented here for the simplicity sake. Only one that is present is one that checks whether the user's `genretype` entered value matches anything present in the database, or should the song provided by the user be categorizes as `Other`, because of how the system is built so far. However, due to this, do not use this minimum examples in production as I'm pretty sure someone can literally drop your tables with a malicious SQL injection statement.

To see and try all the methods available yourself and via an easy interface, use the link at `http://localhost:5000/swagger/index.html`

## Getting Started

0. Make sure you have Git and Docker installed on your computer and Docker is running
1. Clone the repository via `git clone https://github.com/Si-ja/.NET-Core-PostgreSQL-Docker-Template.git`
2. Navigate to the root of the repository
3. Call `docker-compose.up`
4. Have fun

## Imoportant Notice:

The links do only work on `http` side. You can use the api as is on port 5000, and swagger uses the same one but has a different extension.

You will run into issues if you will want to run the backend service as a service with say `dotnet watch run`. This is because the connection is created using an environmental variable and other type of connections are not implemented in the code.

## Roadmap for Current Fixes:
-[X] Add appsettings.json files for development and deployment environments
-[] Configurate docker-compose for development and deployment enviornments
-[] Fix the swagger interface and add ability to modify info on api routes
-[X] Implement Interfaces to make the current system more decoupled