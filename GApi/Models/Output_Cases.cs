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
    using System;
    using System.Collections.Generic;
    
    public partial class Output_Cases
    {
        public int ID { get; set; }
        public int Input_ID { get; set; }
        public string Path { get; set; }
    
        public virtual Input_Cases Input_Cases { get; set; }
    }
}
