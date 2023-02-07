# Web API with ASP.NET Core and PostgreSQL Database
A assignment based in the [Create a web API with ASP.NET Core](https://https://github.com/nhk1978/TodoAPI) to use a PostgreSQL Database.

## First steps
Follow the steps in https://https://github.com/nhk1978/TodoAPI repository, as that is the starting point.

## Tutorial Steps
### Add the PostgreSQL Provider
```
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

### Create tables on PostgreSQL
run script_er.sql to create 2 tables: user and todo and a type todo_status
```
### Run profile TodoAPI

### On Postman, Run testcases
POST http://localhost:7979/api/v1/signup
Body in JSON
{
    "email": "abc123@abc.abc",
    "password": "hahaha"
}
->Success
Body in JSON
{
    "email": "abc123@abc.abc",
    "password": "1234567"
}
->Fail
POST http://localhost:7979/api/v1/signin
Body in JSON
{
    "email": "abc123@abc.abc",
    "password": "hahaha"
}

Using bearer token from this step for following APIs
->Success
Body in JSON
{
    "email": "abc123@abc.abc",
    "password": "hahaha"
}
->Fail
PUT http://localhost:7979/api/v1/changePassword/newpass
Body in JSON
{
    "email": "abc123@abc.abc",
    "password": "hahaha"
}
->Success
PUT http://localhost:7979/api/v1/changePassword/newpass
Body in JSON
{
    "email": "abc123@abc.abc",
    "password": "hahaha"
}
->Fail
POST http://localhost:7979/api/v1/todos
Body in JSON
{
    "name": "abcfffsssssssff",
    "description": "desffffssssssssffffffff77777fff1",
    "userid":1,
    "status":1
}
->Success
POST http://localhost:7979/api/v1/todos
Body in JSON
{
    "name": "abcfffsssssssff",
    "description": "desffffssssssssffffffff77777fff1",
    "userid":6,
    "status":1
}
->Fail
PUT http://localhost:7979/api/v1/todos/1
Body in JSON
{
    "name": "abcddddddddddddd",
    "description": "des1555aaaaaaaaaaaaaaaaaaaaaaahhhhhhhhh5",
    "userid":1,
    "status":2
}
->Success
PUT http://localhost:7979/api/v1/todos/1
Body in JSON
{
    "name": "abcddddddddddddd",
    "description": "des1555aaaaaaaaaaaaaaaaaaaaaaahhhhhhhhh5",
    "userid":1,
    "status":2
}
->Success
GET http://localhost:7979/api/v1/todos?status=Completed
->Success
GET http://localhost:7979/api/v1/todos?status=OnGoing
->Fail
DELETE http://localhost:7979/api/v1/todos/1
->Success
DELETE http://localhost:7979/api/v1/todos/1
->Fail
