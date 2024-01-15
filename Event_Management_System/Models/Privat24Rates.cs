using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Event_Management_System.Models;

public class Privat24Rates
{
    public string Date { get; set; }
    public string Bank { get; set; }
    public int BaseCurrency { get; set; }
    public string BaseCurrencyLit { get; set; }
    [JsonProperty("exchangeRate")]
    public List<Rate> ExchangeRates { get; set; } = new List<Rate>();
}
