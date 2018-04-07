using Microsoft.AspNetCore.Identity;
using zulu.Models;

namespace zulu.Data
{
  public class AppUser : IdentityUser
  {
    public Member Member { get; set; }
  }
}
