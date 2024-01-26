# Overview - Hotel API

<details>
  <summary>Summary</summary>

  1. [Tools used](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#tools-used)
  2. [Running Locally](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#running-locally)
  3. [Endpoint HTTP Methods](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#endpoint-http-methods)<br/>
    3.1 [API Status - endpoint /](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#api-status)<br/>
    3.2 [Endpoint /user](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#endpoint-user)<br/>
    3.3 [Endpoint /login](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#endpoint-login)<br/>
    3.4 [Endpoint /city](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#endpoint-city)<br/>
    3.5 [Endpoint /hotel](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#endpoint-hotel)<br/>
    3.6 [Endpoint /room](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#endpoint-room)<br/>
    3.7 [Endpoint /booking](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#endpoint-booking)<br/>
    3.8 [Endpoint /geo](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#endpoint-geo)<br/>
  4. [Wanna learn about C# and WebAPIs?](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#wanna-learn-about-c-and-webapis)
  5. [Authors](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#authors)
  6. [Styles for cloned repo!](https://github.com/lucasKoyama/hotel-api-csharp/tree/main?tab=readme-ov-file#styles-for-cloned-repo)
</details>

This project is an API to manage and use a network of hotels. It has user creation, login with authentication, and the creation of cities, hotels, rooms and bookings in the database. To create hotels and rooms, the user must be admin (admin authorization). It also has a geo-localization feature to measure the user's location distance to the registered hotels!

# Tools used
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

# Running Locally
1. Clone this repository: `git clone https://github.com/lucasKoyama/hotel-api-csharp.git`
2. Go to project folder: `cd hotel-api-csharp`
3. Inside project folders run docker: `docker-compose up -d --build`
4. run the script `init.sh` it will wait until the database is up to run a migration script
5. Make an http request to check API status `http://localhost:5501/`

# Endpoint HTTP Methods

## API Status

### <code class="get">GET</code> /
<details>
  <summary class="ok">Response OK - 200</summary>

```json
{ "message": "online" }
```
</details>

## Endpoint /user

### <code class="post">POST</code> /user
<details>
  <summary>📃 Body</summary>

```json
{
  "Name":"John Doe",
  "Email": "Johndoe@gmail.com",
  "Password": "123456"
}
```
</details>

<details>
  <summary class="created">✅ Response CREATED - 201</summary>

```json
{
  "UserId": 1,
  "Name":"John Doe",
  "Email": "Johndoe@gmail.com",
  "UserType": "client"
}
```
</details>

<details>
  <summary class="conflict">❌ Response CONFLICT - 409</summary>

```json
{
  "message": "User email already exists"
}
```
</details>

### <code class="get">GET</code> /user
Get All Users
<p>🔐 Admin user only<p>
<details>
  <summary class="ok">✅ Response OK - 200</summary>

```json
[
  {
    "UserId": 1,
    "Name":"John Doe",
    "Email": "Johndoe@gmail.com",
    "UserType": "client"
  },
  /*Other users...*/
]
```
</details>

<details>
  <summary class="badrequest">❌ Response Unauthorized - 401</summary>
When there is no admin user
</details>

## Endpoint /login

### <code class="post">POST</code> /login
<details>
  <summary>📃 Body</summary>

```json
{
  "Email": "Johndoe@gmail.com",
  "Password": "123456"
}
```
</details>

<details>
  <summary class="ok">✅ Response OK - 200</summary>

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYWRtaW4iLCJlbWFpbCI6ImRhbmlsby5zaWx2YUBiZXRyeWJlLmNvbSIsIm5iZiI6MTY4ODQxMTIxMiwiZXhwIjoxNjg4NDk3NjEyLCJpYXQiOjE2ODg0MTEyMTJ9.q1cNj2_xspeQC6Uz1maV79P95hVtWH4Z7auZgOen-Qo",
}
```
</details>

<details>
  <summary class="badrequest">❌ Response Unauthorized - 401</summary>
Incorrect email or password

```json
{
  "message": "Incorrect e-mail or password"
}
```
</details>


## Endpoint /city

### <code class="post">POST</code> /city
<details>
  <summary>📃 Body</summary>

```json
{
  "Name": "São Paulo",
  "State": "SP"
}
```
</details>

<details>
  <summary class="created">✅ Response CREATED - 201</summary>

```json
{
  "CityId": 1,
  "Name": "São Paulo",
  "State": "SP"
}
```
</details>

### <code class="get">GET</code> /city
Get All Cities
<details>
  <summary class="ok">✅ Response OK - 200</summary>

```json
[
  {
    "CityId": 1,
    "Name": "São Paulo",
    "State": "SP"
  },
  /*Other cities...*/
]
```
</details>

### <code class="put">PUT</code> /city
<details>
  <summary>📃 Body</summary>

```json
{
  "CityId": 1,
  "Name": "Rio de Janeiro",
  "State": "RJ"
}
```
</details>

<details>
  <summary class="ok">✅ Response OK - 200</summary>

```json
{
  "CityId": 1,
  "Name": "Rio de Janeiro",
  "State": "RJ"
}
```
</details>

## Endpoint /hotel

### <code class="post">POST</code> /hotel
<p>🔐 Admin user only<p>
<details>
  <summary>📃 Body</summary>

```json
{
  "Name": "Trybe Hotel SP",
  "Address": "Avenida Paulista, 1400",
  "CityId": 1,
}
```
</details>

<details>
  <summary class="created">✅ Response CREATED - 201</summary>

```json
{
  "HotelId": 1,
  "Name": "Trybe Hotel SP",
  "Address": "Avenida Paulista, 1400",
  "CityId": 1,
  "CityName": "São Paulo",
  "State": "SP"
}
```
</details>

<details>
  <summary class="badrequest">❌ Response Unauthorized - 401</summary>
When there is no admin user
</details>

### <code class="get">GET</code> /hotel
Get All Hotels
<details>
  <summary class="ok">✅ Response OK - 200</summary>

```json
[
  {
    "HotelId": 1,
    "Name": "Trybe Hotel SP",
    "Address": "Avenida Paulista, 1400",
    "CityId": 1,
    "CityName": "São Paulo",
    "State": "SP"
  },
  /*Other hotels...*/
]
```
</details>

## Endpoint /room

### <code class="post">POST</code> /room
<p>🔐 Admin user only<p>

<details>
  <summary>📃 Body</summary>

```json
{
  "Name":"Suite básica",
  "Capacity":2,
  "Image":"image suite",
  "HotelId": 1
}
```
</details>

<details>
  <summary class="created">✅ Response CREATED - 201</summary>

```json
{
  "RoomId": 1,
  "Name": "Suite básica",
  "Capacity": 2,
  "Image": "image suite",
  "Hotel": {
    "HotelId": 1,
    "Name": "Trybe Hotel SP",
    "Address": "Avenida Paulista, 1400",
    "CityId": 1,
    "CityName": "São Paulo",
    "State": "SP"
  }
}
```
</details>

<details>
  <summary class="badrequest">❌ Response Unauthorized - 401</summary>
When there is no admin user
</details>

### <code class="get">GET</code> /room/{hotelId}
Get All Rooms of a specified Hotel
<details>
  <summary class="ok">✅ Response OK - 200</summary>

```json
[
  {
    "RoomId": 1,
    "Name": "Suite básica",
    "Capacity": 2,
    "Image": "image suite",
    "Hotel": {
      "hotelId": 1,
      "Name": "Trybe Hotel SP",
      "Address": "Avenida Paulista, 1400",
      "CityId": 1,
      "CityName": "São Paulo",
      "State": "SP"
    }
  }
  /*Other rooms...*/
]
```
</details>

### <code class="delete">DELETE</code> /room/{roomId}
<p>🔐 Admin user only<p>

<details>
  <summary class="no-content">✅ Response NoContent - 204</summary>
</details>

<details>
  <summary class="badrequest">❌ Response Unauthorized - 401</summary>
When there is no admin user
</details>

## Endpoint /booking

### <code class="post">POST</code> /booking
<details>
  <summary>📃 Body</summary>

```json
{
  "CheckIn":"2030-08-27",
  "CheckOut":"2030-08-28",
  "GuestQuant":"1",
  "RoomId":1
}
```
</details>

<details>
  <summary class="created">✅ Response CREATED - 201</summary>

```json
{
  "BookingId": 1,
  "CheckIn": "2030-08-27T00:00:00",
  "CheckOut": "2030-08-28T00:00:00",
  "GuestQuant": 1,
  "Room": {
    "RoomId": 1,
    "Name": "Suite básica",
    "Capacity": 2,
    "Image": "image suite",
    "Hotel": {
      "HotelId": 1,
      "Name": "Trybe Hotel RJ",
      "Address": "Avenida Atlântica, 1400",
      "CityId": 1,
      "CityName": "Rio de Janeiro"
    }
  }
}
```
</details>

<details>
  <summary class="badrequest">❌ Response Bad Request - 400</summary>
When trying to book more then the room capacity.

```json
{
  "message": "Guest quantity over room capacity"
}
```
</details>

<details>
  <summary class="badrequest">❌ Response Unauthorized - 401</summary>
When there is no user logged in.

```json
{
  "message": "User not found!"
}
```
</details>

### <code class="get">GET</code> /booking/{bookingId}
<details>
  <summary class="ok">✅ Response OK - 200</summary>

```json
{
  "BookingId": 1002,
  "CheckIn": "2023-08-27T00:00:00",
  "CheckOut": "2023-08-28T00:00:00",
  "GuestQuant": 1,
  "Room": {
    "RoomId": 1,
    "Name": "Suite básica",
    "Capacity": 2,
    "Image": "image suite",
    "Hotel": {
      "HotelId": 1,
      "Name": "Trybe Hotel RJ",
      "Address": "Avenida Atlântica, 1400",
      "CityId": 1,
      "CityName": "Rio de Janeiro"
    }
  }
}
```
</details>

<details>
  <summary class="unauthorized">❌ Response Unauthorized - 401</summary>
When the booking belongs to another user.

```json
{
  "message": "{{user.Name}} not Authorized! This booking doesn't belong to {user.Name}"
}
```
</details>

<details>
  <summary class="unauthorized">❌ Response Unauthorized - 401</summary>
When the booking belongs to another user.

```json
{
  "message": "User not found!"
}
```
</details>

<details>
  <summary class="not-found">❌ Response Not Found - 404</summary>
When the booking belongs to another user.

```json
{
  "message": "Booking not found!"
}
```
</details>

## Endpoint /geo

### <code class="get">GET</code> /geo/status
<details>
  <summary class="ok">✅ Response OK - 200</summary>

```json
{
  "Status": 0,
  "Message": "OK",
  "Data_updated": "2020-05-04T14:47:00+00:00",
  "Software_version": "3.6.0-0",
  "Database_version": "3.6.0-0"
}
```
</details>

### <code class="get">GET</code> /geo/address
<details>
  <summary>📃 Body</summary>

```json
{
  "Address":"Rua Arnaldo Barreto",
  "City":"Campinas",
  "State":"SP"
}
```
</details>

<details>
  <summary class="ok">✅ Response OK - 200</summary>

```json
  [
    {
      "HotelId": 1,
      "Name": "Trybe Hotel SP",
      "Address": "Avenida Paulista, 2000",
      "CityName": "São Paulo",
      "State": "SP",
      "Distance": 82
    },
    {
      "HotelId": 2,
      "Name": "Trybe Hotel RJ",
      "Address": "Avenida Atlântica, 1400",
      "CityName": "Rio de Janeiro",
      "State": "RJ",
      "Distance": 399
    },
    /*Other Hotels...*/
  ]
```
</details>
<br/>

# Wanna learn about C# and WebAPIs?
Checkout the [Gist](https://gist.github.com/lucasKoyama/2078263386f130516e2a5b778a0b073e) that I did during this API development!

# Authors
- [lucasKoyama](https://github.com/lucasKoyama)

# Styles for cloned repo!
<style>
  .post, .get, .put, .delete { color: white; }

  .post { background-color: #489e77; }
  .get { background-color: #4c91ff; }
  .put { background-color: #e97500; }
  .delete { background-color: #cf3030; }

  .ok, .created, .no-content { color: #62c173; }
  .badrequest, .unauthorized, .not-found, .conflict { color: red; }
</style>
