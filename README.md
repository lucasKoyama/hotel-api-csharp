<style>
  .post, .get, .put, .delete { color: white; }

  .post { background-color: #489e77; }
  .get { background-color: #4c91ff; }
  .put { background-color: #e97500; }
  .delete { background-color: #cf3030; }

  .ok, .created { color: #62c173; }
  .badrequest, .unauthorized { color: red; }
</style>


# Endpoint HTTP Methods

### <code class="get">GET</code> /
<details>
  <summary class="ok">Response OK - 200</summary>

```json
{ "message": "online" }
```
</details>


## /booking

### <code class="post">POST</code> /booking
<details>
  <summary>Body</summary>

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
  <summary class="badrequest">Response Bad Request - 400</summary>
When trying to book more then the room capacity.

```json
{
  "message": "Guest quantity over room capacity"
}
```
</details>

<details>
  <summary class="badrequest">Response Unauthorized - 401</summary>
When there is no user logged in.

```json
{
  "message": "User not found!"
}
```
</details>

<details>
  <summary class="created">Response CREATED - 201</summary>

```json
{
  "bookingId": 1,
  "checkIn": "2030-08-27T00:00:00",
  "checkOut": "2030-08-28T00:00:00",
  "guestQuant": 1,
  "room": {
    "roomId": 1,
    "name": "Suite básica",
    "capacity": 2,
    "image": "image suite",
    "hotel": {
      "hotelId": 1,
      "name": "Trybe Hotel RJ",
      "address": "Avenida Atlântica, 1400",
      "cityId": 1,
      "cityName": "Rio de Janeiro"
    }
  }
}
```
</details>

### <code class="get">GET</code> /booking/{bookingId}
<details>
  <summary class="unauthorized">Response Unauthorized - 401</summary>
When there is no user logged in or the user is trying to access a booking that doesn't belong to him.

```json
{
  "message": "{{user.Name}} not Authorized! This booking doesn't belong to {user.Name}"
}
```
</details>

<details>
  <summary class="ok">Response OK - 200</summary>

```json
{
	"bookingId": 1002,
	"checkIn": "2023-08-27T00:00:00",
	"checkOut": "2023-08-28T00:00:00",
	"guestQuant": 1,
	"room": {
  	  "roomId": 1,
  	  "name": "Suite básica",
  	  "capacity": 2,
  	  "image": "image suite",
  	  "hotel": {
			"hotelId": 1,
  		  "name": "Trybe Hotel RJ",
  		  "address": "Avenida Atlântica, 1400",
  		  "cityId": 1,
  		  "cityName": "Rio de Janeiro"
  	  }
    }
}
```
</details>


##

### <code class="post">POST</code>
### <code class="get">GET</code>
### <code class="put">PUT</code>
### <code class="delete">DELETE</code>

### <code class="post">POST</code>
### <code class="get">GET</code>
### <code class="put">PUT</code>
### <code class="delete">DELETE</code>