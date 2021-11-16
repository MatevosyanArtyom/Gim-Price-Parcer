using System.Collections.Generic;

namespace Gim.PriceParser.Bll.Common.Entities.Images
{
    public class GimImageFilter
    {
        public List<string> Ids { get; set; }
        public string ProductId { get; set; }
        public bool? IsMain { get; set; }
        public GimImageDownloadStatus? Status { get; set; }
    }
}