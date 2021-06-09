# Template for the .NET Core + PostgreSQL Database w/ Docker

This is a template project for any starting points, where someone wants to utilize PostgreSQL Database (together with the PgAdmin4 tool for easier navigation work with the database itself) with a .NET Core project (in a context of using a REST API, but naturally this can be modified for other goals) all running on Docker.

This version creates automatically a solution that stores a Database with 2 tables that depend one on another. One is a database with songs and the other with music genres. They are both generated through a `seed.sql` file in the __DBScripts__ folder that get's executed when a PosgreSQL Database is created with the `docker-compose.yaml` file.

To allow for easier interactivity with the Database and see what is going on with it - a PgAdmin4 tool is utilized and gets started with the execution of the `docker-compose.yaml` file and allows to access the Database on a port _localhost:5432_ in a browser. For information that you need to enter to access it, such as e-mail and password, please refer to the Docker compose file.

At the current stage you can use the .NET REST API component with the swagger interface. As I understand, because in line 6 of the _Dockerfile_ the line `ENV ASPNETCORE_ENVIRONMENT=Development` has been added, it is not available for utilization. It does not have many practical uses, especially because the entry points haven't been described, but this is just the basic template and you can go beyond it further with less modifications and just understand what is going on with it as is. But just to drive the point home, you have 3 entry points so far, that are used via 2 controllers. They are the following:

- GET METHODS:
  - http://localhost:5000/Database
    - Should return information on what kind of connection with PostgreSQL Database do you have. In the presented example it should be: `PostgreSQL version: PostgreSQL 13.3 (Debian 13.3-1.pgdg100+1) on x86_64-pc-linux-gnu, compiled by gcc (Debian 8.3.0-6) 8.3.0, 64-bit`
  - http://localhost:5000/Music/random
    - A request that will retrieve a random song from the database. When the database is initialized, it only has 4 songs in the music table. An example of a return is: `{"band": "Nirvana", "song": "Come as You Are", "style": "Grunge"}`
- PUT METHOD:
  - http://localhost:5000/​Music​/style​/{bandname}​/{songname}​/{genretype}
    - You can indicate a name of the band name a song and its genre and they will be added to the database. There are not many user inputs sanitization steps that are taken, so do not use this in production. Someone can literally drop your tables if you are not careful. This is just an example.

To see and try all the methods available, use the link at `http://localhost:5000/swagger/index.html`