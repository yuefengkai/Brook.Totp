using Brook.Totp;
using Brook.Totp.Interface;
using Xunit;

namespace Brook.Totp.Tests
{
    public class TotpValidatorTests
    {
        public TotpValidatorTests()
        {
            totpGenerator = new TotpGenerator();
            totpValidator = new TotpValidator(totpGenerator);
        }

        private readonly ITotpValidator totpValidator;
        private readonly ITotpGenerator totpGenerator;

        //[Fact]
        //public void Validate_TotpGeneratedByGoogleAuthenticatorIsValid_ManualTest()
        //{
        //    var valid = this.totpValidator.Validate("7FF3F52B-2BE1-41DF-80DE-04D32171F8A3", 086717);
        //    Assert.True(valid);
        //}

        [Fact]
        public void Validate_TotpGeneratedByGoogleAuthenticatorIsNotValid()
        {
            var valid = totpValidator.Validate("7FF3F52B-2BE1-41DF-80DE-04D32171F8A3", 284621);
            Assert.False(valid);
        }

        [Fact]
        public void Validate_TotpGeneratedByGoogleAuthenticatorIsValid()
        {
            var totp = totpGenerator.Generate("7FF3F52B-2BE1-41DF-80DE-04D32171F8A3");
            var valid = totpValidator.Validate("7FF3F52B-2BE1-41DF-80DE-04D32171F8A3", totp, 0);
            Assert.True(valid);
        }
    }
}