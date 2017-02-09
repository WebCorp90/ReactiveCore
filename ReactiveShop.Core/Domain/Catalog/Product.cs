﻿using PropertyChangedCore.Helpers;
using ReactiveCore;
using ReactiveDbCore;
using ReactiveHelpers.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Reactive.Linq;
#if CORE
using Microsoft.EntityFrameworkCore.Metadata.Internal;
#else

#endif
using Webcorp.unite;
using System.Diagnostics;

namespace ReactiveShop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product either in Store or ERP
    /// <see cref="https://github.com/smartstoreag/SmartStoreNET/blob/3.x/src/Libraries/SmartStore.Core/Domain/Catalog/Product.cs"/>
    /// </summary>
    [DebuggerDisplay("Societe={Societe} Code={Code},Libelle={Libelle}")]
    public partial class Product : BaseEntity, IAuditable, ISoftDeletable//, ILocalizedEntity, ISlugSupported, IAclSupported, IStoreMappingSupported, IMergedData
    {
#region IAuditable
        /// <summary>
        /// Gets or sets the identy user of entity creation
        /// </summary>
        [DataMember,Required]
        public string CreatedBy { get; set; } = "";
        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        [DataMember,Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets the identy user of entity update
        /// </summary>
        [DataMember]
        public string UpdatedBy { get; set; } = null;
        /// <summary>
        /// Gets or sets the date and time of entity update
        /// </summary>
        [DataMember]
        public DateTime? UpdatedOn { get; set; } = null;




#endregion

#region ISoftDeletable
        /// <summary>
        /// Mark as Soft Deleted
        /// MarkDeleted Is a Database Index
        /// </summary>
        [DataMember]
        public bool MarkDeleted { get; set; }
#endregion

#region Product


        /// <summary>
        /// Unique Code+Complementaire Identifier
        /// </summary>
        [DataMember,Required(ErrorMessage ="Product.Code.Required")]
        [StringLength(24,MinimumLength =3)]
        [Display(Name = "Product.Code")]
        public string Code { get; set; }

        /// <summary>
        /// Unique Code+Complementaire Identifier
        /// </summary>
        [DataMember,Required(ErrorMessage = "Product.Complementary.Required")]
        [StringLength(6)]
        public string Complementary { get; set; }

        /// <summary>
        /// Get or set Product Type
        /// </summary>
        public ProductType ProductType { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether the entity is ship enabled
        /// </summary>
        [DataMember]
        public bool IsShipEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is free shipping
        /// </summary>
        [DataMember]
        public bool IsFreeShipping { get; set; }



        /// <summary>
        /// Gets or sets the Global Trade Item Number (GTIN). These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
        /// </summary>
        [DataMember]
        public string Gtin { get; set; }
        
        /// <summary>
		/// Gets or sets a value indicating whether the product is gift card
		/// </summary>
		[DataMember]
        public bool IsGiftCard { get; set; }

        /// <summary>
		/// Gets or sets the gift card type identifier
		/// </summary>
		[DataMember]
        public int GiftCardTypeId { get; set; }

        /// <summary>
		/// Gets or sets a value indicating whether to disable buy (Add to cart) button
		/// </summary>
		[DataMember]
        public bool DisableBuyButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable "Add to wishlist" button
        /// </summary>
        [DataMember]
        public bool DisableWishlistButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is available for Pre-Order
        /// </summary>
        [DataMember]
        public bool AvailableForPreOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show "Call for Pricing" or "Call for quote" instead of price
        /// </summary>
        [DataMember]
        public bool CallForPrice { get; set; }

        /// <summary>
        /// Gets or sets the mass of the product
        /// can be calculated
        /// </summary>
        [DataMember]
        public Mass Mass { get; set; }

        /// <summary>
        /// Gets or set if the mass must be auto calculated from length/width/Thickness/Density
        /// </summary>
        [DataMember]
        public bool MassAutoCalculated { get; set; }

        /// <summary>
        /// Get or sets the length of the product
        /// </summary>
        [DataMember]
        [Reactive]
        public Length Length { get; set; }

        /// <summary>
        /// Gets or set Width of the product
        /// </summary>
        [DataMember]
        [Reactive]
        public Length Width { get; set; }

        /// <summary>
        /// Gets or set the thickness of the product
        /// </summary>
        [DataMember]
        [Reactive]
        public Length Thickness { get; set; }

        /// <summary>
        /// Gets or sets the material of the product
        /// </summary>
        [DataMember]
        [Reactive]
        public Material Material { get; set; }

        /// <summary>
        /// Gets or set Linear mass for profile per exemple Carre/HEA/ Plat
        /// </summary>
        [DataMember]
        [Reactive]
        public MassLinear MassLinear { get; set; }


        /// <summary>
		/// Gets or sets a value indicating whether to display stock availability
		/// </summary>
		[DataMember]
        [Reactive]
        public bool DisplayStockAvailability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display stock quantity
        /// </summary>
        [DataMember]
        [Reactive]
        public bool DisplayStockQuantity { get; set; }

        [DataMember]
        [Reactive]
        public bool GereEnStock { get; set; }

        [DataMember]
        [Reactive]
        public bool AutoriseStockNegatif { get; set; }

        [DataMember]
        [Reactive]
        public double StockPhysique { get; set; }

        [DataMember]
        [Reactive]
        public double  QuantiteReservee { get; set; }

        [DataMember]
        [Reactive]
        public double QuantiteAttendue { get; set; }

        [DataMember]
        [Reactive]
        public double  StockMini { get; set; }

        [DataMember]
        [Reactive]
        public double  StockMaxi { get; set; }

        [DataMember]
        [Reactive]
        public double ReapproMini { get; set; }

         [DataMember]
        [Reactive]
        public double ReapproLot { get; set; }


        #endregion

        public override void Initialize()
        {
            base.Initialize();
            Changed.Where(p => MassAutoCalculated && new List<string>() { "Length", "Width", "Thickness", "Material", "MassLinear" }.Contains(p.PropertyName)).Subscribe(e => CalculateMass());
        }

        protected void CalculateMass()
        {
            if (MassLinear.IsNotNull() && Length.IsNotNull())
                Mass = MassLinear * Length;
            else if (Length.IsNotNull() && Width.IsNotNull() && Thickness.IsNotNull() && Material.IsNotNull())
                Mass = Length * Width * Thickness * Material.Density;
        }
    }
}
