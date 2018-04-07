using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels.Member;

namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/members")]
  [Authorize]
  public class MemberController : Controller
  {
    public MemberController(AppDbContext dbContext, IMapper mapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    private AppDbContext DbContext { get; }
    private IMapper Mapper { get; }


    [HttpGet("{id:int}", Name = "GetMember")]
    public async Task<ActionResult> Get(int id)
    {
      var member = await DbContext.Members
        .Include(m => m.Position)
        .Include(m => m.Rank)
        .SingleOrDefaultAsync(m => m.Id == id);
      if (member == null)
      {
        return NotFound();
      }

      return Ok(Mapper.Map<MemberViewModel>(member));
    }


    [HttpPost("")]
    public async Task<ActionResult> Post([FromBody]CreateMemberViewModel model)
    {
      var member = Mapper.Map<Member>(model);

      await DbContext.Members.AddAsync(member);
      await DbContext.SaveChangesAsync();
      return CreatedAtRoute("GetMember", new { member.Id }, Mapper.Map<MemberViewModel>(member));
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]EditMemberViewModel model)
    {
      var member = await DbContext.Members.SingleOrDefaultAsync(m => m.Id == id);
      if (member == null)
      {
        return NotFound();
      }

      Mapper.Map(model, member);

      await DbContext.SaveChangesAsync();

      return Ok(Mapper.Map<MemberViewModel>(member));
    }

  }
}