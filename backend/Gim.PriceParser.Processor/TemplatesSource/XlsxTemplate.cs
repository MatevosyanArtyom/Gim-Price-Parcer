// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using OfficeOpenXml;

// ReSharper disable HeuristicUnreachableCode

namespace Gim.PriceParser.Processor.TemplatesSource
{
    public class XlsxTemplate
    {
        public List<PriceListItemSource> Evaluate(ExcelPackage ep)
        {
            // REPLACE_START
            // just for compilation succeeding
            return new List<PriceListItemSource>();
            // REPLACE_FINISH

#pragma warning disable 162
            var items = new List<PriceListItemSource>();

            var sheets = ep.Workbook.Worksheets;

            foreach (var sheet in sheets)
            {
                //for (var i = 2; i < sheet.Dimension.Rows; i++)
                for (var i = 2; i < 5; i++)
                {
                    var path = sheet.GetValue<string>(i, 9);
                    var categories = Regex.Split(path, "->");
                    var category1 = categories.FirstOrDefault();
                    var category2 = categories.Skip(1).FirstOrDefault();
                    var category3 = categories.Skip(2).FirstOrDefault();
                    var category4 = categories.Skip(3).FirstOrDefault();
                    var category5 = categories.Skip(4).FirstOrDefault();

                    var format = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = "."
                    };
                    // ReSharper disable once ConvertToLocalFunction
                    Func<string, decimal?> tryParse = s =>
                        decimal.TryParse(s, NumberStyles.AllowDecimalPoint, format, out var price)
                            ? price
                            : (decimal?) null;

                    var price1 = tryParse(sheet.GetValue<string>(i, 10));
                    var price2 = tryParse(sheet.GetValue<string>(i, 12));
                    var imagesLinks = sheet.GetValue<string>(i, 22);

                    var item = new PriceListItemSource
                    {
                        Code = sheet.GetValue<string>(i, 6),
                        Category1Name = category1,
                        Category2Name = category2,
                        Category3Name = category3,
                        Category4Name = category4,
                        Category5Name = category5,
                        ProductName = sheet.GetValue<string>(i, 8),
                        Price1 = price1 ?? 0,
                        Price2 = price2,
                        Quantity = tryParse(sheet.GetValue<string>(i, 13)),
                        Description = sheet.GetValue<string>(i, 15) ?? "",
                        Images = imagesLinks.Split(' ').ToList()
                    };
                    item.Properties.Add("manufacturer", sheet.GetValue<string>(i, 16));
                    item.Properties.Add("fm", sheet.GetValue<string>(i, 25));
                    items.Add(item);
                }
            }

            return items;
#pragma warning restore 162
        }
    }
}