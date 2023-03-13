using GApi.Models;
using GApi.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace GApi.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ManageContestController : ApiController
    {
        /// <summary>
        /// Use For Create One Contest 
        /// </summary>
        /// <returns>if data isn't Valid Return 409 . If Data is Valid 201 . If unknow will return 500</returns>
        [HttpPost]
        [Route("api/contest")]
        public IHttpActionResult Contest([FromBody] ContestBinding contest)
        {
            int IsCreated = ContestServices.IsCreated(contest, ModelState);
            if (IsCreated == 0)
            {
                var Errors = ModelState.Values.SelectMany(v => v.Errors);

                return Content(
                    System.Net.HttpStatusCode.Conflict,
                    new { Errors, contest });
            }
            else if (IsCreated == -1)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, contest);
            }
            else
            {
                Gdb db = new Gdb();
                int ID = db.Contests.Max(WWW => WWW.C_ID);

                var fullPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Contests/" + ID);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                return StatusCode(System.Net.HttpStatusCode.Created);
            }
        }
        /// <summary>
        /// Use For Return Data Of All Contest
        /// </summary>
        /// <returns>It Will be 200 in all time</returns>
        [HttpGet]
        [Route("api/contests")]
        [AllowAnonymous]
        public IHttpActionResult Contests()
        {
            Gdb db = new Gdb();
            var contests = db.Contests
                .Select(x => new
                {
                    ID = x.C_ID,
                    Name = x.Name,
                    End_in = x.End_in,
                    Start_at = x.Start_at,
                    Number_of_Problem = db.Problems.Where(Problem => Problem.C_ID == x.C_ID)
                }).ToList();

            return Content(System.Net.HttpStatusCode.OK, contests);
        }
        /// <summary>
        /// for get Data for specfy contest
        /// </summary>
        /// <returns>return 200 if data is found . if Data isn't found return 404</returns>
        [HttpGet]
        [Route("api/contest/{id:int}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IHttpActionResult Contest([FromUri] int? id)
        {
            Gdb db = new Gdb();

            var Item = db.Contests
                .FirstOrDefault(contest => contest.C_ID == id);

            if (Item != null)
            {;
                return Content(System.Net.HttpStatusCode.OK, new{
                    ID = Item.C_ID,
                    Name = Item.Name,
                    End_in = Item.End_in,
                    Start_at = Item.Start_at
                });
            }
            else
                return StatusCode(System.Net.HttpStatusCode.NotFound);
        }
        /// <summary>
        /// Use for edit contest
        /// </summary>
        /// <returns>if Data is Valid return 204 . if Data isn't Valid return 409 and Json file has Errors and Data . if unknow error return 500</returns>
        [HttpPut]
        [Route("api/contest/{id:int}")]
        public IHttpActionResult Contest([FromUri] int? id, [FromBody] ContestBindingModel Item)
        {
            Gdb db = new Gdb();
            ContestServices.ValidName(Item.Name, ModelState);
            ContestServices.ValidDates(Item.Start_at, Item.End_in, ModelState);
            if (id == null || Item.ID == null || id != Item.ID)
            {
                ModelState.AddModelError("", "PLZ LEAVE UR TESTING SKILLS OUT OF MY APP ☺️");
            }
            if (!ModelState.IsValid)
            {
                var Errors = ModelState.Values.SelectMany(v => v.Errors);

                return Content(System.Net.HttpStatusCode.Conflict, new { Errors, Item });
            }
            int IsUpdated = ContestServices.UpdateContest(Item);

            if (IsUpdated == -1)
            {
                return StatusCode(System.Net.HttpStatusCode.InternalServerError);
            }
            else
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
        }
        /// <summary>
        /// Delete Contest
        /// </summary>
        /// <returns>It will return All time 204</returns>
        [HttpPost]
        [Route("api/contest/delete")]
        public IHttpActionResult Contests([FromBody] int? id)
        {
            Gdb db = new Gdb();
            if (id != null)
            {
                var contest = db.Contests.FirstOrDefault(cntest => cntest.C_ID == id);

                if (contest != null)
                {
                    var fullPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Contests/" + contest.C_ID);
                    db.Contests.Remove(contest);
                    try
                    {
                        db.SaveChanges();
                        Directory.Delete(fullPath);
                    }
                    catch{ }
                }
            }
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}
