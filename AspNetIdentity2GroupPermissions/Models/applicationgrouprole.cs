//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IdentitySample.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class applicationgrouprole
    {
        public string ApplicationRoleId { get; set; }
        public string ApplicationGroupId { get; set; }
    
        public virtual applicationgroup applicationgroup { get; set; }
    }
}