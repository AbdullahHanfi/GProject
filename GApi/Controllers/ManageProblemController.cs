using GApi.Models;
using GApi.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;

namespace GApi.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ManageProblemController : ApiController
    {
        /// <summary>
        /// For Create Problems in Contest
        /// </summary>
        /// <param name="c_id">Contest ID</param>
        /// <param name="problems">List of Problem Data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/contest/{c_id:int}/prolems")]
        public IHttpActionResult Prolem([FromUri] int? c_id, [FromBody] List<ProblemBinding> problems)
        {

            if (c_id is null)
                ModelState.AddModelError("", "PLZ LET UR TESTING SKILLS OUT OF MY APP");
            else
            {
                bool IsSameID = ProblemServices
                    .IsNotSame((List<int>)problems.Select(p => p.C_ID),
                    c_id ?? -1);

                if (!IsSameID)
                    ModelState.AddModelError("", "PLZ LET UR TESTING SKILLS OUT OF MY APP");
                Gdb db = new Gdb();
                bool IsValidName = ProblemServices
                    .IsNotSame((List<string>)problems.Select(p => p.ProbelmFile.FileName),
                        db.Problems.Select(X => X.Name).ToList());

                if (!IsSameID)
                    ModelState.AddModelError("", "PLZ LET UR TESTING SKILLS OUT OF MY APP");
                if(!IsValidName)
                    ModelState.AddModelError("", "Name isn't valid plz make Problems has unique Names");
            }
            if (!ModelState.IsValid)
            {
                var Errors = ModelState.Values.SelectMany(v => v.Errors);
                return Content((HttpStatusCode)422, Errors);
            }

            bool IsCreated = ProblemServices.IsCreated(problems, c_id);
            if (IsCreated)
            {
                return StatusCode(HttpStatusCode.Created);
            }
            else
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// For Get All problems in contest 
        /// </summary>
        /// <param name="c_id">Contest ID</param>
        /// <returns>if Data isn't Valid return 422 . if data Valid return 200 and Json file With Problem Data</returns>
        [HttpGet]
        [Route("api/contest/{c_id:int}/problems")]
        [AllowAnonymous]
        public IHttpActionResult Problems([FromUri] int? c_id)
        {
            if (c_id is null)
                ModelState.AddModelError("", "PLZ LEAVE UR TESTING SKILLS OUT OF MY APP ☺️");
            if (!ModelState.IsValid)
            {
                var Errors = ModelState.Values.SelectMany(v => v.Errors);
                return Content((HttpStatusCode)422, Errors);
            }
            Gdb db = new Gdb();

            var problems = db.Problems.Where(contest => contest.C_ID == c_id)
                .Select(x => new
                {
                    ID = x.ID,
                    Name = x.Name.Trim().Split(',')[0],
                    Time_Limit = x.Time_Limit,
                    Memory_Limit = x.Memory_Limit,
                    C_ID = x.C_ID,
                    Problem_Link = Problem(x.C_ID, x.ID)
                }).ToList();

            return Content(System.Net.HttpStatusCode.OK, problems);
        }
        /// <summary>
        /// Get File of problem
        /// </summary>
        /// <param name="c_id">Contest ID</param>
        /// <param name="p_id">Problem ID</param>
        /// <returns>if Data isn't Valid return 422 . if data Valid return 200 and file for user</returns>
        [HttpGet]
        [Route("api/contest/{c_id:int}/problem/{p_id:int}")]
        [AllowAnonymous]
        public IHttpActionResult Problem([FromUri] int? c_id, [FromUri] int? p_id)
        {
            Gdb db = new Gdb();

            if (c_id is null || p_id is null)
                ModelState.AddModelError("", "PLZ LEAVE UR TESTING SKILLS OUT OF MY APP ☺️");

            if (!ModelState.IsValid)
            {
                var Errors = ModelState.Values.SelectMany(v => v.Errors);
                return Content((HttpStatusCode)422, Errors);
            }

            var uploadedFile = db.Problems.FirstOrDefault(Problem => Problem.ID == p_id && Problem.C_ID == c_id);

            if (uploadedFile is null || uploadedFile.C_ID != c_id)
                return Content(HttpStatusCode.NotFound, "Not Valid Data");

            var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Contest/" + $"{c_id}/{uploadedFile.P_Description}");

            MemoryStream memoryStream = new MemoryStream();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            fileStream.CopyTo(memoryStream);

            memoryStream.Position = 0;
            string[] Name = uploadedFile.Name.Trim().Split(',');

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(memoryStream.GetBuffer())
            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = Name[0]
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(Name[1]);

            var response = ResponseMessage(result);

            return response;
        }
        /// <summary>
        /// Delete problem
        /// </summary>
        /// <param name="c_id">Contest ID</param>
        /// <param name="p_id">Problem ID</param>
        /// <returns>i will make it return 200 all time </returns>
        [HttpDelete]
        [Route("api/contest/{c_id:int}/problem/")]
        public IHttpActionResult DeleteProblem([FromUri] int? c_id, [FromBody] int? p_id)
        {
            Gdb db = new Gdb();

            if (c_id is null || p_id is null)
                ModelState.AddModelError("", "PLZ LEAVE UR TESTING SKILLS OUT OF MY APP ☺️");

            if (!ModelState.IsValid)
            {
                var Errors = ModelState.Values.SelectMany(v => v.Errors);
                return Content((HttpStatusCode)422, Errors);
            }

            var ProblemData = db.Problems.FirstOrDefault(Problem => Problem.ID == p_id && Problem.C_ID == c_id);

            if (ProblemData is null)
                return Content(HttpStatusCode.NotFound, "Not Valid Data");

            var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Contest/" +$"{c_id}/{ProblemData.P_Description}");

            File.Delete(path);

            string dir = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Contests/" + $"{ProblemData.Name}");
            db.Problems.Remove(ProblemData);
            try
            {
                db.SaveChanges();
                Directory.Delete(dir);
            }
            catch
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
