using System.Collections.Generic;

namespace Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues
{
    public class CategoryPropertyValueFilter
    {
        public List<string> PropertiesIds { get; set; }
        public string PropertyId { get; set; }
        public List<string> ValuesIds { get; set; }
    }
}
