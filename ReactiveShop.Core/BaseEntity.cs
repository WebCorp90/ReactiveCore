
using ReactiveDbCore;
using ReactiveHelpers.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace ReactiveShop.Core
{
    /// <summary>
    /// Base Class for all Shop Entities stored in DB
    /// </summary>
    public abstract partial class BaseEntity:ReactiveDbObject,IInitializable
    {
        /// <summary>
        /// Blank Ctor for EntityFactory and EF
        /// </summary>
        public BaseEntity()
        {
            Initialize();
        }
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
		[DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Reactive]
        public int Id { get; set; }


        /// <summary>
        /// Get or set the company code (ex: 001, or 123)
        /// must be on three characters
        /// </summary>
        [DataMember,Required,StringLength(maximumLength:3,MinimumLength =3)]
        //[Reactive]
        public string Societe { get; set; }

        /// <summary>
        /// Get Unproxied Type
        /// </summary>
        /// <returns>Type</returns>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public Type GetUnproxiedType()
        {
            var t = GetType();
            if (t.AssemblyQualifiedName.StartsWith("System.Data.Entity."))
            {
                // it's a proxied type
                t = t.GetElementType();// BaseType;
            }
            return t;
        }

        /// <summary>
		/// Transient objects are not associated with an item already in storage.  For instance,
		/// a Product entity is transient if its Id is 0.
		/// </summary>
		public virtual bool IsTransientRecord()
        {
            return Id == 0;
        }

        /// <summary>
        /// Test equality of 2 Base Entity Objects
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>return true if obj equal this object</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        protected virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (HasSameNonDefaultIds(other))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.Equals(otherType);
            }

            return false;
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            if (IsTransientRecord())
            {
                return base.GetHashCode();
            }
            else
            {
                unchecked
                {
                    // It's possible for two objects to return the same hash code based on
                    // identically valued properties, even if they're of two different types,
                    // so we include the object's type in the hash calculation
                    var hashCode = GetUnproxiedType().GetHashCode();
                    return (hashCode * 31) ^ Id.GetHashCode();
                }
            }
        }

        /// <summary>
        /// Compare 2 Base Entity Object if are equal
        /// </summary>
        /// <param name="x">First BaseEntity</param>
        /// <param name="y">Second BaseEntity</param>
        /// <returns>True if equal, false otherwise</returns>
        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Compare 2 Base Entity Object if are different
        /// </summary>
        /// <param name="x">First BaseEntity</param>
        /// <param name="y">Second BaseEntity</param>
        /// <returns>True if different, false otherwise</returns>
        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Compare Id
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if Ids are equal</returns>
        private bool HasSameNonDefaultIds(BaseEntity other)
        {
            return !this.IsTransientRecord() && !other.IsTransientRecord() && this.Id == other.Id;
        }

        #region IInitializable
        public virtual void Initialize()
        {
           
        }
        #endregion
    }
}
