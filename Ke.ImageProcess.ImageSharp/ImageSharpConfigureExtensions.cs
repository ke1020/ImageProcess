
using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.ImageSharp;
using Microsoft.Extensions.DependencyInjection;

public static class ImageSharpConfigureExtensions
{
    /// <summary>
    /// 添加 ImageSharp 图片处理服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddImageSharp(this IServiceCollection services)
    {
        services.AddSingleton<IImageProcessHelper, DefaultImageProcessHelper>();
        services.AddTransient<IImageConverter, ImageSharpConverter>();
        services.AddTransient<IImageWatermarker, ImageSharpWatermarker>();
        services.AddTransient<IImageScaler, ImageSharpScaler>();
        services.AddTransient<IImageProcessor, ImageSharpProcessor>();
        return services;
    }
}