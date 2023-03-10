//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GApi.Models
{
    using GApi.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Cache;
    using System.Runtime.Remoting.Messaging;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Web;

    public partial class Contest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contest()
        {
            this.Problems = new HashSet<Problem>();
            this.Users = new HashSet<User>();
        }

        public int C_ID { get; set; }
        public string Name { get; set; }
        public System.DateTime Start_at { get; set; }
        public System.DateTime End_in { get; set; }
        public int Admin_ID { get; set; }

        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Problem> Problems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }

        public static explicit operator Contest(ContestBinding item)
        {
            Contest contest = new Contest();
            contest.Name = item.Name;
            contest.Start_at = item.Start_at;
            contest.End_in = item.End_in;
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            contest.Admin_ID = Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == "ID").Value);
            return contest;
        }
    }
}
