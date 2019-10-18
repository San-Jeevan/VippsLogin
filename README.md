# VippsLogin
.net and .net core client for Vipps Login

# how to use (for regular .net remove the await)
```csharp

VippsLoginManager vipps = new VippsLoginManager(VippsEnvironment.Prod, "client_id", "client_secret", "redirect_url");

vipps.SetScope("name phoneNumber"); 

var url_to_init_login = vipps.GetRedirectUrl(); //this process will output the auth_code used below

var token = await vipps.GetToken("authorization_code_from_last_step");
var userinfo = await vipps.GetUserInfo();
```


# nuget package
https://www.nuget.org/packages/Gressquel.Vippslogin/

can be found by searching in visual studio for "vipps" also
