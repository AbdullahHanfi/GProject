using GApi.Models;
using GApi.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;

namespace GApi.Controllers
{
    [AllowAnonymous]
    public class AccountController : ApiController
    {
        /// <summary>
        /// For register new user by defult he is student for now
        /// </summary>
        /// <returns>if data correct 201 ; If data is wrong will return 406 and Json file with the wrong fileds ; If unknow problem 500.</returns>
        [HttpPost]
        [Route("api/register")]
        public IHttpActionResult register([FromBody] RegisterBinding item)
        {
            int ValidRegistertion= AccountValidate.IsValidRegistertion(item,ModelState);
            
            if (ValidRegistertion == 0)
            {
                return Content(HttpStatusCode.NotAcceptable, ModelState);
            }
            else if (ValidRegistertion == -1)
            {
                return InternalServerError();
            }
            else
            {
                return StatusCode(HttpStatusCode.Created);
            }
        }
        [HttpGet]
        [Route("api/dir")]
        public IHttpActionResult dir(string dir)
        {

            var fullPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/yourXmlFile.xml");
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(dir);
            }

            return Ok();
        }



        /// <summary>
        /// For login user in generale
        /// He can login by Name or Email like Codeforces
        /// </summary>
        /// <returns>If data correct 204 ; If data is wrong will return 401 and Json file with the wrong fileds. </returns>
        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult login([FromBody] LoginBinding item)
        {
            if(item == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            bool Valid= AccountValidate.IsVaildAccount(item.Email, item.Password);
            
            if (Valid)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                ModelState.AddModelError("Email", "Email or Password is wrong");
                var Errors = ModelState.Values.SelectMany(v => v.Errors);

                return Content(HttpStatusCode.Unauthorized, Errors);
            }
        }
        /// <summary>
        /// Just signout 😀
        /// </summary>
        /// <returns>all time 204</returns>
        [Route("api/signout")]
        public IHttpActionResult SignOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                Request.GetOwinContext().Authentication.SignOut();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
