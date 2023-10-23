using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

using static TrainScreenConsole.Program;

namespace TrainScreenConsole
{
    internal class Program
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Destination
        {
            public string tiploc { get; set; }
            public string description { get; set; }
            public string workingTime { get; set; }
            public string publicTime { get; set; }
        }

        public class Location
        {
            public string name { get; set; }
            public string crs { get; set; }
            public string tiploc { get; set; }
            public string country { get; set; }
            public string system { get; set; }
        }

        public class LocationDetail
        {
            public bool realtimeActivated { get; set; }
            public string tiploc { get; set; }
            public string crs { get; set; }
            public string description { get; set; }
            public string gbttBookedArrival { get; set; }
            public string gbttBookedDeparture { get; set; }
            public List<Origin> origin { get; set; }
            public List<Destination> destination { get; set; }
            public bool isCall { get; set; }
            public bool isPublicCall { get; set; }
            public string realtimeArrival { get; set; }
            public bool realtimeArrivalActual { get; set; }
            public string realtimeDeparture { get; set; }
            public bool realtimeDepartureActual { get; set; }
            public string platform { get; set; }
            public bool platformConfirmed { get; set; }
            public bool platformChanged { get; set; }
            public string displayAs { get; set; }
            public string cancelReasonCode { get; set; }
            public string cancelReasonShortText { get; set; }
            public string cancelReasonLongText { get; set; }
        }

        public class Origin
        {
            public string tiploc { get; set; }
            public string description { get; set; }
            public string workingTime { get; set; }
            public string publicTime { get; set; }
        }

        public class Root
        {
            public Location location { get; set; }
            public object filter { get; set; }
            public List<Service> services { get; set; }
        }

        public class Service
        {
            public LocationDetail locationDetail { get; set; }
            public string serviceUid { get; set; }
            public string runDate { get; set; }
            public string trainIdentity { get; set; }
            public string runningIdentity { get; set; }
            public string atocCode { get; set; }
            public string atocName { get; set; }
            public string serviceType { get; set; }
            public bool isPassenger { get; set; }
            public List<Origin> origin { get; set; }
            public List<Destination> destination { get; set; }
        }



        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://api.rtt.io/api/v1/json"),
        };

        private static string userName = @"your_api_username";
        private static string userPassword = @"your_api_password";

        static void Main(string[] args)
        {
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                var authenticationString = $"{userName}:{userPassword}";
                var base64String = Convert.ToBase64String(
                   System.Text.Encoding.ASCII.GetBytes(authenticationString));

                //var client = new HttpClient();
                //var request = new HttpRequestMessage(HttpMethod.Get, $"{sharedClient.BaseAddress}/search/PRE/arrivals");
                var request = new HttpRequestMessage(HttpMethod.Get, $"{sharedClient.BaseAddress}/search/PRE");
                request.Headers.Add("Authorization", "Basic "+ base64String);
                var response = sharedClient.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();

                //Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                Root myDeserializedClass = JsonSerializer.Deserialize<Root>(response.Content.ReadAsStringAsync().Result);

                Console.WriteLine($"{("Destination").PadRight(25)} | {("Scheduled").PadRight(10)} | {("Estimated").PadRight(10)}");
                Console.WriteLine($"-------------------------------------------------");

                foreach (Service service in myDeserializedClass.services)
                {
                    Console.WriteLine($"{service.locationDetail.destination[0].description.PadRight(25)} | {service.locationDetail.gbttBookedDeparture.Insert(2,":").PadRight(10)} | {service.locationDetail.realtimeDeparture.Insert(2, ":").PadRight(10)}");
                }

                Console.WriteLine();

                Thread.Sleep(5000);
            }
        }
    }
}