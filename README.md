# Aihr Calculator

The project allows to estimate, how many hours per week you need to spend to finish selected courses within selected time.

> **Note!** <br/>
> Unfortunately, I was not able to create `docker-compose` for a client part, nginx didn't want to proxy requests to backend. I spent two evening on it, so it feels like not the best place for such efforts (I'm not Angular expert for now).

## Get Started
1. `docker-compose up -d` will run BE and populate initial data in localstack
2. `cd client` && `npm i`
3. `npm run start` will run project with proxy and run http://localhost:4200
4. Now you can play with it.

> Everything has been tested with `Safari` browser only.

## Assumptions

| Topic                | Comment                                                                                                        |
| -------------------- | -------------------------------------------------------------------------------------------------------------- |
| Client Authorization | It's easy to implement with AWS SSM and JWT token but time consuming                                           |
| Study Week           | Considered to (24 * 7) week length                                                                             |
| History Update       | History will be updated, when the client asks for estimate (current client only)                               |
| Weekend or Holidays  | Not considered within estimation process                                                                       |
| Pagination           | Skipped, it's difficult to implement on FE for me now (just time related)                                      |
| History Limit        | No limit is here                                                                                               |
| Past Days            | Not available for user (but it could be, if use started study before and wants to know when he will finish it) |


## Possible improvements

- Better to have minimum delta between `StartDate` and `EndDate` calculated based on selected courses and their duration
- `State` implementation for storing selected courses in case of page reload

## Backend overwiew

- **Persistence layer:** DynamodDB
- **Cloud Provider:** Localstack
- **Testing:** Integration & UnitTests
- **Initial Data:** loading by `backend/localstack/setup-localstack.sh`