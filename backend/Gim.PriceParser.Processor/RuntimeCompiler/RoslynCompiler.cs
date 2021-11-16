using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.DependencyModel;

namespace Gim.PriceParser.Processor.RuntimeCompiler
{
    /// <summary>
    ///     Компилирует библиотеку из шаблона и скрипта
    /// </summary>
    internal class RoslynCompiler : IRuntimeCompiler
    {
        /// <summary>
        ///     Компилирует библиотеку в runtime
        /// </summary>
        /// <param name="template">Шаблон</param>
        /// <param name="script">Скрипт</param>
        /// <returns></returns>
        public CompileResult Compile(string script, string template = null)
        {
            var code = script;
            if (!string.IsNullOrWhiteSpace(template))
            {
                var start = template.IndexOf(Templates.ReplaceStart, StringComparison.CurrentCulture);
                var finish = template.IndexOf(Templates.ReplaceFinish, StringComparison.CurrentCulture) +
                             Templates.ReplaceFinish.Length;
                var sb = new StringBuilder(template);
                sb.Remove(start, finish - start);
                sb.Insert(start, script);
                code = sb.ToString();
            }

            var result = new CompileResult();

            try
            {
                var tree = SyntaxFactory.ParseSyntaxTree(code, new CSharpParseOptions(LanguageVersion.LatestMajor));

                var references = DependencyContext.Default.CompileLibraries
                    .SelectMany(s => s.ResolveReferencePaths())
                    .Select(s => MetadataReference.CreateFromFile(s));

                Compilation compilation = CSharpCompilation
                    .Create($"{Guid.NewGuid()}.dll")
                    .AddReferences(references)
                    .AddSyntaxTrees(new List<SyntaxTree> {tree})
                    .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                using var stream = new MemoryStream();
                var emitResult = compilation.Emit(stream);
                if (emitResult.Success)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(stream.GetBuffer());
                    result.Assembly = assembly;
                }

                result.EmitResult = emitResult;
            }
            catch (Exception)
            {
                // ignored
            }

            return result;

        }
    }
}