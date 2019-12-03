# VippsLogin
.net and .net core client for Vipps Login

# how to use (for regular .net remove the await)
```csharp

VippsLoginManager vipps = new VippsLoginManager(VippsEnvironment.Prod, "client_id", "client_secret", "redirect_url", "state");

vipps.SetScope("name phoneNumber"); 

var url_to_init_login = vipps.GetRedirectUrl(); //this process will output the auth_code used below

var token_response = await vipps.GetAndSetAccessToken("authorization_code_from_last_step");
var userinfo = new UserInfoResponse();
if (token_response.Success)  userinfo = await vipps.GetUserInfo();
else {
      Console.WriteLine($"Something went wrong: {token_response.Message}");
      }
```


# nuget package
https://www.nuget.org/packages/Gressquel.Vippslogin/

can be found by searching in visual studio for "vipps" also
