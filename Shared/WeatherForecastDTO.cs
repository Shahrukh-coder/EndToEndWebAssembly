using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndToEndWebAssembly.Shared
{
    public class WeatherForecastDTO
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? TemperatureC { get; set; }
        public int? TemperatureF { get; set; }
        public string Summary { get; set; }
        public string UserName { get; set; }
    }
}
