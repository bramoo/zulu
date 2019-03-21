using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using zulu.Data;
using Microsoft.Extensions.FileProviders;

namespace zulu.Controllers
{
  [Route("api/v1/images")]
  //[Authorize]
  public class ImageController : Controller
  {
    public ImageController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment, IFileProvider fileProvider)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(hostingEnvironment));
      HostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
      FileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }


    public AppDbContext DbContext { get; }
    private IHostingEnvironment HostingEnvironment { get; }
    private IFileProvider FileProvider { get; }


    [HttpGet("{id:int}", Name = "GetImage")]
    public async Task<ActionResult> Get(int id)
    {
      var image = await DbContext.Images.Include(i => i.ContentType).FirstOrDefaultAsync(i => i.Id == id);

      if(image.FileName == null)
      {
        return NotFound();
      }

      var path = FileProvider.GetFileInfo("/").PhysicalPath;
      var filePath = Path.Combine(path, image.FileName);

      var fs = new FileStream(filePath, FileMode.Open);
      return new FileStreamResult(fs, image.ContentType.Name);
    }


    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult> Put(int id)
    {
      var image = await DbContext.Images.FirstOrDefaultAsync(i => i.Id == id);
      if (image == null) return NotFound();

      image.ContentType = await DbContext.ContentTypes.SingleAsync(ct => ct.Name == Request.ContentType);

      var path = FileProvider.GetFileInfo("/").PhysicalPath;

      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }

      if (string.IsNullOrWhiteSpace(image.FileName))
      {
        image.FileName = Guid.NewGuid().ToString();
      }

      var filePath = Path.Combine(path, image.FileName);

      using (FileStream fs = new FileStream(filePath, FileMode.Create))
      {
        await Request.Body.CopyToAsync(fs);
      }

      DbContext.SaveChanges();

      return Ok(image);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
      var image = await DbContext.Images.FirstOrDefaultAsync(i => i.Id == id);
      if (image == null) return NotFound();

      image.Delete();

      DbContext.SaveChanges();

      return NoContent();
    }
  }
}