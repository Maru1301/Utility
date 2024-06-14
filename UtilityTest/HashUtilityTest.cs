using Utility;

namespace InfrastructureTests;

public class HashUtilityTest
{
    [Theory]
    [InlineData("", "anySalt", "49643FFC238C09DC0D324A0C56CCEA86DCACA897EF024278B8D3E81AFD635E14")]
    public void ToSHA256_Empty_String_Produces_Expected_Hash(string plainText, string salt, string expectedHash)
    {
        string actualHash = HashUtility.ToSHA256(plainText, salt);

        Assert.Equal(actualHash, expectedHash);
    }

    [Theory]
    [InlineData("This is a test string!", "customSalt", "662981264435F2F7EC8A28C9299C302B2472BB094646C0837A22B9AC45FC2AFA")]
    public void ToSHA256_Specific_String_Produces_Expected_Hash(string plainText, string salt, string expectedHash)
    {
        string actualHash = HashUtility.ToSHA256(plainText, salt);

        Assert.Equal(actualHash, expectedHash);
    }
}
