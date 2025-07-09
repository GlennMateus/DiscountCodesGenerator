
namespace DiscountCodesGenerator.Tools.NanoIdGenerator;

public class IdGenerator : IIdGenerator
{
    private string alphabet = Nanoid.Alphabets.LettersAndDigits;

    public string Generate(int size) => Nanoid.Generate(alphabet, size);

    public Task<string> GenerateAsync(int size) => Nanoid.GenerateAsync(alphabet, size);
}
