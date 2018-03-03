using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zulu.Data;
using zulu.ViewModels;

namespace zulu.Controllers
{
	[Route("api/v1/accounts")]
  public class AccountController : Controller
  {
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager, AppDbContext appDbContext)
    {
      _userManager = userManager;
      _appDbContext = appDbContext;
    }


    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var userIdentity = new AppUser
      {
        UserName = model.Email,
        Email = model.Email,
      };

      var result = await _userManager.CreateAsync(userIdentity, model.Password);

      if (!result.Succeeded)
      {
        //return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
        return BadRequest();
      }

      //await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
      await _appDbContext.SaveChangesAsync();

      return Ok("Account created");
    }
  }
}
