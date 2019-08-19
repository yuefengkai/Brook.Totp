using System.Collections.Generic;
using Brook.Totp;
using Brook.Totp.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Brook.Totp.WebApi2FA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITotp _totp;
        public ValuesController(ITotp totp)
        {
            _totp = totp;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        // DELETE api/values/5

        [HttpGet]
        [Route("qr")]
        public ActionResult<IEnumerable<string>> Qr()
        {
            var totpSetup = _totp.GenerateBase64("gzz-title", "gzz@gzz.cn", "secret");

            return new[] {totpSetup.ManualSetupKey, totpSetup.QrCodeImageBase64};
        }

        [HttpGet]
        [Route("valid")]
        public ActionResult<IEnumerable<string>> Valid(int code)
        {
            var valid = _totp.Validate("secret", code);

            return new[] {valid.ToString()};
        }
    }
}