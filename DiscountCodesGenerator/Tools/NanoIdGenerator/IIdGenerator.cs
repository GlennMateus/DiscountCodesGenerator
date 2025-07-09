namespace DiscountCodesGenerator.Tools.NanoIdGenerator;

public interface IIdGenerator
{
    string Generate(int size);
    Task<string> GenerateAsync(int size);
}
