using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using zulu.Data;
using zulu.Services;
using zulu.ViewModels;
using zulu.ViewModels.FacebookApi;
using zulu.Attributes;
using Microsoft.Extensions.Logging;

namespace zulu.Controllers
{
  public class AuthController : Controller
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly JwtIssuerOptions _jwtOptions;
    private readonly FacebookOptions _facebookOptions;
    private readonly ILogger _logger;


    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtService jwtService, IOptions<JwtIssuerOptions> jwtOptions, IOptions<FacebookOptions> facebookOptions, ILogger<AuthController> logger)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _jwtService = jwtService;
      _jwtOptions = jwtOptions.Value;
      _facebookOptions = facebookOptions.Value;
      _logger = logger;
    }


    [HttpPost("/api/v1/auth")]
    public async Task<IActionResult> Login([FromBody]CredentialsViewModel model)
    {
      var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

      if (!result.Succeeded)
      {
        return BadRequest(); //TODO modelstate from result.
      }

      var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
      var claims = await _userManager.GetClaimsAsync(appUser);
      var identity = new ClaimsIdentity(claims);

      return Ok(new JwtToken
      {
        Id = appUser.Id,
        Token = await _jwtService.GenerateEncodedToken(appUser.UserName, identity),
        ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
      });
    }

    [HttpPost("/api/v1/fbauth")]
    public async Task<IActionResult> FacebookAuth([FromBody]FacebookAuthToken model)
    {
      _logger.LogDebug($"Attempting Facebook auth - token: {model.Token}");
      var client = new HttpClient();

      var appAccessTokenResponse = await client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_facebookOptions.AppId}&client_secret={_facebookOptions.AppSecret}&grant_type=client_credentials");
      var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
      _logger.LogDebug($"Facebook access token recieved.");

      var userAccessTokenValidationResponse = await client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.Token}&access_token={appAccessToken.AccessToken}");
      var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

      if (!userAccessTokenValidation.Data.IsValid)
      {
        _logger.LogDebug($"Facebook access token validation failed.");
        ModelState.AddModelError("login_failure", "Invalid facebook token.");
        return BadRequest(ModelState);
      }
      _logger.LogDebug($"Facebook access token validated.");

      var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v3.2/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.Token}");
      var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);
      _logger.LogDebug($"Facebook user info recieved for user {userInfo.Email}");

      var result = await _signInManager.ExternalLoginSignInAsync("facebook", userAccessTokenValidation.Data.UserId.ToString(), isPersistent: false, bypassTwoFactor: true);

      AppUser appUser;
      if (result.Succeeded)
      {
        _logger.LogDebug($"Facebook sign in successful. Retriving application user.");
        appUser = _userManager.Users.SingleOrDefault(r => r.Email == userInfo.Email);
        _logger.LogDebug($"AppUser: {appUser.Email}");
      }
      else
      {
        if (result.IsLockedOut || result.IsNotAllowed)
        {
          _logger.LogDebug($"User forbidden.");
          return Forbid();
        }

        //No account
        _logger.LogDebug($"No account. Registering user.");
        appUser = new AppUser { UserName = userInfo.Email, Email = userInfo.Email };
        var idResult = await _userManager.CreateAsync(appUser);
        if (idResult.Succeeded)
        {
          _logger.LogDebug($"Storing Facebook credentials.");
          idResult = await _userManager.AddLoginAsync(appUser, new UserLoginInfo("facebook", userAccessTokenValidation.Data.UserId.ToString(), userInfo.Name));
          if (idResult.Succeeded)
          {
            _logger.LogDebug($"Credentials stored. Signing user in.");
            await _signInManager.SignInAsync(appUser, isPersistent: false);
          }
        }
      }

      _logger.LogDebug($"Retrieving user claims");
      var claims = await _userManager.GetClaimsAsync(appUser);
      var identity = new ClaimsIdentity(claims);

      _logger.LogDebug($"Login successful.");
      return Ok(new JwtToken
      {
        Id = appUser.Id,
        Token = await _jwtService.GenerateEncodedToken(appUser.UserName, identity),
        ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
      });
    }


    [Authorize]
    [HttpGet("superSecretSignal")]
    public IActionResult SuperSecretSignal()
    {
      return Redirect("https://media.giphy.com/media/zLBQYkwGGQdJC/giphy.gif");
    }


    [Authorize]
    [HttpGet("secretSquirrel")]
    public IActionResult SecretSquirrel()
    {
      return Redirect("https://pensivesquirrel.files.wordpress.com/2013/09/secret-sqjuirrel-2.gif");
    }
  }
}
