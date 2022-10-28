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
    
    
    public class AddressModel {
        
        protected internal System.Guid _id;
        
        protected internal string _userId;
        
        protected internal int _addressTypeId;
        
        protected internal int _stateId;
        
        protected internal string _addressLine1;
        
        protected internal string _addressLine2;
        
        protected internal string _city;
        
        protected internal string _postalCode;
        
        protected internal bool _isDefault;
        
        protected internal string _createdBy;
        
        protected internal System.DateTime _createdDate;
        
        protected internal string _updatedBy;
        
        protected internal System.DateTime? _updatedDate;
        
        public AddressModel() {
            this._id = SequentialGuid.NewGuid();
        }
        
        internal AddressModel(Address entity) {
            this._id = entity.Id;
            this._userId = entity.UserId;
            this._addressTypeId = entity.AddressTypeId;
            this._stateId = entity.StateId;
            this._addressLine1 = entity.AddressLine1;
            this._addressLine2 = entity.AddressLine2;
            this._city = entity.City;
            this._postalCode = entity.PostalCode;
            this._isDefault = entity.IsDefault;
            this._createdBy = entity.CreatedBy;
            this._createdDate = entity.CreatedDate;
            this._updatedBy = entity.UpdatedBy;
            this._updatedDate = entity.UpdatedDate;
        }
        
        [Required()]
        [Display(Name = "Id")]
        public System.Guid Id {
            get {
                return this._id;
            }
            set {
                this._id = value;
            }
        }
        
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
        
        [Required()]
        [Display(Name = "Address type ID")]
        public int AddressTypeId {
            get {
                return this._addressTypeId;
            }
            set {
                this._addressTypeId = value;
            }
        }
        
        [Required()]
        [Display(Name = "State ID")]
        public int StateId {
            get {
                return this._stateId;
            }
            set {
                this._stateId = value;
            }
        }
        
        [Required()]
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Name = "Address line 1")]
        public string AddressLine1 {
            get {
                return this._addressLine1;
            }
            set {
                this._addressLine1 = value;
            }
        }
        
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Name = "Address line 2")]
        public string AddressLine2 {
            get {
                return this._addressLine2;
            }
            set {
                this._addressLine2 = value;
            }
        }
        
        [Required()]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "City")]
        public string City {
            get {
                return this._city;
            }
            set {
                this._city = value;
            }
        }
        
        [Required()]
        [MaxLength(50)]
        [StringLength(50)]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postal code")]
        public string PostalCode {
            get {
                return this._postalCode;
            }
            set {
                this._postalCode = value;
            }
        }
        
        [Required()]
        [Display(Name = "Is default")]
        public bool IsDefault {
            get {
                return this._isDefault;
            }
            set {
                this._isDefault = value;
            }
        }
        
        [Required()]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Created by")]
        public string CreatedBy {
            get {
                return this._createdBy;
            }
            set {
                this._createdBy = value;
            }
        }
        
        [Required()]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created date")]
        public System.DateTime CreatedDate {
            get {
                return this._createdDate;
            }
            set {
                this._createdDate = value;
            }
        }
        
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Updated by")]
        public string UpdatedBy {
            get {
                return this._updatedBy;
            }
            set {
                this._updatedBy = value;
            }
        }
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated date")]
        public System.DateTime? UpdatedDate {
            get {
                return this._updatedDate;
            }
            set {
                this._updatedDate = value;
            }
        }
        
        public override int GetHashCode() {
            int hash = 0;
            hash ^=Id.GetHashCode();
            return hash;
        }
        
        public override string ToString() {
            return Id.ToString()
;
        }
        
        public override bool Equals(object obj) {
        bool result = false;

            if (obj is AddressModel) {
                AddressModel toCompare = (AddressModel)obj;
              if(toCompare != null)
              {
                  result = Equals(toCompare);
              }
            }

            return result;
        }
        
        public virtual bool Equals(AddressModel toCompare) {

        bool result = false;

            if (toCompare != null) {
                result = toCompare.Id.Equals(Id)
;
            }

            return result;
        }
    }
}
