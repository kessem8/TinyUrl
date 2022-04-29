using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using TinyUrl.Models;
using TinyUrl.Services;

namespace TinyUrl.Controllers
{
    [ApiController]
    public class ShortUrlController : ControllerBase
    {
        private readonly IUrlServices _urlServices;
        private readonly ILogger<ShortUrlController> _logger;

        public ShortUrlController(IUrlServices urlServices, ILogger<ShortUrlController> logger)
        {
            _urlServices = urlServices;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult CreateShortUrl([FromQuery] string url)
        {
            try
            {
                var shortUrl = _urlServices.CreateShortUrl(url);
                return Ok(Constants.HOME_URL + shortUrl.Key);
            }
            catch (Exception ex)
            {
                _logger.LogError("error creating url", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, "oops, something went wrong... try again later");
            }
        }

        [HttpGet("Redirect")]
        public IActionResult RedirectUrl(string shortUrl) 
        {
            try
            {
                string key = shortUrl.Replace(Constants.HOME_URL, "");
                var full = _urlServices.GetFullUrl(key);
                if (full != null)
                {
                    return Redirect(full);
                }
                else
                {
                    return NotFound("The Requested URL Was Not Found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error creating url", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, "oops, something went wrong... try again later");
            }
        }
    }
}
