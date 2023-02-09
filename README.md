# JWTAuthentication
A sample of service that utilize the .net core API and JWT process.


## How It Works?

UI will contact this API, JWT authentication, for user login. They will get a token which will allow the UI to talk with other related system (still being developed by now). Below is a simple descriptive diagram that I made using [Mermaid](https://mermaidjs.github.io/).

```mermaid
sequenceDiagram
User ->> UI: let me log in
UI ->> Authentication API: are this username and password valid?
Authentication API -->> UI : yes, here is a token for you to talk to others
UI -->> User: ok, you're in now
User ->> UI: nice, let me see something
UI ->> Other API: hey, may I get something from you? Here's a token as proof that I am real.
Other API ->> Authentication API : is this token valid? can you check?
Authentication API -->> Other API: yes, it is valid.
Other API -->> UI: here is something for you to show since your token is good and we are friends.
UI -->> User: here is the stuff you requested, man
```