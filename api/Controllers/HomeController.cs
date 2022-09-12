using api.Models;
using api.Models.DataContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
using System.Net.Http.Headers;
using System.Web;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDBContext db;
        private IWebHostEnvironment env;
        private IHttpContextAccessor http;
        public HomeController(AppDBContext _db,IWebHostEnvironment _env, IHttpContextAccessor _http)
        {
            db = _db;
            env = _env;
            http = _http;
        }
        [HttpGet]
        public async Task<IActionResult> Getdata()
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.StandardInput.WriteLine("ipconfig");
            p.StandardInput.WriteLine("D:");
            p.StandardInput.WriteLine("cd angular");
            p.StandardInput.WriteLine("ng new mangal");
            p.StandardInput.Flush();
            p.StandardInput.Close();
            p.WaitForExit();
            Console.WriteLine(p.StandardOutput.ReadToEnd());
            Console.ReadKey();


            var user = await db.Users.ToListAsync();
            return Ok(user);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getbyid(int id)
        {
            User u = new User();
            var data = db.Users.FirstOrDefaultAsync(x => x.User_id == id);
            if (data == null)
            {
                return Ok(data);
            }
            return NotFound("data not found");


        }
        [HttpPost]
        public async Task<IActionResult> adduser(User u)
        {
            await db.Users.AddAsync(u);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Getdata), u.User_id, u);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updatedata(int id, User u)
        {
            var data = await db.Users.FirstOrDefaultAsync(x => x.User_id == id);

            if (data != null)
            {
                data.CardNumber = u.CardNumber;
                data.Card_holdername = u.Card_holdername;
                data.Expirydate = u.Expirydate;
                data.ExpiryYear = u.ExpiryYear;
                data.CVC = u.CVC;
                await db.SaveChangesAsync();
                return Ok(data);
            }
            return NotFound("not found data");

        }
        /*        [HttpPost]
                [Route("upload")]
                public dynamic upload(List<IFormFile> userfile)
                {
                    if (userfile.Count > 0)
                    {
                        foreach (var file in userfile)
                        {
                            string filename = file.FileName;
                            filename = Path.GetFileName(filename);
                            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "C://Users//Beast//source//repos//api//api//Controllers//", filename);
                            var stream = new FileStream(uploadpath, FileMode.Create);
                            file.CopyToAsync(stream);



                        }
                    }
                    return "success";


                }*/
        [HttpPost("upload")]
        public IActionResult upload()
        {
            try
            {
                var form = Request.Form;
                foreach(var file in form.Files)
                {
                    /* var path= Path.Combine(Directory.GetCurrentDirectory(), "C://Users//Beast//source//repos//api//api//Controllers//", file.FileName);*/
                    var path = Path.Combine(env.ContentRootPath, "C://Users//Beast//source//repos//api//api//Controllers//", file.FileName);
                    /*var path = Path.Combine(env.WebRootPath, "upload", file.FileName);*/
                    using ( var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                };
            }
            catch
            {
                return BadRequest();

            }
            return Ok();
            
        }

    }
}
