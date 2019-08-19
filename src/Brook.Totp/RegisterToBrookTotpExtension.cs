using Brook.Totp.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Brook.Totp
{
    public static class RegisterToBrookTotpExtension
    {
        /// <summary>
        /// 添加2FA认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddBrookTotp(this IServiceCollection services)
        {
            services.AddSingleton<ITotpSetupGenerator, TotpSetupGenerator>();
            services.AddSingleton<ITotpValidator, TotpValidator>();
            services.AddSingleton<ITotpGenerator, TotpGenerator>();
            services.AddSingleton<ITotp, Totp>();

            return services;
        }

    }
}
