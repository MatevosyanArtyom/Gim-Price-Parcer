namespace Gim.PriceParser.Processor.RuntimeCompiler
{
    public interface IRuntimeCompiler
    {
        CompileResult Compile(string script, string template = null);
    }
}