using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using urlShortener.Data;
using urlShortener.Helpers;
using urlShortener.Models;
using urlShortener.ViewModels;
using IronBarCode;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace urlShortener.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly UrlDbContext _db;

        public HomeController(UrlDbContext dbcontext)
        {
            _db = dbcontext;
        }




        [HttpPost]
        [Route("getShortUrl")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user, admin")]
        public async Task<ActionResult> Add(UrlVM u)
        {
            string path = encodeDecode.generateRandom();
            bool check = true;

            while (check)
            {
                var x = _db.urls.FirstOrDefault(o => o.urlIdentifier == path);
                if (x != null)
                {
                    path = encodeDecode.generateRandom();
                }
                else
                {
                    check = false;
                }

            }

            _db.urls.Add(new Models.Url
            {
                originalUrl = u.originalUrl,
                urlIdentifier = path
            });
            await _db.SaveChangesAsync();



            string shortUrl = "https://localhost:7056/RedirectTo/" + path;
            return Ok(shortUrl);
        }

        [HttpPost]
        [Route("getOriginalUrl")]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> get(string u)
        {
            var x = _db.urls.FirstOrDefault(o => o.urlIdentifier == u);
            return Ok(x.originalUrl);
        }



        [HttpGet("/RedirectTo/{path:required}")]
        [AllowAnonymous]
        public IActionResult RedirectTo(string path)
        {
            if (path == null)
            {
                return NotFound();
            }

            var x = _db.urls.FirstOrDefault(o => o.urlIdentifier == path);

            x.clickCount = x.clickCount + 1;

            _db.SaveChanges();

            return Redirect(x.originalUrl);
        }

        [HttpPost]
        [Route("getQr")]
        [Authorize(Roles = "user, admin")]
        public IActionResult getQr(string url)
        {

            GeneratedBarcode myBarcode = BarcodeWriter.CreateBarcode(url, BarcodeEncoding.QRCode);

            string x = encodeDecode.generateRandom();
            string save = x + ".png";
            myBarcode.SaveAsImage("./generatedQR/" + save);
            return Ok();
        }

        //[HttpPost]
        //[Route("getQrShortUrl")]
        //[Authorize(Roles = "user, admin")]
        //public IActionResult getQrShort(string shortUrl)
        //{

        //    GeneratedBarcode myBarcode = BarcodeWriter.CreateBarcode(shortUrl, BarcodeEncoding.QRCode);

        //    string x = encodeDecode.generateRandom();
        //    string save = x + ".png";
        //    myBarcode.SaveAsImage("./generatedQR/" + save);
        //    return Ok();
        //}


        [HttpPost]
        [Route("getQrPath")]
        [Authorize(Roles = "user, admin")]
        public IActionResult getQrPath(string path)
        {
            var url = _db.urls.FirstOrDefault(o => o.urlIdentifier == path);

            string originalUrl = url.originalUrl;
            GeneratedBarcode myBarcode = BarcodeWriter.CreateBarcode(originalUrl, BarcodeEncoding.QRCode);

            string x = encodeDecode.generateRandom();
            string save = x + ".png";
            myBarcode.SaveAsImage("./generatedQR/" + save);
            return Ok();
        }

        [HttpPost]
        [Route("getCount")]
        [Authorize(Roles = "admin")]
        public IActionResult getCount(string path)
        {
            var x = _db.urls.FirstOrDefault(o => o.urlIdentifier == path);
            int count = x.clickCount;
            return Ok(count);
        }

    }
}

