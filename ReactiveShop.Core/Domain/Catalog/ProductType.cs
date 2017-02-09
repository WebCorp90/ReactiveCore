using System.ComponentModel;

namespace ReactiveShop.Core.Domain.Catalog
{
    public enum ProductType
    {
        [Description("Produit Fini")]
        Fini,
        [Description("Produit Semi-Fini")]
        SemiFini,
        [Description("Matière Premiere")]
        MatierePremiere,
        [Description("Libellé")]
        Libelle,
        [Description("Frais Generaux")]
        FraisGeneraux,
        [Description("Sous-Traitance")]
        SousTraitance,
        [Description("Composant")]
        Composant,
        [Description("Tole")]
        Tole,
        [Description("Profilé")]
        Profile,
        [Description("Chiffrage")]
        Chiffrage
    }
}