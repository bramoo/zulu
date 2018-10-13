using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zulu.Data;
using zulu.Models;
using zulu.ViewModels;
using zulu.ViewModels.Mapper;


namespace zulu.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/members")]
  [Authorize]
  public class MemberController : Controller
  {
    public MemberController(AppDbContext dbContext, MemberMapper mapper)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    private AppDbContext DbContext { get; }
    private MemberMapper Mapper { get; }


    [HttpGet("draft")]
    public async Task<IActionResult> ListDraft()
    {
      var events = await DbContext.Members.Where(e => e.State == Models.EntityState.Draft).Select(e => Mapper.Map(e)).ToListAsync();
      return Ok(events);
    }


    [HttpGet("published")]
    public async Task<IActionResult> ListPublished()
    {
      var members = await DbContext.Members.Where(e => e.State == Models.EntityState.Published).Select(e => Mapper.Map(e)).ToListAsync();
      return Ok(members);
    }


    [HttpGet("deleted")]
    public async Task<IActionResult> ListDeleted()
    {
      var members = await DbContext.Members.Where(e => e.State == Models.EntityState.Deleted).Select(e => Mapper.Map(e)).ToListAsync();
      return Ok(members);
    }


    
    [HttpGet()]
    public async Task<ActionResult> List()
    {
      var members = await DbContext.Members
        .Where(m => m.State != Models.EntityState.Deleted)
        .Include(m => m.Position)
        .Include(m => m.Rank)
        .Select(m => Mapper.Map(m))
        .ToListAsync();

      return Ok(members);
    }


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

      return Ok(Mapper.Map(member));
    }


    [HttpPost("")]
    public async Task<ActionResult> Post([FromBody]MemberViewModel model)
    {
      var member = Mapper.Update(new Member(), model);

      await DbContext.Members.AddAsync(member);
      await DbContext.SaveChangesAsync();
      return CreatedAtRoute("GetMember", new { member.Id }, Mapper.Map(member));
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]MemberViewModel model)
    {
      var member = await DbContext.Members.SingleOrDefaultAsync(m => m.Id == id);
      if (member == null)
      {
        return NotFound();
      }

      Mapper.Update(member, model);

      await DbContext.SaveChangesAsync();

      return Ok(Mapper.Map(member));
    }

  }
}