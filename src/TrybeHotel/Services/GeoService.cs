using System.Net.Http;
using System.Text.Json.Serialization;
using FluentAssertions;
using TrybeHotel.Dto;
using TrybeHotel.Repository;

namespace TrybeHotel.Services
{
    public class GeoService : IGeoService
    {
         private readonly HttpClient _client;
         private const string _baseUrl = "https://nominatim.openstreetmap.org/";
        public GeoService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "aspnet-user-agent");
        }

        // 11. Desenvolva o endpoint GET /geo/status
        public async Task<object> GetGeoStatus()
        {
            var response = await _client.GetAsync("status.php?format=json");

            if (!response.IsSuccessStatusCode) return default(Object);

            var result = await response.Content.ReadFromJsonAsync<object>();
            return result;
        }
        
        // 12. Desenvolva o endpoint GET /geo/address
        public async Task<GeoDtoResponse> GetGeoLocation(GeoDto geoDto)
        {
            var geoAddress = $"search?"
                + $"street={geoDto.Address}&"
                + $"city={geoDto.City}&"
                +  "country=Brazil&"
                + $"state={geoDto.State}&"
                + "format=json&limit=1";
            Console.WriteLine(geoAddress);
            var response = await _client.GetAsync(geoAddress);

            if (!response.IsSuccessStatusCode) return default(GeoDtoResponse);

            var result = await response.Content.ReadFromJsonAsync<List<GeoDtoResponse>>();
            var resultData = result[0];
            return new GeoDtoResponse {
                lat = resultData.lat,
                lon = resultData.lon
            };
        }

        // 12. Desenvolva o endpoint GET /geo/address
        public async Task<List<GeoDtoHotelResponse>> GetHotelsByGeo(GeoDto geoDto, IHotelRepository repository)
        {
            var userCoordinates = await GetGeoLocation(geoDto);

            var hotels = repository.GetHotels();
            var hotelsDistance = new List<GeoDtoHotelResponse>(); 
            foreach (var hotel in hotels)
            {
                var hotelAddress = new GeoDto {
                    Address = hotel.Address,
                    City = hotel.CityName,
                    State = hotel.State,
                };
                var hotelCoordinates = await GetGeoLocation(hotelAddress);
                var hotelDistance = CalculateDistance(
                    userCoordinates.lat, userCoordinates.lon,
                    hotelCoordinates.lat, hotelCoordinates.lon
                );

                hotelsDistance.Add(new GeoDtoHotelResponse{
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityName = hotel.CityName,
                    State = hotel.State,
                    Distance = hotelDistance,
                });
            }

            return hotelsDistance.OrderBy(hotel => hotel.Distance).ToList();
        }

       

        public int CalculateDistance (string latitudeOrigin, string longitudeOrigin, string latitudeDestiny, string longitudeDestiny) {
            double latOrigin = double.Parse(latitudeOrigin.Replace('.',','));
            double lonOrigin = double.Parse(longitudeOrigin.Replace('.',','));
            double latDestiny = double.Parse(latitudeDestiny.Replace('.',','));
            double lonDestiny = double.Parse(longitudeDestiny.Replace('.',','));
            double R = 6371;
            double dLat = radiano(latDestiny - latOrigin);
            double dLon = radiano(lonDestiny - lonOrigin);
            double a = Math.Sin(dLat/2) * Math.Sin(dLat/2) + Math.Cos(radiano(latOrigin)) * Math.Cos(radiano(latDestiny)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
            double distance = R * c;
            return int.Parse(Math.Round(distance,0).ToString());
        }

        public double radiano(double degree) {
            return degree * Math.PI / 180;
        }

    }
}