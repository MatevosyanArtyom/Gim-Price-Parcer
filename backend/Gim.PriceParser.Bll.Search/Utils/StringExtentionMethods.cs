namespace Gim.PriceParser.Bll.Search.Utils
{
    public static class StringExtentionMethods
    {
        public static string ToCamelCase(this string s)
        {
            return s.Substring(0, 1).ToLower() + s.Substring(1, s.Length - 1);
        }
    }
}