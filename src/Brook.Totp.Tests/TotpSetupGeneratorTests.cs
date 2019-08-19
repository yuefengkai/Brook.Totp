using Xunit;

namespace Brook.Totp.Tests
{
    public class TotpSetupGeneratorTests
    {
        public TotpSetupGeneratorTests()
        {
            totpSetupGenerator = new TotpSetupGenerator();
        }

        private readonly TotpSetupGenerator totpSetupGenerator;

        [Fact]
        public void GenerateSetupCode_shouldNotBeNull_manuelTest_workWithGoogleAuthenticator()
        {
            var totpSetup = totpSetupGenerator.GenerateBase64("Super Totp Tester", "Damir Kusar",
                "7FF3F52B-2BE1-41DF-80DE-04D32171F8A3",1);
            Assert.NotNull(totpSetup);
            Assert.Equal(1, 1);
            Assert.Equal("G5DEMM2GGUZEELJSIJCTCLJUGFCEMLJYGBCEKLJQGRCDGMRRG4YUMOCBGM", totpSetup.ManualSetupKey);
            Assert.Equal(
                "data:image/png;base64,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",
                totpSetup.QrCodeImageBase64);
        }

        [Fact]
        public void GenerateSetupCodeUrl_shouldNotBeNull_manuelTest_workWithGoogleAuthenticator()
        {
            var totpSetup = totpSetupGenerator.GenerateUrl("Super Totp Tester", "Damir Kusar",
                "7FF3F52B-2BE1-41DF-80DE-04D32171F8A3");
            Assert.NotNull(totpSetup);
            Assert.Equal("G5DEMM2GGUZEELJSIJCTCLJUGFCEMLJYGBCEKLJQGRCDGMRRG4YUMOCBGM", totpSetup.ManualSetupKey);
            Assert.Equal(
                "otpauth://totp/DamirKusar?secret=G5DEMM2GGUZEELJSIJCTCLJUGFCEMLJYGBCEKLJQGRCDGMRRG4YUMOCBGM&issuer=Super Totp Tester",
                totpSetup.QrCodeImageContent);
        }
    }
}