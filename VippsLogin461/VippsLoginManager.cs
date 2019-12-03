using Newtonsoft.Json;
using RestSharp;
using System;


namespace VippsLogin461
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

        public GetTokenResponse GetAndSetAccessToken(string authorization_code) {
            var response = new GetTokenResponse();

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
            var authorization_token = System.Convert.ToBase64String(plainTextBytes);


            var client = new RestClient(token_endpoint);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", api_host);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Net core client");
            request.AddHeader("Authorization", $"Basic {authorization_token}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", $"grant_type=authorization_code&code={authorization_code}&redirect_uri={redirect_url}", ParameterType.RequestBody);
            IRestResponse http_response = client.Execute(request);
            if(http_response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json_response = JsonConvert.DeserializeObject<OAuthToken>(http_response.Content);
                access_token = !string.IsNullOrEmpty(json_response.access_token) ? json_response.access_token : "";
                response.Success = true;
                response.OAuthToken = json_response;
                return response;
            }
            if (http_response.StatusCode == System.Net.HttpStatusCode.Unauthorized || http_response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                response.Success = false;
                response.Message = http_response.Content;
                response.ErrorResponse = JsonConvert.DeserializeObject<ErrorResponse>(http_response.Content);
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = http_response.Content;
                return response;
            }
         
        }

        public UserInfoResponse GetUserInfo()
        {
            var response = new UserInfoResponse();
            var client = new RestClient(userinfo_endpoint);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", api_host);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Net core client");
            request.AddHeader("Authorization", $"Bearer {access_token}");
            IRestResponse http_response = client.Execute(request);

            if (http_response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json_response = JsonConvert.DeserializeObject<UserInfo>(http_response.Content);
                response.Success = true;
                response.UserInfo = json_response;
                return response;
            }
            if (http_response.StatusCode == System.Net.HttpStatusCode.Unauthorized || http_response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                response.Success = false;
                response.Message = http_response.Content;
                response.ErrorResponse = JsonConvert.DeserializeObject<ErrorResponse>(http_response.Content);
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = http_response.Content;
                return response;
            }
        }
    }
}
