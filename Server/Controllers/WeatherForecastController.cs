using EndToEndDb.Data;
using EndToEndDb.Models;
using EndToEndWebAssembly.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EndToEndWebAssembly.Server.Controllers
{
    [Authorize]
    [ApiController]
    //[Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly EndToEndClientContext _context;

        public WeatherForecastController(EndToEndClientContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/WeatherForecast/GetAsync")]
        public async Task<List<WeatherForecastDTO>> GetAsync()
        {
            // get current user 
            string strCurrentUser = this.User.Claims.
                                    Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                                    .FirstOrDefault().Value;
            // get weather forecasts
            var result = await _context.WeatherForecast
                        // Only get entries for the current logged in user
                        .Where(v => v.UserName == strCurrentUser)
                        // Use AsNoTracking to disable EF change tracking
                        .AsNoTracking()
                        // Use ToListAsync to avoid blocking a thread
                        .ToListAsync();

            // collection to return 
            List<WeatherForecastDTO> colWeatherForecast = new List<WeatherForecastDTO>();
            foreach (var item in result)
            {
                WeatherForecastDTO weatherForecastDTO = new WeatherForecastDTO();
                weatherForecastDTO.UserName = item.UserName;
                weatherForecastDTO.TemperatureF = item.TemperatureF;
                weatherForecastDTO.TemperatureC = item.TemperatureC;
                weatherForecastDTO.Id = item.Id;
                weatherForecastDTO.Summary = item.Summary;
                weatherForecastDTO.Date = item.Date;
                colWeatherForecast.Add(weatherForecastDTO);
            }
            return colWeatherForecast;
        }

        [HttpPost]
        [Route("api/weatherForecast/Post")]
        public void Post([FromBody]WeatherForecastDTO paramWeatherForecast)
        {
            WeatherForecast weatherForecast = new WeatherForecast();
            string strCurrentUserr = this.User.Claims
                                    .Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                                    .FirstOrDefault().Value;

            weatherForecast.UserName = paramWeatherForecast.UserName;
            weatherForecast.TemperatureC = paramWeatherForecast.TemperatureC;
            weatherForecast.TemperatureF = paramWeatherForecast.TemperatureF;
            weatherForecast.Summary = paramWeatherForecast.Summary;
            weatherForecast.Id = paramWeatherForecast.Id;
            weatherForecast.Date = paramWeatherForecast.Date;
            _context.WeatherForecast.Add(weatherForecast);
            _context.SaveChanges();
        }

        [HttpPut]
        [Route("api/weatherForecast/Put")]
        public void Put([FromBody]WeatherForecastDTO paramWeatherForecast)
        {
            string strCurrentUserr = this.User.Claims
                                    .Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                                    .FirstOrDefault().Value;

            var existingWeatherForecast = _context.WeatherForecast
                                            .Where(c=>c.Id == paramWeatherForecast.Id && c.UserName == strCurrentUserr)
                                            .FirstOrDefault();
            if (existingWeatherForecast != null)
            {
                existingWeatherForecast.UserName = strCurrentUserr;
                existingWeatherForecast.TemperatureC = paramWeatherForecast.TemperatureC;
                existingWeatherForecast.TemperatureF = paramWeatherForecast.TemperatureF;
                existingWeatherForecast.Summary = paramWeatherForecast.Summary;
                existingWeatherForecast .Date = paramWeatherForecast.Date;

                _context.SaveChanges ();
            }
        }

        [HttpDelete]
        [Route("api/weatherForecast/Delete/{id}")]
        public void Delete(int id)
        {
            string strCurrentUser = this.User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").FirstOrDefault().Value;
            var existingWeatherForecast = _context.WeatherForecast.Where(c=>c.UserName.Equals(strCurrentUser) && c.Id == id).FirstOrDefault();
            if (existingWeatherForecast != null)
            {
                _context.Remove(existingWeatherForecast);
                _context.SaveChanges ();
            }
        }
    }
}