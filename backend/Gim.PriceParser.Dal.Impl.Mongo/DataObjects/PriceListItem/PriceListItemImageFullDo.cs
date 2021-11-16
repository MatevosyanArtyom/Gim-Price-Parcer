using System.Collections.Generic;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem
{
    internal class PriceListItemImageFullDo : PriceListItemImageDo
    {
        public List<ImageDo> GimImages { get; set; }
    }
}