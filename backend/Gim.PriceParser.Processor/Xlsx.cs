using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;
using Gim.PriceParser.Processor.RuntimeCompiler;
using Gim.PriceParser.Processor.TemplatesSource;
using OfficeOpenXml;

namespace Gim.PriceParser.Processor
{
    public static class Xlsx
    {
        public static List<PriceListItemSource> Parse(Assembly assembly, string data)
        {
            var bytes = Convert.FromBase64String(data);

            using (var stream = new MemoryStream(bytes))
            {
                using (var ep = new ExcelPackage(stream))
                {
                    var type = assembly.GetTypes().First(x => x.Name == nameof(XlsxTemplate));
                    var method = type.GetMethod(nameof(XlsxTemplate.Evaluate));
                    var obj = assembly.CreateInstance($"{type.Namespace}.{nameof(XlsxTemplate)}");
                    var result = (List<PriceListItemSource>) method?.Invoke(obj, BindingFlags.InvokeMethod, null,
                        new object[] {ep}, CultureInfo.CurrentCulture);
                    return result;
                }
            }
        }

        public static List<PriceListItemSource> Parse(SchedulerTask task, string data)
        {
            var bytes = Convert.FromBase64String(data);

            using (var stream = new MemoryStream(bytes))
            {
                using (var ep = new ExcelPackage(stream))
                {
                    var compiler = new RoslynCompiler();
                    var compileResult = compiler.Compile(Templates.Xlsx, task.Script);

                    if (!compileResult.EmitResult.Success)
                    {
                        return null;
                    }

                    var assembly = compileResult.Assembly;
                    var type = assembly.GetTypes().First(x => x.Name == nameof(XlsxTemplate));
                    var method = type.GetMethod(nameof(XlsxTemplate.Evaluate));
                    var obj = assembly.CreateInstance($"{type.Namespace}.{nameof(XlsxTemplate)}");
                    var result = (List<PriceListItemSource>) method?.Invoke(obj, BindingFlags.InvokeMethod, null,
                        new object[] {ep}, CultureInfo.CurrentCulture);
                    return result;
                }
            }
        }
    }
}