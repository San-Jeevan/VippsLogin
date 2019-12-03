
using System.Collections.Generic;


namespace VippsLogin461
{

    public class AbstractResponse {
        public bool Success { get; set; }
        public string Message { get; set; }
    }


    public class GetTokenResponse: AbstractResponse
    {
        public OAuthToken OAuthToken { get; set; }
        public ErrorResponse ErrorResponse { get; set; }
    }

    public class UserInfoResponse : AbstractResponse
    {
        public UserInfo UserInfo { get; set; }
        public ErrorResponse ErrorResponse { get; set; }
    }


    public class Address
    {
        public string address_type { get; set; }
        public string country { get; set; }
        public string formatted { get; set; }
        public string postal_code { get; set; }
        public string region { get; set; }
        public string street_address { get; set; }
    }

    public class UserInfo
    {
        public List<Address> address { get; set; }
        public string birthdate { get; set; }
        public string email { get; set; }
        public string family_name { get; set; }
        public string given_name { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
        public string sid { get; set; }
        public string sub { get; set; }
        public string nnin { get; set; }
    }

    public class OAuthToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public int id_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
    }

    public class ErrorResponse
    {
        public string error { get; set; }
        public int error_code { get; set; }
        public string error_debug { get; set; }
    }
}
