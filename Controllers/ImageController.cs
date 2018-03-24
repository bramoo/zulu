using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zulu.Data;

namespace zulu.Controllers
{
  [Route("api/v1/events")]
  [Authorize]
  public class ImageController : Controller
  {
    private const string ImageDir = "images";

    public ImageController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(hostingEnvironment));
      HostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
    }


    public AppDbContext DbContext { get; }
    private IHostingEnvironment HostingEnvironment { get; }


    [HttpGet("{id:int}", Name = "GetImage")]
    public async Task<FileResult> Get(int id)
    {
      var image = await DbContext.Images.FirstOrDefaultAsync(i => i.Id == id);

      var path = Path.Combine(HostingEnvironment.WebRootPath, ImageDir);
      var filePath = Path.Combine(path, image.FileName);
      var fs = new FileStream(filePath, FileMode.Open);

      return new FileStreamResult(fs, image.ContentType.Name) { FileDownloadName = image.FileName };
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id)
    {
      var image = await DbContext.Images.FirstOrDefaultAsync(i => i.Id == id);
      if(image == null) return NotFound();
      
      image.ContentType = await DbContext.ContentTypes.SingleAsync(ct => ct.Name == Request.ContentType);

      var path = Path.Combine(HostingEnvironment.WebRootPath, ImageDir);
      var filePath = Path.Combine(path, image.FileName);

      await Request.Body.CopyToAsync(new FileStream(filePath, FileMode.Create));
      var fs = new FileStream(filePath, FileMode.Open);

      DbContext.SaveChanges();

      return Ok();
    }
  }
}