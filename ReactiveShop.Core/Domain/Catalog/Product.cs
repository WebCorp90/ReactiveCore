
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
using ReactiveHelpers.Core.Attributes;

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
        [DataMember, Required]
        public string CreatedBy { get; set; } = "";
        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        [DataMember, Required]
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
        [Reactive]
        public bool MarkDeleted { get; set; }
        #endregion

        #region Product


        /// <summary>
        /// Unique Code+Complementaire Identifier
        /// </summary>
        [DataMember, Required(ErrorMessage = "Product.Code.Required")]
        [StringLength(24, MinimumLength = 3)]
        [Display(Name = "Product.Code")]
        [Reactive]
        [Index("IX_PRODUCT_SOC_CODE_COMPL",1)]
        public string Code { get; set; }

        /// <summary>
        /// Unique Code+Complementaire Identifier
        /// </summary>
        [DataMember, Required(ErrorMessage = "Product.Complementary.Required")]
        [StringLength(6)]
        [Reactive]
        [Index("IX_PRODUCT_SOC_CODE_COMPL", 2)]
        public string Complementaire { get; set; }

        /// <summary>
        /// Get or set Product Type
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        [DataMember]
        [Reactive]
        public string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        [DataMember]
        [Reactive]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the product on home page
        /// </summary>
        [DataMember]
        [Reactive]
        public bool ShowOnHomePage { get; set; }

        /// <summary>
		/// Gets or sets the display order for homepage products
		/// </summary>
		[DataMember]
        [Reactive]
        public int HomePageDisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
		[DataMember]
        [Reactive]
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        [DataMember]
        [Reactive]
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
		[DataMember]
        [Reactive]
        public string MetaTitle { get; set; }

        /// <summary>
		/// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
		/// </summary>
		[DataMember]
        [Reactive]
        public bool LimitedToStores { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is ship enabled
        /// </summary>
        [DataMember]
        [Reactive]
        public bool IsShipEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is free shipping
        /// </summary>
        [DataMember]
        [Reactive]
        public bool IsFreeShipping { get; set; }



        /// <summary>
        /// Gets or sets the Global Trade Item Number (GTIN). These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
        /// </summary>
        [DataMember]
        [Reactive]
        public string Gtin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is gift card
        /// </summary>
        [DataMember]
        [Reactive]
        public bool IsGiftCard { get; set; }

        /// <summary>
		/// Gets or sets the gift card type identifier
		/// </summary>
		[DataMember]
        [Reactive]
        public int GiftCardTypeId { get; set; }

        /// <summary>
		/// Gets or sets a value indicating whether to disable buy (Add to cart) button
		/// </summary>
		[DataMember]
        [Reactive]
        public bool DisableBuyButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable "Add to wishlist" button
        /// </summary>
        [DataMember]
        [Reactive]
        public bool DisableWishlistButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is available for Pre-Order
        /// </summary>
        [DataMember]
        [Reactive]
        public bool AvailableForPreOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show "Call for Pricing" or "Call for quote" instead of price
        /// </summary>
        [DataMember]
        [Reactive]
        public bool CallForPrice { get; set; }

        /// <summary>
        /// Gets or sets the mass of the product
        /// can be calculated
        /// </summary>
        [DataMember]
        [Reactive]
        public Mass Mass { get; set; } = new Mass();

        /// <summary>
        /// Gets or set if the mass must be auto calculated from length/width/Thickness/Density
        /// </summary>
        [DataMember]
        [Reactive]
        public bool MassAutoCalculated { get; set; }

        /// <summary>
        /// Get or sets the length of the product
        /// </summary>
        [DataMember]
        [Reactive]
        public Length Length { get; set; } = new Length();

        /// <summary>
        /// Gets or set Width of the product
        /// </summary>
        [DataMember]
        [Reactive]
        public Length Width { get; set; } = new Length();

        /// <summary>
        /// Gets or set the thickness of the product
        /// </summary>
        [DataMember]
        [Reactive]
        public Length Thickness { get; set; } = new Length();

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
        public MassLinear MassLinear { get; set; } = new MassLinear();

        #region STOCKS
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
        public double QuantiteReservee { get; set; }

        [DataMember]
        [Reactive]
        public double QuantiteAttendue { get; set; }

        public double StockDisponible => StockPhysique - QuantiteReservee;

        [DataMember]
        [Reactive]
        public double StockMini { get; set; }

        [DataMember]
        [Reactive]
        public double StockMaxi { get; set; }

        [DataMember]
        [Reactive]
        public double ReapproMini { get; set; }

        [DataMember]
        [Reactive]
        public double ReapproLot { get; set; }
        #endregion

        #region COUTS
        [DataMember]
        [Reactive]
        public Currency CoutPreparation { get; set; } = new Currency();
        [DataMember]
        [Reactive]
        public Currency CoutMatierePremiere { get; set; } = new Currency();
        [DataMember]
        [Reactive]
        public Currency CoutMainOeuvre { get; set; } = new Currency();
        [DataMember]
        [Reactive]
        public Currency CoutSousTraitance { get; set; } = new Currency();
        [DataMember]
        [Reactive]
        public Currency CoutFraisGeneraux { get; set; } = new Currency();

        [DataMember]
        [Reactive]
        public Currency CoutTotal { get; set; } = new Currency();


        public string _CoutMo{ get { return CoutMainOeuvre.ToString(); }  set { CoutMainOeuvre = new Currency(value); } }
        #endregion

        #endregion

        public override void Initialize()
        {
            base.Initialize();
            Changed.Where(p => MassAutoCalculated && new List<string>() { "Length", "Width", "Thickness", "Material", "MassLinear" }.Contains(p.PropertyName)).Subscribe(e => CalculateMass());
            Changed.Where(p => new List<string>() {"CoutPreparation","CoutMatierePremiere","CoutMainOeuvre","CoutSousTraitance","CoutFraisGeneraux" }.Contains(p.PropertyName)).Subscribe(e => CalculateCout());
        }

        protected virtual void CalculateMass()
        {
            if (MassLinear.IsNotNull() )
                Mass = MassLinear * Length;
            else /*if (Length.IsNotNull() && Width.IsNotNull() && Thickness.IsNotNull() && Material.IsNotNull())*/
                Mass = (Length * Width * Thickness) * Material?.Density;
        }

        protected virtual void CalculateCout()
        {
            CoutTotal = CoutMatierePremiere + CoutMainOeuvre + CoutSousTraitance + CoutFraisGeneraux;
        }
    }
}
