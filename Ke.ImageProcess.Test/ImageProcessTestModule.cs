using Ke.ImageProcess.Abstractions;
//using Ke.ImageProcess.ImageMagick;
using Ke.ImageProcess.ImageSharp;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Ke.ImageProcess.Test;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpCachingStackExchangeRedisModule)
)]
public class ImageProcessTestModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () =>
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var serv = context.Services;

        /*
        Configure<ImageProcessOptions>(opts =>
        {
            new ConfigurationBuilder().AddJsonFile("Configs/image-process.json").Build().Bind(opts);
        });
        */

        //serv.AddSingleton<IImageProcessor<IImageEncoder>, ImageSharpProcessor>();
        serv.AddSingleton<IImageScaler, ImageSharpScaler>();
        serv.AddSingleton<IImageConverter, ImageSharpConverter>();
        serv.AddSingleton<IImageWatermarker, ImageSharpWatermarker>();
        serv.AddSingleton<IImageProcessHelper, DefaultImageProcessHelper>();
        //serv.AddKeyedSingleton<IBatchScaler, ImageMagickBatchScaler>(ImageProcessTestConsts.ImageMagickKeyed);
        //serv.AddKeyedSingleton<IBatchConverter, ImageMagickBatchConverter>(ImageProcessTestConsts.ImageMagickKeyed);
        //serv.AddKeyedSingleton<IBatchWatermarker, ImageMagickBatchWatermarker>(ImageProcessTestConsts.ImageMagickKeyed);
    }
}
