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
    
    
    public class StateModel {
        
        protected internal int _id;
        
        protected internal string _abbreviation;
        
        protected internal string _name;
        
        public StateModel() {
        }
        
        internal StateModel(State entity) {
            this._id = entity.Id;
            this._abbreviation = entity.Abbreviation;
            this._name = entity.Name;
        }
        
        [Display(Name = "Id")]
        public int Id {
            get {
                return this._id;
            }
            set {
                this._id = value;
            }
        }
        
        [Required()]
        [MaxLength(3)]
        [StringLength(3)]
        [Display(Name = "Abbreviation")]
        public string Abbreviation {
            get {
                return this._abbreviation;
            }
            set {
                this._abbreviation = value;
            }
        }
        
        [Required()]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name {
            get {
                return this._name;
            }
            set {
                this._name = value;
            }
        }
        
        /// Child Addresses where [Address].[StateId] point to this entity (FK_Address_State)
        public virtual ICollection<AddressModel> AddressesModel { get; set; } = new HashSet<AddressModel>();
        
        public override int GetHashCode() {
            int hash = 0;
            hash ^=Abbreviation.GetHashCode();
            hash ^=Name.GetHashCode();
            return hash;
        }
        
        public override string ToString() {
            return Abbreviation
                 + "-" + Name
;
        }
        
        public override bool Equals(object obj) {
        bool result = false;

            if (obj is StateModel) {
                StateModel toCompare = (StateModel)obj;
              if(toCompare != null)
              {
                  result = Equals(toCompare);
              }
            }

            return result;
        }
        
        public virtual bool Equals(StateModel toCompare) {

        bool result = false;

            if (toCompare != null) {
                result = string.Compare(toCompare.Abbreviation, Abbreviation, true) == 0
             && string.Compare(toCompare.Name, Name, true) == 0
;
            }

            return result;
        }
    }
}
