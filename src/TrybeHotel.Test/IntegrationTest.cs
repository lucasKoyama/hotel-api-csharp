namespace TrybeHotel.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Diagnostics;
using System.Xml;
using System.IO;
using TrybeHotel.Dto;

public class LoginJson {
    public string? token { get; set; }
}

public class CityPost {
    public string? Name { get; set; }
    public string? State { get; set; }
}

public class HotelPost {
    public string? Name { get; set; }
    public string? Address { get; set; }
    public int CityId { get; set; }
}

public class RoomPost {
    public string? Name { get; set; }
    public int Capacity { get; set; }
    public string? Image { get; set; }
    public int HotelId { get; set; }
}

public class ErrorJson {
    public string? message { get; set; }
}

public class IntegrationTest: IClassFixture<WebApplicationFactory<Program>>
{
     public HttpClient _clientTest;

     public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        //_factory = factory;
        _clientTest = factory.WithWebHostBuilder(builder => {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TrybeHotelContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ContextTest>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDatabase");
                });
                services.AddScoped<ITrybeHotelContext, ContextTest>();
                services.AddScoped<ICityRepository, CityRepository>();
                services.AddScoped<IHotelRepository, HotelRepository>();
                services.AddScoped<IRoomRepository, RoomRepository>();
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<ContextTest>())
                {
                    appContext.Database.EnsureCreated();
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                    appContext.Cities.Add(new City {CityId = 1, Name = "Manaus", State = "AM"});
                    appContext.Cities.Add(new City {CityId = 2, Name = "Palmas", State = "TO"});
                    appContext.SaveChanges();
                    appContext.Hotels.Add(new Hotel {HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1});
                    appContext.Hotels.Add(new Hotel {HotelId = 2, Name = "Trybe Hotel Palmas", Address = "Address 2", CityId = 2});
                    appContext.Hotels.Add(new Hotel {HotelId = 3, Name = "Trybe Hotel Ponta Negra", Address = "Addres 3", CityId = 1});
                    appContext.SaveChanges();
                    appContext.Rooms.Add(new Room { RoomId = 1, Name = "Room 1", Capacity = 2, Image = "Image 1", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 2, Name = "Room 2", Capacity = 3, Image = "Image 2", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 3, Name = "Room 3", Capacity = 4, Image = "Image 3", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 4, Name = "Room 4", Capacity = 2, Image = "Image 4", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 5, Name = "Room 5", Capacity = 3, Image = "Image 5", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 6, Name = "Room 6", Capacity = 4, Image = "Image 6", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 7, Name = "Room 7", Capacity = 2, Image = "Image 7", HotelId = 3 });
                    appContext.Rooms.Add(new Room { RoomId = 8, Name = "Room 8", Capacity = 3, Image = "Image 8", HotelId = 3 });
                    appContext.Rooms.Add(new Room { RoomId = 9, Name = "Room 9", Capacity = 4, Image = "Image 9", HotelId = 3 });
                    appContext.SaveChanges();
                    appContext.Users.Add(new User { UserId = 1, Name = "Ana", Email = "ana@trybehotel.com", Password = "Senha1", UserType = "admin" });
                    appContext.Users.Add(new User { UserId = 2, Name = "Beatriz", Email = "beatriz@trybehotel.com", Password = "Senha2", UserType = "client" });
                    appContext.Users.Add(new User { UserId = 3, Name = "Laura", Email = "laura@trybehotel.com", Password = "Senha3", UserType = "client" });
                    appContext.SaveChanges();
                    appContext.Bookings.Add(new Booking { BookingId = 1, CheckIn = new DateTime(2023, 07, 02), CheckOut = new DateTime(2023, 07, 03), GuestQuant = 1, UserId = 2, RoomId = 1});
                    appContext.Bookings.Add(new Booking { BookingId = 2, CheckIn = new DateTime(2023, 07, 02), CheckOut = new DateTime(2023, 07, 03), GuestQuant = 1, UserId = 3, RoomId = 4});
                    appContext.SaveChanges();
                }
            });
        }).CreateClient();
    }
 
   // standard way of passing the object to be created
    public StringContent create(object objectToCreate)
    {
        return new StringContent (
            JsonConvert.SerializeObject(objectToCreate),
            System.Text.Encoding.UTF8,
            "application/json"
        );
    }

    public async Task<T> responseDtoToType<T>(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseString);
    }

        public async Task<string> GetAdminToken()
    {
        var loginAction = new {
            Email = "ana@trybehotel.com",
            Password = "Senha1"
        };
        var response = await _clientTest.PostAsync("/login", create(loginAction));
        
        LoginJson admin = await responseDtoToType<LoginJson>(response);
        return admin.token;
    }
    
    public async Task<string> GetClientToken()
    {
        var loginAction = new {
            Email = "beatriz@trybehotel.com",
            Password = "Senha2"
        };
        var response = await _clientTest.PostAsync("/login", create(loginAction));
        
        LoginJson client = await responseDtoToType<LoginJson>(response);
        return client.token;
    }

    // USER
    [Trait("Category", "Testing endpoint /user methods")]
    [Theory(DisplayName = "POST, Response STATUS = 201, Response JSON = created User")]
    [InlineData("/user")]
    public async Task TestPostUser(string url)
    {
       var newUser = new {
            Name = "Maria",
            Email = "mariab@trybehotel.com",
            Password = "123456"
        };
        var response = await _clientTest.PostAsync(url, create(newUser));

        UserDto jsonResponse = await responseDtoToType<UserDto>(response);

        Assert.Equal(System.Net.HttpStatusCode.Created, response?.StatusCode);
        Assert.Equal(4, jsonResponse.UserId);
        Assert.Equal("Maria", jsonResponse.Name);
        Assert.Equal("mariab@trybehotel.com", jsonResponse.Email);
        Assert.Equal("client", jsonResponse.UserType);
    }

    [Trait("Category", "Testing endpoint /user methods")]
    [Theory(DisplayName = "Admin GET ALL, Response STATUS = 200, Response JSON = all Users")]
    [InlineData("/user")]
    public async Task TestUserControllerGetResponse(string url)
    {
        var adminToken = await GetAdminToken();
        _clientTest.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _clientTest.GetAsync(url);

        List<UserDto> jsonResponse = await responseDtoToType<List<UserDto>>(response);
        jsonResponse = jsonResponse.OrderBy(u => u.UserId).ToList();

        Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
        Assert.Contains("ana@trybehotel.com", jsonResponse[0].Email);
        Assert.Contains("beatriz@trybehotel.com", jsonResponse[1].Email);
        Assert.Contains("laura@trybehotel.com", jsonResponse[2].Email);

        Assert.Contains("admin", jsonResponse[0].UserType);
        Assert.Contains("client", jsonResponse[1].UserType);
        Assert.Contains("client", jsonResponse[2].UserType);
    }

    // LOGIN
    [Trait("Category", "Testing endpoint /login methods")]
    [Theory(DisplayName = "POST, Response STATUS = 200, Response JSON = token")]
    [InlineData("/login")]
    public async Task TestPostLogin(string url)
    {
        var loginAction = new {
            Email = "ana@trybehotel.com",
            Password = "Senha1"
        };
        var response = await _clientTest.PostAsync(url, create(loginAction));
        
        LoginJson jsonResponse = await responseDtoToType<LoginJson>(response);

        Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
        Assert.NotEmpty(jsonResponse.token);
    }

    [Trait("Category", "Testing endpoint /login methods")]
    [Theory(DisplayName = "POST, Response STATUS = 401, Response Unauthorized")]
    [InlineData("/login")]
    public async Task TestPostLoginUnauthorized(string url)
    {
        var loginAction = new {
            Email = "ana@trybehotel.com",
            Password = "Senha16"
        };
        var response = await _clientTest.PostAsync(url, create(loginAction));

        ErrorJson jsonResponse = await responseDtoToType<ErrorJson>(response);

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response?.StatusCode);
        Assert.Equal("Incorrect e-mail or password", jsonResponse.message);
    }

    // CITY
    [Trait("Category", "Testing endpoint /city methods")] // Trait used to group into a category of tests
    [Theory(DisplayName = "POST, Response STATUS = 201, Response JSON = created City")] // Theory used to describe what is to happen, and is required to have a InlineData
    [InlineData("/city")] // InlineData its the data passed to the testing function
    public async Task TestPostCity(string url) // url => InlineData passed, in this case /city
    {
        // Requesting
        var newCity = new CityPost {
            Name = "Rio de Janeiro",
            State = "RJ"
        };
        var response = await _clientTest.PostAsync(url, create(newCity));
        
        // ?? Data treament ??
        CityDto jsonResponse = await responseDtoToType<CityDto>(response);

        // Testing returned data
        Assert.Equal(System.Net.HttpStatusCode.Created, response?.StatusCode);
        Assert.Equal(3, jsonResponse.CityId);
        Assert.Equal("Rio de Janeiro", jsonResponse.Name);
        Assert.Equal("RJ", jsonResponse.State);
    }

    [Trait("Category", "Testing endpoint /city methods")]
    [Theory(DisplayName = "GET ALL, Response STATUS = 200, Response JSON = all Cities")]
    [InlineData("/city")]
    public async Task TestGetCities(string url)
    {
        var response = await _clientTest.GetAsync(url);

        List<CityDto> jsonResponse = await responseDtoToType<List<CityDto>>(response);
        
        Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
        Assert.Contains("Manaus", jsonResponse[0].Name);
        Assert.Contains("Palmas", jsonResponse[1].Name);

        Assert.Contains("AM", jsonResponse[0].State);
        Assert.Contains("TO", jsonResponse[1].State);
    }

    // HOTEL
    [Trait("Category", "Testing endpoint /hotel methods")]
    [Theory(DisplayName = "Admin POST, Response STATUS = 201, Response JSON = created Hotel")]
    [InlineData("/hotel")]
    public async Task TestAdminPostHotel(string url)
    {
        var adminToken = await GetAdminToken();
        _clientTest.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        var newHotel = new HotelPost {
            Name = "Manaus Hotel",
            Address = "Avenida Manaus, 123",
            CityId =  1
        };
        var response = await _clientTest.PostAsync(url, create(newHotel));

        HotelDto jsonResponse = await responseDtoToType<HotelDto>(response);

        Assert.Equal(System.Net.HttpStatusCode.Created, response?.StatusCode);
        Assert.Equal(4,                     jsonResponse.HotelId);
        Assert.Equal("Manaus Hotel",        jsonResponse.Name);
        Assert.Equal("Avenida Manaus, 123", jsonResponse.Address);
        Assert.Equal(1,                     jsonResponse.CityId);
        Assert.Equal("Manaus",              jsonResponse.CityName);
        Assert.Contains("AM",               jsonResponse.State);
    }

    [Trait("Category", "Testing endpoint /hotel methods")]
    [Theory(DisplayName = "Client Trying POST, Response STATUS 403, Response Forbidden")]
    [InlineData("/hotel")]
    public async Task TestClientTryingPostHotel(string url)
    {
        var clientToken = await GetClientToken();
        _clientTest.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);
    
        var newHotel = new {
            Name = "New Trybe Hotel Palmas",
            Address = "Address 4",
            Cityid = 2
        };
        var response = await _clientTest.PostAsync(url, create(newHotel));

        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response?.StatusCode);
    }

    [Trait("Category", "Testing endpoint /hotel methods")]
    [Theory(DisplayName = "Trying POST, Response STATUS 403, Response Unauthorized")]
    [InlineData("/hotel")]
    public async Task TestTryingPostHotel(string url)
    {
        var newHotel = new {
            Name = "New Trybe Hotel Palmas",
            Address = "Address 4",
            Cityid = 2
        };
        var response = await _clientTest.PostAsync(url, create(newHotel));

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response?.StatusCode);
    }

    [Trait("Category", "Testing endpoint /hotel methods")]
    [Theory(DisplayName = "GET ALL, Response STATUS = 200, Response JSON = all Hotels")]
    [InlineData("/hotel")]
    public async Task TestGetHotels(string url)
    {
        var response = await _clientTest.GetAsync(url);

        List<HotelDto> jsonResponseNotOrder = await responseDtoToType<List<HotelDto>>(response);
        List<HotelDto> jsonResponse = jsonResponseNotOrder.OrderBy(item => item.HotelId).ToList();
        
        Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
        Assert.Contains("Trybe Hotel Manaus", jsonResponse[0].Name);
        Assert.Contains("Trybe Hotel Palmas", jsonResponse[1].Name);
        Assert.Contains("Trybe Hotel Ponta Negra", jsonResponse[2].Name);

        Assert.Contains("Address 1", jsonResponse[0].Address);
        Assert.Contains("Address 2", jsonResponse[1].Address);
        Assert.Contains("Addres 3", jsonResponse[2].Address);
        
        Assert.Equal(1, jsonResponse[0].CityId);
        Assert.Equal(2, jsonResponse[1].CityId);
        Assert.Equal(1, jsonResponse[2].CityId);

        Assert.Contains("AM", jsonResponse[0].State);
        Assert.Contains("TO", jsonResponse[1].State);
        Assert.Contains("AM", jsonResponse[2].State);
    }

    // ROOM
    [Trait("Category", "Testing endpoint /room methods")]
    [Theory(DisplayName = "Admin POST, Response STATUS = 201, Response JSON = created Room")]
    [InlineData("/room")]
    public async Task TestAdminPostRoom(string url)
    {
        var adminToken = await GetAdminToken();
        _clientTest.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        var newRoom = new RoomPost {
            Name = "Suite básica",
            Capacity = 8,
            Image = "Image suite",
            HotelId = 3
        };
        var response = await _clientTest.PostAsync(url, create(newRoom));

        RoomDto jsonResponse = await responseDtoToType<RoomDto>(response);

        Assert.Equal(System.Net.HttpStatusCode.Created, response?.StatusCode);
        Assert.Equal(10, jsonResponse.RoomId);
        Assert.Contains("Suite básica", jsonResponse.Name);
        Assert.Equal(8, jsonResponse.Capacity);
        Assert.Contains("Image suite", jsonResponse.Image);
        Assert.Contains("Trybe Hotel Ponta Negra", jsonResponse.Hotel.Name);
        Assert.Contains("Manaus", jsonResponse.Hotel.CityName);
        Assert.Contains("AM", jsonResponse.Hotel.State);
    }

    [Trait("Category", "Testing endpoint /room methods")]
    [Theory(DisplayName = "Client Trying POST, Response STATUS = 403, Response Forbidden")]
    [InlineData("/room")]
    public async Task TestClientTryingPostRoom(string url)
    {
        var clientToken = await GetClientToken();
        _clientTest.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);
    
        var newRoom = new RoomPost {
            Name = "Suite básica",
            Capacity = 2,
            Image = "image suite",
            HotelId = 1
        };
        var response = await _clientTest.PostAsync(url, create(newRoom));

        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response?.StatusCode);
    }

    [Trait("Category", "Testing endpoint /room methods")]
    [Theory(DisplayName = "Trying POST, Response STATUS = 401, Response Unauthorized")]
    [InlineData("/room")]
    public async Task TestTryingPostRoom(string url)
    {
        var newRoom = new RoomPost {
            Name = "Suite básica",
            Capacity = 2,
            Image = "image suite",
            HotelId = 1
        };
        var response = await _clientTest.PostAsync(url, create(newRoom));

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response?.StatusCode);
    }

    [Trait("Category", "Testing endpoint /room methods")]
    [Theory(DisplayName = "GET ALL BY HotelId, Response STATUS = 200, Response JSON = all rooms by HotelId")]
    [InlineData("/room/1")]
    public async Task TestGetRoom(string url)
    {
        var response = await _clientTest.GetAsync(url);

        List<RoomDto> jsonResponseNotOrder = await responseDtoToType<List<RoomDto>>(response);
        List<RoomDto> jsonResponse = jsonResponseNotOrder.OrderBy(item => item.RoomId).ToList();

        Assert.Equal(1, jsonResponse[0].RoomId);
        Assert.Contains("Room 1", jsonResponse[0].Name);
        Assert.Equal(2, jsonResponse[0].Capacity);
        Assert.Contains("Image 1", jsonResponse[0].Image);
        Assert.Contains("Trybe Hotel Manaus", jsonResponse[0].Hotel.Name);
        Assert.Contains("Manaus", jsonResponse[0].Hotel.CityName);
        Assert.Contains("AM", jsonResponse[0].Hotel.State);

        Assert.Equal(2, jsonResponse[1].RoomId);
        Assert.Contains("Room 2", jsonResponse[1].Name);
        Assert.Equal(3, jsonResponse[1].Capacity);
        Assert.Contains("Image 2", jsonResponse[1].Image);
        Assert.Contains("Trybe Hotel Manaus", jsonResponse[1].Hotel.Name);
        Assert.Contains("Manaus", jsonResponse[1].Hotel.CityName);
        Assert.Contains("AM", jsonResponse[1].Hotel.State);
    }
    
    [Trait("Category", "Testing endpoint /room methods")]
    [Theory(DisplayName = "Admin DELETE ONE BY ID, Response STATUS = 204, Response JSON = no content")]
    [InlineData("/room/1")]
    public async Task TestAdminGetRoomsByHotelId(string url)
    {
        var adminToken = await GetAdminToken();
        _clientTest.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _clientTest.DeleteAsync(url);
        
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response?.StatusCode);
    }
   
    // BOOKING
    [Trait("Category", "Testing endpoint /booking methods")]
    [Theory(DisplayName = "Admin POST, Response STATUS = 201, Response JSON = created Booking")]
    [InlineData("/booking")]
    public async Task TestAdminPostBooking(string url)
    {
        var adminToken = await GetAdminToken();
        _clientTest.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        var newBooking = new {
            CheckIn = "2030-08-27",
            CheckOut = "2030-08-28",
            GuestQuant = 1,
            RoomId = 1
        };
        var response = await _clientTest.PostAsync(url, create(newBooking));

        BookingResponse jsonResponse = await responseDtoToType<BookingResponse>(response);
        
        Assert.Equal(System.Net.HttpStatusCode.Created, response?.StatusCode);
        Assert.Equal(3, jsonResponse.BookingId);
        Assert.Equal(1, jsonResponse.GuestQuant);
        Assert.Equal(1, jsonResponse.Room.RoomId);
        Assert.Equal("Room 1", jsonResponse.Room.Name);
        Assert.Equal(1, jsonResponse.Room.Hotel.HotelId);
        Assert.Equal("Trybe Hotel Manaus", jsonResponse.Room.Hotel.Name);
        Assert.Equal("AM", jsonResponse.Room.Hotel.State);
    }

    [Trait("Category", "Testing endpoint /booking methods")]
    [Theory(DisplayName = "POST, Response STATUS = 401, Response Unauthorized")]
    [InlineData("/booking")]
    public async Task TestPostBooking(string url)
    {
        var newBooking = new {
            CheckIn = "2030-08-27",
            CheckOut = "2030-08-28",
            GuestQuant = 1,
            RoomId = 1
        };
        var response = await _clientTest.PostAsync(url, create(newBooking));

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response?.StatusCode);
    }

    [Trait("Category", "Testing endpoint /booking methods")]
    [Theory(DisplayName = "Client GET BY BookingId, Response STATUS = 200, Response JSON = one Booking by BookingId")]
    [InlineData("/booking/1")]
    public async Task TestClientGetBookingById(string url)
    {
        var clientToken = await GetClientToken();
        _clientTest.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);

        var response = await _clientTest.GetAsync(url);

        BookingResponse jsonResponse = await responseDtoToType<BookingResponse>(response);

        Assert.Equal(System.Net.HttpStatusCode.OK, response?.StatusCode);
        Assert.Equal(1, jsonResponse.BookingId);
        Assert.Equal(1, jsonResponse.GuestQuant);
        Assert.Equal(1, jsonResponse.Room.RoomId);
        Assert.Equal("Room 1", jsonResponse.Room.Name);
        Assert.Equal(1, jsonResponse.Room.Hotel.HotelId);
        Assert.Equal("Trybe Hotel Manaus", jsonResponse.Room.Hotel.Name);
        Assert.Equal("AM", jsonResponse.Room.Hotel.State);
    }

    [Trait("Category", "Testing endpoint /booking methods")]
    [Theory(DisplayName = "GET BY BookingId, Response STATUS = 401, Response Unauthorized")]
    [InlineData("/booking/1")]
    public async Task TestGetBookingById(string url)
    {
        var response = await _clientTest.GetAsync(url);
        
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response?.StatusCode);
    }

    
}