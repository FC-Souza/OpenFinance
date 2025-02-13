using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("mutual_fund")]
    public class MutualFundModel
    {
        /*
         * Table: Fund
         * mf_id (PK, int, not null)
         * product_id (FK, int, not null)
         * mf_name (varchar(100), not null)
         * mf_symbol (varchar(100), not null)
         * mf_type (varchar(100), not null)
         * mf_currency (varchar(100), not null)
         * mf_price (decimal(10,2), not null)
         * mf_nav (decimal(5,5))
         * mf_quantity (int, not null)
         * mf_inception_date (datetime, not null)
         * mf_management_fee (decimal(5,5), not null)
         * last_updated (datetime, not null)
         */

        [Key]
        [Column("mf_id")]
        public int MFID { get; set; }

        [Column("product_id")]
        public int productId { get; set; }
        [ForeignKey("product_types")]
        public ProductTypesModel Product { get; set; }

        [Column("mf_name")]
        public string MFName { get; set; }

        [Column("mf_symbol")]
        public string MFSymbol { get; set; }

        [Column("mf_type")]
        public string MFType { get; set; }

        [Column("mf_currency")]
        public string MFCurrency { get; set; }

        [Column("mf_last_nav")]
        public decimal MFNAV { get; set; }

        [Column("mf_inception_date")]
        public DateOnly MFInceptionDate { get; set; }

        [Column("mf_management_fee")]
        public decimal MFManagementFee { get; set; }

        [Column("last_updated")]
        public DateTime lastUpdated { get; init; }


    }
}
