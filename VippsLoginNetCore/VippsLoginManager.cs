using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace VippsLoginNetCore
{
    public class VippsLoginManager
    {
        private string api_endpoint = "https://api.vipps.no/access-management-1.0/access";
        private string api_host = "";
        private string client_id = "";
        private string client_secret = "";
        private string redirect_url = "";
        private string scope = "openid";
        private string token_endpoint = "";
        private string auth_endpoint = "";
        private string userinfo_endpoint = "";
        private string access_token = "";
        private string state = "";
        
        public VippsLoginManager (string api_endpoint ,string client_id, string client_secret, string redirect_url, string state) {
            this.client_id = client_id;
            this.client_secret = client_secret;
            this.redirect_url = redirect_url;
            this.api_endpoint = api_endpoint;
            this.state = state;
            token_endpoint = this.api_endpoint + "/oauth2/token";
            auth_endpoint = this.api_endpoint + "/oauth2/auth";
            userinfo_endpoint = this.api_endpoint + "/userinfo";
            api_host = new Uri(this.api_endpoint).Host;
        }

        public void SetScope(string scope) {
            this.scope = scope;
        }

        public string GetRedirectUrl()
        {
            return auth_endpoint + $"?client_id={client_id}&response_type=code&scope={scope}&state={state}&redirect_uri={redirect_url}";
        }

        public async Task<GetTokenResponse> GetToken(string authorization_code) {
            var response = new GetTokenResponse();

            var plainTextBytes = Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
            var authorization_token = Convert.ToBase64String(plainTextBytes);

            var request = new HttpRequestMessage() { Method = HttpMethod.Post,  RequestUri = new Uri(token_endpoint),
                Content = new StringContent($"grant_type=authorization_code&code={authorization_code}&redirect_uri={redirect_url}", Encoding.UTF8, "application/x-www-form-urlencoded") };
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Host", api_host);
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "Net core client");
            request.Headers.Add("Authorization", $"Basic {authorization_token}");
            var client = new HttpClient();
            var http_response = await client.SendAsync(request);



            var http_response_content = await http_response.Content.ReadAsStringAsync();
            if (http_response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json_response = JsonConvert.DeserializeObject<OAuthToken>(http_response_content);
                access_token = !string.IsNullOrEmpty(json_response.access_token) ? json_response.access_token : "";
                response.Success = true;
                response.OAuthToken = json_response;
                return response;
            }
            if (http_response.StatusCode == System.Net.HttpStatusCode.Unauthorized || http_response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                response.Success = false;
                response.Message = http_response_content;
                response.ErrorResponse = JsonConvert.DeserializeObject<ErrorResponse>(http_response_content);
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = http_response_content;
                return response;
            }
         
        }

        public async Task<UserInfoResponse> GetUserInfo()
        {
            var response = new UserInfoResponse();

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(userinfo_endpoint)
            };
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Host", api_host);
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "Net core client");
            request.Headers.Add("Authorization", $"Bearer {access_token}");
            var client = new HttpClient();
            var http_response = await client.SendAsync(request);

            var http_response_content = await http_response.Content.ReadAsStringAsync();
            if (http_response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json_response = JsonConvert.DeserializeObject<UserInfo>(http_response_content);
                response.Success = true;
                response.UserInfo = json_response;
                return response;
            }
            if (http_response.StatusCode == System.Net.HttpStatusCode.Unauthorized || http_response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                response.Success = false;
                response.Message = http_response_content;
                response.ErrorResponse = JsonConvert.DeserializeObject<ErrorResponse>(http_response_content);
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = http_response_content;
                return response;
            }
        }
    }
}
