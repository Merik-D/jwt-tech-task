using System.Globalization;
using System.Text.Json.Serialization;

namespace JwtTechTask;

public class ExchangeRate
{
    [JsonPropertyName("ccy")]

    public Currency Ccy { get; set; }
    [JsonPropertyName("base_ccy")]
    public Currency BaseCcy { get; set; }
    [JsonPropertyName("buy")]
    public string Buy { get; set; }
    [JsonPropertyName("sale")]
    public string Sale { get; set; }
    
    public decimal BuyRate => decimal.Parse(Buy, CultureInfo.InvariantCulture);
    public decimal SaleRate => decimal.Parse(Sale, CultureInfo.InvariantCulture);
}