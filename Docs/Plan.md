# App Idea
I will be making a web app that will allow users to play the [Wiki Game](https://en.wikipedia.org/wiki/Wikipedia:Wiki_Game). The game will be played in real time with other users. The game will be played in a single room with all users. The game will start when the host starts the game. The host will be able to set the start and end pages. The host will also be able to set the max number of players. The host will also be able to set the max number of rounds. The host will also be able to set the max time per round. The host will also be able to set the max time per game. The host will also be able to set the max number of players per


# Tech Stack

## Backend

### Server and Database
I will be using ASP.NET 8 with the MVC pattern to build the server side of the application. I will be using the Entity Framework to connect to a Postgres SQL database ware game state and account info will be stored. I plan to have a HostedService that will run every 5 minutes to check for games that have been inactive for 30 minutes and remove them from the database.

### Authentication
I will be using ASP.NET Identity to handle authentication and authorization. This will allow me to easily create and manage user accounts and roles. The UI for this is provided by the `Microsoft.VisualStudio.Web.CodeGeneration.Design` package. If a user is not logged in a anonymous account will be created for them, this will be saved in the database but will be removed after a set amount of time.

### Email service
I will be using SendGrid to send emails to users. This will be used to send emails to users when they create an account, reset their password.

## DevOps

### CI/CD
I will be using GitHub Actions to handle CI/CD. I will have a workflow that will run on every push to the master branch. This will build the application, run tests, and deploy the application to a server.

### Hosting
I will be hosting this on my own home server(a old laptop under my desk). The server will be runing fedora 39 as the host os with docker installed. I will be using docker-compose to manage the containers. I will be using caddy as a reverse proxy to handle ssl and routing to the correct container. 

### Domain Name and DNS
I have already purchased the domain name `wikirace.app` from cloudflare. I will be using cloudflare for managing the DNS records. I will also be using cloudflare for caching, ssl and ddos protection. 

### Docker
I will be using Docker to containerize the application. This will allow me to easily deploy the application to a server. The database will also be containerized using Docker. I will be using docker-compose to manage the containers. 

### Build System
For the server i will be using MSBuild to build the backend C# server. For the client i will be using esbuild to compile the typescript and css files into a single js file. It will also minify the js file.

## Frontend

### UI Framework
I will be using lit-element for rendering components on the client side. This will allow me to easily create reusable components. lit uses web components to create custom html elements that will be rendered on the client side. This will alow the server side razor pages to supply the initial data to the client side components in the html.

### CSS Framework
I will be using bootstrap for the css framework. This will allow me to easily create a responsive website. I plan on implmenting the [Catppuccin](https://github.com/catppuccin) theme for bootstrap. This will give the website a nice dark or light theme.

### Realtime Communication
I will be using SignalR to handle realtime communication between the client and server. This will be used to allow clients to notify the server when a user has navigated to a new page. This will also be used to notify clients when a user has won a game or when a new user joins the game.

# Wikipedia
Being the main source of data for the application, Wikipedia will be a key part of the application. There are varies way of getting this data all with pros and cons.

## Wikipedia API (Client Side)
I could have each client make requests to the Wikipedia API to get the data they need. This would be the easiest to implement but would be the slowest. This would also put a lot of load on the Wikipedia servers if there are a lot of users. However this would be the most up to date data. It would also remove the need for the server to parse and cache the any data.

## Wikipedia API (Server Side)
I could have the server make requests to the Wikipedia API to get the data. This would be faster than having the client make the requests. However the Wikipedia API is limited to 200/s requests. This would limit the number of users that could be playing at once.

## Mirror Wikipedia
This is by far the most complicated option but also the most scalable. I could mirror the entered of the english version of wikipedia. The compressed version of the english wikipedia is 22GB. However this is in wikipedia own markdown format wikitext. I would need to parse this into html. Thare is some existing tooling to do the like [dumpster-dive](https://github.com/spencermountain/dumpster-dive). However this would take a *LOT* of time to parse, [dumpster-dive](https://github.com/spencermountain/dumpster-dive) can convert all of en wikipedia to plain text in about 5hr. So converting this to html would take a lot longer. I would also need to keep this up to date with the latest changes to wikipedia. This would also take a lot of storage space. 

Ultimately i think i will be using the Wikipedia API on the client seeing as it will be the fastest and easiest to implement.