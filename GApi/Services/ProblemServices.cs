using GApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;

namespace GApi.Services
{
    static public class ProblemServices
    {
        static public bool IsCreated(List<ProblemBinding> problems, int? c_id)
        {
            Gdb db = new Gdb();

            foreach (var problem in problems)
            {

                var fakeFileName = Path.GetRandomFileName();

                Problem uploadedFile = new Problem()
                {
                    Name = $"{problem.ProbelmFile.FileName},{problem.ProbelmFile.ContentType}",
                    Time_Limit = problem.Time_Limit,
                    Memory_Limit = problem.Memory_Limit,
                    C_ID = problem.C_ID,
                    P_Description = fakeFileName
                };
                db.Problems.Add(uploadedFile);

                var dirPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Contests/" + $"{problem.ProbelmFile.Name}");

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Contests/" + $"{c_id}/{db.Problems.Max(pro=>pro.ID)}");

                FileStream fileStream = new FileStream(path, FileMode.Create);
                problem.ProbelmFile.CopyTo(fileStream);

            }
           
            return true;
        }
        static public bool IsNotSame<T>(List<T> values, List<T> Items)
            where T : IComparable<T>
        {
            foreach (var value in values)
            {
                foreach (var Item in Items)
                {
                    if (!Item.Equals(value))
                        return false;
                }
            }
            return true;
        }
        static public bool IsNotSame<T>(List<T> values, T Item)
            where T : IComparable<T>
        {
            foreach (var value in values)
            {
                if (!Item.Equals(value))
                    return false;
            }
            return true;
        }

    }
}