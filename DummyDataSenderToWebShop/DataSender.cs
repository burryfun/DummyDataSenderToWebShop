using System.Text;
using System.Text.Json;

namespace DummyDataSenderToWebShop
{
    public class DataSender
    {
        private HttpClient _httpClient;
        private string _url;
        private string _jwt;

        public static async Task<DataSender> Create(string api_url, string email, string password)
        {
            var sender = new DataSender(api_url);
            await sender.GetJWT(email, password);
            return sender;
        }

        private DataSender(string api_url)
        {
            _httpClient = new HttpClient();
            _url = api_url;
        }

        public async Task<HttpResponseMessage> SendBrand(string name)
        {

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var brand = new
            {
                name = name
            };

            var data = ObjectToJson(brand);

            var response = await SendJsonData("/catalog", data);

            return response;
        }

        public async Task<HttpResponseMessage> SendBrandImage(string imagePath)
        {
            return null;
        }

        public async Task<HttpResponseMessage> SendSmartphone()
        {
            return null;
        }

        private async Task GetJWT(string email, string password)
        {

            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("email or password is null or empty");
            }

            var account = new
            {
                email = email,
                password = password,
            };

            var data = ObjectToJson(account);
            var response = await _httpClient.PostAsync(_url + "/login", data);

            if (response.IsSuccessStatusCode)
            {
                string? jsonString = await response.Content.ReadAsStringAsync();

                if (jsonString != null)
                {
                    JWTResponse jwtResponse = JsonSerializer.Deserialize<JWTResponse>(jsonString);
                    _jwt = jwtResponse.jwtToken;
                }
                else
                {
                    throw new Exception("jwt from server is null");
                }
            }
        }

        private async Task<HttpResponseMessage> SendJsonData(string endpoint, HttpContent data)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_url + endpoint),
                Headers =
                {
                    { "Authorization", $"Bearer {_jwt}" }
                },
                Content = data
            };

            return await _httpClient.SendAsync(request);
        }


        private StringContent ObjectToJson(object _object)
        {
            if (_object == null)
            {
                throw new ArgumentNullException(nameof(_object));
            }

            StringContent json = new StringContent(JsonSerializer.Serialize(_object), Encoding.UTF8, "application/json");
            return json;
        }
    }

    public class JWTResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public string jwtToken { get; set; }
        public string refreshToken { get; set; }
    }

    public class SmartphoneResponse
    {

        public Guid id { get; set; }
        public string name { get; set; }
        public string brandName { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int discount { get; set; }
        public decimal? amount { get; set; }
    }

}
