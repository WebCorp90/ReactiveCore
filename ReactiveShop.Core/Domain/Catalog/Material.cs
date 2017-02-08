using System.ComponentModel.DataAnnotations;
using Webcorp.unite;

namespace ReactiveShop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a physical material
    /// </summary>
    public class Material : BaseEntity
    {
        /// <summary>
        /// Gets or sets Unique Code of a material ex:1.0976
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets Material Group
        /// </summary>
        [Required]
        public MaterialGroup Group { get; set; }

        /// <summary>
        /// Gets or set grade (ex: S355MC)
        /// </summary>
        [Required]
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets density. Needed for weight calculation
        /// </summary>
        [Required]
        public Density Density { get; set; }

        /// <summary>
        /// correspondance like DIN , AFNOR, JIS, UNI , ....
        /// </summary>
        public string[] Correspondance { get; set; }
    }

    /// <summary>
    /// @see CMC Classification
    /// </summary>
    public enum MaterialGroup
    {
        Acier = 0,      //   P / Acier=>0
        Inox,           //   M / Inox=>1
        Fonte,          //   K / Fontes=>2
        Aluminium,      //   N / Non Ferreux Aluminium=>3
        SuperAlliage,   //   S / Superalliage, Réfractaire=>4
        AcierTrempe     //   H / Aciers Trempés=>5
    }
}