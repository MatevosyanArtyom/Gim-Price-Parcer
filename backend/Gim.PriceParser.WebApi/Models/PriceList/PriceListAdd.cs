using Gim.PriceParser.WebApi.Models.GimFile;

namespace Gim.PriceParser.WebApi.Models.PriceList
{
    public class PriceListAdd : PriceListBase
    {
        public string Code { get; set; }
        //public GimFileAdd CodeFile { get; set; }
        public GimFileAdd PriceListFile { get; set; }
    }
}