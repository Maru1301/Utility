using Utility;

namespace InfrastructureTests
{
    internal class HashUtilityTest
    {
        [Test]
        public void ToSHA256_Empty_String_Produces_Expected_Hash()
        {
            string plainText = "";
            string salt = "anySalt";
            string expectedHash = "49643FFC238C09DC0D324A0C56CCEA86DCACA897EF024278B8D3E81AFD635E14";

            string actualHash = HashUtility.ToSHA256(plainText, salt);

            Assert.That(actualHash, Is.EqualTo(expectedHash));
        }

        [Test]
        public void ToSHA256_Specific_String_Produces_Expected_Hash()
        {
            string plainText = "This is a test string!";
            string salt = "customSalt";
            string expectedHash = "662981264435F2F7EC8A28C9299C302B2472BB094646C0837A22B9AC45FC2AFA";

            string actualHash = HashUtility.ToSHA256(plainText, salt);

            Assert.That(actualHash, Is.EqualTo(expectedHash));
        }
    }
}
