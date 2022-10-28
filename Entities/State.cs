// ----------------------------------------------------------------------------------
// <copyright company="Exesoft Inc.">
//	This code was generated by Instant Web API code automation software (https://www.InstantWebAPI.com)
//	Copyright Exesoft Inc. © 2019.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TenantAPI.Entities {
    
    
    public partial class State {
        
        [Key()]
        [Display(Name = "Id")]
        public int Id { get; set; }
        
        [Display(Name = "Abbreviation")]
        public string Abbreviation { get; set; }
        
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        /// Child Addresses where [Address].[StateId] point to this entity (FK_Address_State)
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
    }
}
