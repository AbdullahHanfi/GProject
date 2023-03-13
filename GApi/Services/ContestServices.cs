using GApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace GApi.Services
{
    static public class ContestServices
    {
        /// <summary>
        /// For check Contest Name
        /// </summary>
        /// <returns>if Name is Valid return One else Zero</returns>
        static public bool ValidName(string Name, ModelStateDictionary ModelState = default)
        {
            Gdb db = new Gdb();
            bool IsValid = db.Contests.Where(ww => ww.Name == Name).Any();
            if (IsValid && ModelState != default)
            {
                ModelState.AddModelError("Name", "Not Valid Name");
            }
            return !IsValid;
        }
        /// <summary>
        /// For check Contest Dates
        /// </summary>
        /// <returns>if Date is Valid return One else Zero</returns>
        static public bool ValidDates(DateTime Start_at, DateTime End_in, ModelStateDictionary ModelState = default)
        {
            Gdb db = new Gdb();
            bool IsValid = DateTime.Compare(Start_at, End_in) == 0 &&
                         DateTime.Compare(DateTime.Now, Start_at) == 0;
            if (IsValid && ModelState != default)
            {
                ModelState.AddModelError("", "Not Valid Dates");
            }

            return !IsValid;
        }

        /// <summary>
        /// Create Contest
        /// </summary>
        /// <returns>return One if Contest Data is Valid or Zero if Contest Data isn't Valid or -1 if unknow Error</returns>
        static public int IsCreated(ContestBinding contest, ModelStateDictionary ModelState)
        {
            ContestServices.ValidName(contest.Name, ModelState);
            ContestServices.ValidDates(contest.Start_at, contest.End_in, ModelState);

            if (!ModelState.IsValid)
            {
                return 0;
            }
            else
            {
                Gdb db = new Gdb();

                db.Contests.Add((Contest)contest);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return -1;
                }
                return 1;
            }
        }
        /// <summary>
        /// Update Contest
        /// </summary>
        /// <returns>return One if Contest Data is Update or Zero if Contest Data isn't Uodate or -1 if unknow Error</returns>
        static public int UpdateContest(ContestBindingModel Item)
        {
            using (Gdb db = new Gdb())
            {
                var result = db.Contests.SingleOrDefault(b => b.C_ID == Item.ID);
                if (result != null)
                {
                    result.Start_at = Item.Start_at;
                    result.End_in = Item.End_in;
                    result.Name = Item.Name;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
        }
    }
}