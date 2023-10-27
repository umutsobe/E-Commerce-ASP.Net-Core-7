# ASP.NET Mini-E-Commerce App

![WhatsApp Image 2023-10-25 at 03 04 58](https://github.com/umutsobe/E-Commerce-ASP.Net-Core-7/assets/120561448/bcc473ae-d5ce-4a8c-b209-c9e91b08f50b)

**Before You Begin:** To try out the application, you can visit the following link: [E-Commerce App](http://206.81.31.147:4200). Please note that the initial API request may take 10-15 seconds to respond because I use Azure student subscription, but next requests will be much faster.

## Technologies

### .Net 7

- ASP.NET Core 7 Web API
- ASP.NET Identity
- JWT Authentication
- Refresh Token
- SignalR
- Entity Framework Core
- FluentValidatiton
- SignalR

## Architecture

- Onion Architecture
- CQRS Pattern
- Repository Pattern
- MediatR

## Hosting

- Docker
- Azure (before Docker)
- Heroku (before Docker)
- DigitalOcean Machine (now hosting)
- Jenkins

## Other Technologies

- Multiple Role Management (admin, moderator, normalUser ...)
- Mail Service
- Google Login
- Azure Blob Storage

## Solution Explorer Overview

![Screenshot 2023-10-25 031918](https://github.com/umutsobe/E-Commerce-ASP.Net-Core-7/assets/120561448/9aa36e16-e050-41ab-b902-b84dbd2a7d49)

## Bugs

- API requests are made twice because the frontend and backend are on the same machine (ssr problem). However, there is no problem when they are on separate machines.
