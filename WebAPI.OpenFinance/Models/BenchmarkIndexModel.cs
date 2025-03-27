using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAPI.OpenFinance.Models
{
    [Table("benchmark_index")]
    public class BenchmarkIndexModel
    {
        /*
        * Table: benchmark_indexes
        * benchmark_id (PK, int, not null)
        * benchmark_period (date, not null)
        * sptsx (decimal(5,2), not null)
        * cpi (decimal(5,2), not null)
        * cbpr (decimal(5,2), not null)
        */

        [Key]
        [Column("benchmark_id")]
        public int BenchmarkID { get; set; }

        [Required]
        [Column("benchmark_period", TypeName = "date")]
        public DateTime BenchmarkPeriod { get; set; }

        [Required]
        [Column("sp_tsx", TypeName = "decimal(5,2)")]
        public decimal SPTSX { get; set; }

        [Required]
        [Column("cpi", TypeName = "decimal(5,2)")]
        public decimal CPI { get; set; }

        [Required]
        [Column("cbpr", TypeName = "decimal(5,2)")]
        public decimal CBPR { get; set; }
    }
}
