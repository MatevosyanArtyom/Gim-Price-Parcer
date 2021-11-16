namespace Gim.PriceParser.WebApi.Models.Category
{
    public class CategoryFull : CategoryBase
    {
        public string Path { get; set; }
        public string Parent { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
    }
}