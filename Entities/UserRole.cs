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
    
    
    public partial class UserRole {
        
        [Key()]
        [Column(Order=1)]
        [Display(Name = "Role ID")]
        public int RoleId { get; set; }
        
        [Key()]
        [Column(Order=2)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        
        // Parent Role pointed by [UserRole].([RoleId]) (FK_UserRole_Role)
        public virtual Role Role { get; set; }
        
        // Parent User pointed by [UserRole].([UserId]) (FK_UserRole_User)
        public virtual User User { get; set; }
    }
}

