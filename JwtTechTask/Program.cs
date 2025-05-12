using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace JwtTechTask
{
    class Program
    {
        static void Main(string[] args)
        {
            string jwt = "eyJhbGciOiJub25lIn0.eyJkYXRhIjpbeyJ1c2VySWQiOiIxMjM0NSIsInRyYW5zYWN0aW9ucyI6W3siaWQiOiIxIiwiYW1vdW50Ijo1MCwiY3VycmVuY3kiOiJVQUgiLCJtZXRhIjp7InNvdXJjZSI6IkNBQiIsImNvbmZpcm1lZCI6dHJ1ZX0sInN0YXR1cyI6IkNvbXBsZXRlZCJ9LHsiaWQiOiIyIiwiYW1vdW50IjozMC41LCJjdXJyZW5jeSI6IlVBSCIsIm1ldGEiOnsic291cmNlIjoiQUNCIiwiY29uZmlybWVkIjpmYWxzZX0sInN0YXR1cyI6IkluUHJvZ3Jlc3MifSx7ImlkIjoiMyIsImFtb3VudCI6ODkuOTksImN1cnJlbmN5IjoiVUFIIiwibWV0YSI6eyJzb3VyY2UiOiJDQUIiLCJjb25maXJtZWQiOnRydWV9LCJzdGF0dXMiOiJDb21wbGV0ZWQifV19LHsidXNlcklkIjoidTEyMyIsInRyYW5zYWN0aW9ucyI6W3siaWQiOiIxIiwiYW1vdW50Ijo0NDM0LCJjdXJyZW5jeSI6IkVVUiIsIm1ldGEiOnsic291cmNlIjoiQ0FCIiwiY29uZmlybWVkIjp0cnVlfSwic3RhdHVzIjoiQ29tcGxldGVkIn0seyJpZCI6IjIiLCJhbW91bnQiOjU2LjUzLCJjdXJyZW5jeSI6IlVBSCIsIm1ldGEiOnsic291cmNlIjoiQUNCIiwiY29uZmlybWVkIjpmYWxzZX0sInN0YXR1cyI6Mn1dfV19.";

            string payload = ExtractPayload(jwt);
            // TODO: Step 1: Decode the JWT and extract the payload
            
            var data = ParsePayload(payload);
            // TODO: Step 2: Deserialize the payload into C# objects

            Console.WriteLine("User IDs:");
            PrintUserIds(data);

            Console.WriteLine($"Transaction count: {CountTransactions(data)}");

            Console.WriteLine("Amount:");
            var (uah, eur) = CalculateConfirmedAmounts(data);
            Console.WriteLine($"UAH amount: {uah}");
            Console.WriteLine($"EUR amount: {eur}");

            var exchangeRates = FetchExchangeRates();
            var eurRate = exchangeRates.FirstOrDefault(r => r.Ccy == Currency.EUR);

            var (totalInEur, totalInUah) = ConvertCurrency(uah, eur, eurRate);
            Console.WriteLine($"Total in EUR: {totalInEur}");
            Console.WriteLine($"Total in UAH: {totalInUah}");
            // TODO: Step 3: Print user ID, transaction count, total confirmed amount
        }

        static string ExtractPayload(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Payload.SerializeToJson();
        }

        static Data ParsePayload(string payloadJson)
        {
            return JsonSerializer.Deserialize<Data>(payloadJson);
        }

        static void PrintUserIds(Data data)
        {
            foreach (var user in data.Users)
            {
                Console.WriteLine(user.Id);
            }
        }

        static int CountTransactions(Data data)
        {
            return data.Users.Sum(u => u.Transactions.Count);
        }

        static (decimal uah, decimal eur) CalculateConfirmedAmounts(Data data)
        {
            decimal uah = 0, eur = 0;

            foreach (var user in data.Users)
            {
                foreach (var t in user.Transactions)
                {
                    if (t.Meta.Confirmed)
                    {
                        if (t.Currency == Currency.UAH)
                            uah += t.Amount;
                        else if (t.Currency == Currency.EUR)
                            eur += t.Amount;
                    }
                }
            }

            return (uah, eur);
        }

        static ExchangeRate[] FetchExchangeRates()
        {
            using var client = new HttpClient();
            var json = client.GetStringAsync("https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=11").Result;
            return JsonSerializer.Deserialize<ExchangeRate[]>(json);
        }

        static (decimal totalInEUR, decimal totalInUAH) ConvertCurrency(decimal uah, decimal eur, ExchangeRate eurRate)
        {
            decimal totalInEur = eur + uah / eurRate.SaleRate;
            decimal totalInUah = uah + eur * eurRate.BuyRate;
            return (totalInEur, totalInUah);
        }
    }
}
