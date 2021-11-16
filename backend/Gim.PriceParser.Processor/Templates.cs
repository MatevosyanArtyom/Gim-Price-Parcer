namespace Gim.PriceParser.Processor
{
    public static class Templates
    {
        public const string ReplaceStart = "// REPLACE_START";
        public const string ReplaceFinish = "// REPLACE_FINISH";

        public const string Xlsx = @"
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using OfficeOpenXml;

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
        }
    }
}
        ";
    }
}