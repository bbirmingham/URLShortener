using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using URLShortener.Models;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View(new CreateModel());
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(CreateModel model)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                bool isNew = false;
                string URLShortened = "";

                while (!isNew)
                {
                    // generates a GUID and uses the first 8 characters
                    // while GUIDs are very unique in themselves, shortening them to 8 characters increases chance of collision
                    // so repeat until new one found
                    URLShortened = Guid.NewGuid().ToString("n").Substring(0, 8);

                    // if returns nothing then its a URL not in use
                    isNew = connection.QueryFirstOrDefault<CreateModel>("GetURL", param: new { URLShortened = URLShortened }, commandType: System.Data.CommandType.StoredProcedure) is null;

                    // construct the URL then put it in the model
                    if (isNew)
                    {
                        model.URLShortened = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{URLShortened}";
                    }
                }

                // save to database
                if (!string.IsNullOrWhiteSpace(URLShortened))
                {
                    connection.Execute("AddURL", param: new { URL = model.URL, URLShortened = URLShortened }, commandType: System.Data.CommandType.StoredProcedure);
                }
            };

            return View("Finished", model);
        }

        [Route("{*all}")]
        public IActionResult Index()
        {
            string[] urlFrags = Request.Path.ToString().Split("/");

            if (urlFrags.Length > 1)
            {
                string urlFrag = urlFrags[1];

                if (!string.IsNullOrWhiteSpace(urlFrag))
                {
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        var result = connection.QueryFirstOrDefault<CreateModel>("GetURL", param: new { URLShortened = urlFrag }, commandType: System.Data.CommandType.StoredProcedure);

                        if (result is not null && !string.IsNullOrWhiteSpace(result.URL))
                        {
                            return new RedirectResult(result.URL);
                        }
                    };
                }
            }

            // default to this
            return View();
        }
    }
}
