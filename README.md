# VippsLogin
.net and .net core client for Vipps Login

# how to use
```
            //the parameters are
            //endpoint url
            //client_id (get from Vipps portal)
            //client_secret (get from Vipps portal)
            //redirect_url (this will be set by integration@vipps.no)
            VippsLoginManager vipps = new VippsLoginManager(VippsEnvironment.Prod, "client_id", "client_secret", "redirect_url");
            //scope can be more 'openid name phoneNumber address birthDate email nnin'
            vipps.SetScope("name phoneNumber");

            //url_to_init_login will give back a url that user needs to visit in browser. Once this process is done, user will be redirected to the redirect_url with query parameter 
            //code which is the input to GetToken()
            var url_to_init_login = vipps.GetRedirectUrl();

            //parameter is authorization_code which you get from the url parameter from previous step
            var token = await vipps.GetToken("authorization_code");
             var userinfo = await vipps.GetUserInfo();
```
