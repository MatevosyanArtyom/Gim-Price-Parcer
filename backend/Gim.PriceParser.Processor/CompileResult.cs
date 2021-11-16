using System.Reflection;
using Microsoft.CodeAnalysis.Emit;

namespace Gim.PriceParser.Processor
{
    /// <summary>
    ///     Результат компиляции библиотеки
    /// </summary>
    public class CompileResult
    {
        /// <summary>
        ///     Библиотека
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        ///     Результат компиляции
        /// </summary>
        public EmitResult EmitResult { get; set; }
    }
}