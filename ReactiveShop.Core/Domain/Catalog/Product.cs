using PropertyChangedCore.Helpers;
using ReactiveCore;
using ReactiveDbCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ReactiveShop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product either in Store or ERP
    /// <see cref="https://github.com/smartstoreag/SmartStoreNET/blob/3.x/src/Libraries/SmartStore.Core/Domain/Catalog/Product.cs"/>
    /// </summary>
    public class Product : BaseEntity, IAuditable, ISoftDeletable//, ILocalizedEntity, ISlugSupported, IAclSupported, IStoreMappingSupported, IMergedData
    {
        #region IAuditable
        /// <summary>
        /// Gets or sets the identy user of entity creation
        /// </summary>
        [DataMember, Reactive,Required]
        public string CreatedBy { get; set; } = "";
        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        [DataMember, Reactive,Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets the identy user of entity update
        /// </summary>
        [DataMember, Reactive]
        public string UpdatedBy { get; set; } = null;
        /// <summary>
        /// Gets or sets the date and time of entity update
        /// </summary>
        [DataMember,Reactive]
        public DateTime? UpdatedOn { get; set; } = null;




        #endregion

        #region ISoftDeletable
        /// <summary>
        /// Mark as Soft Deleted
        /// MarkDeleted Is a Database Index
        /// </summary>
        [DataMember, Reactive]
        public bool MarkDeleted { get; set; }
        #endregion

        #region Product
        /// <summary>
        /// Unique Code+Complementaire Identifier
        /// </summary>
        [Reactive,DataMember,Required(ErrorMessage ="Product.Code.Required")]
        [StringLength(24,MinimumLength =3)]
        [Display(Name = "Product.Code")]
        public string Code { get; set; }

        /// <summary>
        /// Unique Code+Complementaire Identifier
        /// </summary>
        [Reactive,DataMember,Required(ErrorMessage = "Product.Complementary.Required")]
        [StringLength(6)]
        public string Complementary { get; set; }

        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        [DataMember]
        public string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        [DataMember]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the product on home page
        /// </summary>
        [DataMember]
        public bool ShowOnHomePage { get; set; }

        /// <summary>
		/// Gets or sets the display order for homepage products
		/// </summary>
		[DataMember]
        public int HomePageDisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
		[DataMember]
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        [DataMember]
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
		[DataMember]
        public string MetaTitle { get; set; }

        /// <summary>
		/// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
		/// </summary>
		[DataMember]
        public bool LimitedToStores { get; set; }
        #endregion
    }
}
