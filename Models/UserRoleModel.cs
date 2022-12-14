// ----------------------------------------------------------------------------------
// <copyright company="Exesoft Inc.">
//	This code was generated by Instant Web API code automation software (https://www.InstantWebAPI.com)
//	Copyright Exesoft Inc. © 2019.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TenantAPI.Entities;
using TenantAPI.Models;

namespace TenantAPI.Models {
    
    
    public class UserRoleModel {
        
        protected internal int _roleId;
        
        protected internal string _userId;
        
        public UserRoleModel() {
        }
        
        internal UserRoleModel(UserRole entity) {
            this._roleId = entity.RoleId;
            this._userId = entity.UserId;
        }
        
        [Required()]
        [Display(Name = "Role ID")]
        public int RoleId {
            get {
                return this._roleId;
            }
            set {
                this._roleId = value;
            }
        }

        public string RoleName { get; internal set; }

        [Required()]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "User ID")]
        public string UserId {
            get {
                return this._userId;
            }
            set {
                this._userId = value;
            }
        }

        public string UserName { get; internal set; }

        public override int GetHashCode() {
            int hash = 0;
            hash ^=RoleId.GetHashCode();
            hash ^=UserId.GetHashCode();
            return hash;
        }
        
        public override string ToString() {
            return RoleId.ToString()
                 + "-" + UserId
;
        }
        
        public override bool Equals(object obj) {
        bool result = false;

            if (obj is UserRoleModel) {
                UserRoleModel toCompare = (UserRoleModel)obj;
              if(toCompare != null)
              {
                  result = Equals(toCompare);
              }
            }

            return result;
        }
        
        public virtual bool Equals(UserRoleModel toCompare) {

        bool result = false;

            if (toCompare != null) {
                result = toCompare.RoleId == RoleId
             && string.Compare(toCompare.UserId, UserId, true) == 0
;
            }

            return result;
        }
    }
}

