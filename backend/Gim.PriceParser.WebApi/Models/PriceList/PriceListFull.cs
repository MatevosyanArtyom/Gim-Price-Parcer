using System;
using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.WebApi.Models.GimFile;

namespace Gim.PriceParser.WebApi.Models.PriceList
{
    public class PriceListFull : PriceListBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }

        [Required]
        public string SupplierId { get; set; }

        [Required]
        public string ProcessingRuleId { get; set; }

        [Required]
        public GimFileFull PriceListFile { get; set; }

        public bool HasUnprocessedCodeErrors { get; set; }
        public bool HasUnprocessedNameErrors { get; set; }
        public bool HasUnprocessedPriceErrors { get; set; }
        public bool HasUnprocessedErrors { get; set; }
        public bool HasPropertiesErrors { get; set; }
        public bool CreateProperties { get; set; }

        [Required]
        public PriceListStatus Status { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public DateTime StatusDate { get; set; }
        public DateTime? ParsedDate { get; set; }
    }
}