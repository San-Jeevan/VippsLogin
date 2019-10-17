# VippsLogin
.net and .net core client for Vipps Login

# how to use
```

VippsLoginManager vipps = new VippsLoginManager(VippsEnvironment.Prod, "client_id", "client_secret", "redirect_url");

vipps.SetScope("name phoneNumber");

 //url_to_init_login will give back a url that user needs to visit in browser. Once this process is done, user will be redirected to the redirect_url with query parameter 
//code which is the input to GetToken()
var url_to_init_login = vipps.GetRedirectUrl();

var token = await vipps.GetToken("authorization_code_from_last_step");
var userinfo = await vipps.GetUserInfo();
```
