using ElprisApp.Models;
using Newtonsoft.Json;

namespace ElprisApp.Services
{
    public interface IElPrisService
    {
        Task<IEnumerable<ElPris>> GetAll();
    }

    public class ElPrisService : IElPrisService
    {
        private IHttpService _httpService;

        public ElPrisService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<ElPris>> GetAll()
        {
            // Get elpriser from API
            dynamic json = await _httpService.Get<dynamic>("/api/elpriser");

            // Convert to dynamic object
            dynamic data = JsonConvert.DeserializeObject(json.ToString());

            // Create List with elpriser
            List<ElPris> priser = new List<ElPris>();

            // Pick out values with need - put them in list of elpriser
            foreach (dynamic d in data.result.records) {
                priser.Add(new ElPris{ 
                    Id = d._id, 
                    SpotPriceDKK = $"{Math.Round(Decimal.Parse(d.SpotPriceEUR.ToString()) / 1000, 2)} kr.",
                    HourDK = DateTime.Parse(d.HourDK.ToString()),
                    PriceArea = d.PriceArea
                });
            }

            // Return list as array
            return priser.ToArray<ElPris>();
        }
    }
}