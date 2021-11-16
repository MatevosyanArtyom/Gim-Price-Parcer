using System;

namespace Gim.PriceParser.WebApi.Util
{
    public static class MiscExtensions
    {
        public static string LowcaseFirstLetter(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1);
        }

        public static Type GetThisOrUnderlyingNullableType(this Type t)
        {
            var u = Nullable.GetUnderlyingType(t);
            return u ?? t;
        }

        public static bool IsEnumOrNullableEnum(this Type t)
        {
            if (t.IsEnum)
            {
                return true;
            }

            var u = Nullable.GetUnderlyingType(t);
            return u != null && u.IsEnum;
        }
    }
}