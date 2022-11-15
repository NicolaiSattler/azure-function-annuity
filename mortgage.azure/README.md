# Azure Function - HTTP Trigger

This project is made for educational purposes.

Pre-requisites:
- dotnet < 6.0
- azure core < 2.4
- azure function core tools version 4.x

## start project

`func start`


## Test function

`curl --request GET \ --url 'http://localhost:7071/api/CalculateAnnuity?loan=20000&years=10&interest=4'`
