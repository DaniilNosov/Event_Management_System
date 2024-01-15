using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Event_Management_System.Controllers;
public class PrivatController : Controller
{
    private readonly HttpClient _client;

    public PrivatController()
    {
        _client = new HttpClient();
    }

    public async Task<IActionResult> GetExchangeRates()
    {
        var response = await _client.GetAsync("https://api.privatbank.ua/p24api/exchange_rates?date=01.12.2014");

        if (!response.IsSuccessStatusCode)
        {
            return View("Error", "Failed to fetch exchange rates.");
        }

        var ratesJson = await response.Content.ReadAsStringAsync();
        var rates = JsonConvert.DeserializeObject<Privat24Rates>(ratesJson);

        return View("PrivatRates", rates);
    }
}
