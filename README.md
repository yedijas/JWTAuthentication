

# JWTAuthentication
A sample of service that utilize the .net core API and JWT process.


## How It Works?

UI will contact this API, JWT authentication, for user login. They will get a token which will allow the UI to talk with other related system (still being developed by now). Below is a simple descriptive diagram that I made using [Mermaid](https://mermaidjs.github.io/).

```mermaid
sequenceDiagram
participant A as User
participant B as UI
participant C as Authentication API
participant D as Other API

Note over A,B: Login Scheme
A ->> B:  let me log in
B ->> C:  are this username and password valid?
C -->> B: yes, here is a token for you to talk to others
B -->> A: ok, you're in now
A ->> B:  nice, let me see something
B ->> D:  hey, may I get something from you? Here's a token as proof that I am real.
D ->> C : is this token valid? can you check?
C -->> D: yes, it is valid.
D -->> B: here is something for you to show since your token is good and we are friends.
B -->> A: here is the stuff you requested, man

Note over B,C: Token Renewal Scheme
B ->> C:  hey, this token is expired!
C -->> B: my bad, here's the new stuff.
D ->> C:  this token is expired? what's the new one?
C -->> D: here. that user took so long to do his stuff, right.

Note over A,B: Logout scheme
A ->> B:  I have finished doing my things, let me out
B ->> C:  yo, this user wants out.
C ->> D:  guys, this token is no longer valid. so please reject it.
D -->> C: sure, its deleted from our side
C -->> B: I deleted his session and token, so he's good to leave
B -->> A: you're good to go back home.
```

So in order to properly use this, you need to fill in the audience list in the database. I add a controller for testing purpose. You may also need to prefill the user tables. Then you may test the token.
