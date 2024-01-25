using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            return _context.Cities.Select(city => (
                new CityDto {
                    CityId = city.CityId,
                    Name = city.Name,
                    State = city.State,
                }
            ));
        }

        // 2. Refatore o endpoint POST /city

        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            Save();
            var cityDto = new CityDto {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State,
            };
            return cityDto;
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
           _context.Cities.Update(city);
           Save();
           return new CityDto {
              CityId = city.CityId,
              Name = city.Name,
              State = city.State,
           };
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}