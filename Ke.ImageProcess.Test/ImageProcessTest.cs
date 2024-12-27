using System.Text.Json;

using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.Models;
using Ke.ImageProcess.Models.Convert;
using Ke.ImageProcess.Models.Scale;
using Ke.ImageProcess.Models.Watermark;
using Xunit;

namespace Ke.ImageProcess.Test;

public class ImageProcessTest : TestBase<ImageProcessTestModule>
{
    //private readonly string inputPath = @"D:\0Project\0基础库\src\ImageProcess\Ke.ImageProcess.Test\files\371626104";
    private readonly string outputPath = @"D:\0Project\0基础库\src\ImageProcess\Ke.ImageProcess.Test\files\371626105";
    //private readonly IImageProcessor _imageProcessor;
    private readonly IImageScaler _imageSharpScaler;
    private readonly IImageConverter _imageSharpConverter;
    private readonly IImageWatermarker _imageSharpWatermarker;
    /*
    private readonly IImageScaler _imageMagickScaler;
    private readonly IImageConverter _imageMagickConverter;
    private readonly IImageWatermarker _imageMagickWatermarker;
    */

    public ImageProcessTest()
    {
        _imageSharpScaler = GetRequiredService<IImageScaler>();
        _imageSharpConverter = GetRequiredService<IImageConverter>();
        _imageSharpWatermarker = GetRequiredService<IImageWatermarker>();
        //_imageSharpProcessor = GetRequiredService<IImageProcessor>();

        //_imageMagickScaler = GetRequiredKeyedService<IImageScaler>(ImageProcessTestConsts.ImageMagickKeyed);
        //_imageMagickConverter = GetRequiredKeyedService<IImageConverter>(ImageProcessTestConsts.ImageMagickKeyed);
        //_imageMagickWatermarker = GetRequiredKeyedService<IImageWatermarker>(ImageProcessTestConsts.ImageMagickKeyed);
    }

    [Fact]
    public async Task ImageSharpTest()
    {
        /*
        var images = _imageSharpProcessor.GetImages(
            [
                inputPath,
                @"D:\2images\后端\abp.png"
            ],
            [".jpg", ".png", ".bmp", ".webp", ".gif"])
            ;
        */
        var images = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "files"));

        // 批量缩放
        await _imageSharpScaler.ScaleAsync(new ImageScaleRequest(images, outputPath, "png")
        {
            ScaleMode = ScaleMode.ResizeAndCrop,
            Width = 1440,
            Height = 900,
            Quality = 80,
            //Suffix = null
        });

        // 批量转换格式
        await _imageSharpConverter.ConvertAsync(new ImageConvertRequest(images, outputPath, "webp")
        {
            Quality = 80,
            //Suffix = null
        });

        // 图片水印
        await _imageSharpWatermarker.WatermarkAsync(new ImageWatermarkRequest<ImageWatermark>(images, outputPath, "png")
        {
            Mode = WatermarkMode.Image,
            Watermark = new ImageWatermark(@"D:\backup\抠丁客\logo108.png"),
            Position = WatermarkPosition.BottomRight,
            Opacity = .4,
            Suffix = "-wi",
            //Rotation = 45,
            Margin = 0,
            // IsTile = true
        });


        // 文本水印
        await _imageSharpWatermarker.WatermarkAsync(new ImageWatermarkRequest<TextWatermark>(images, outputPath, "png")
        {
            Mode = WatermarkMode.Text,
            Watermark = new TextWatermark("授权后去除水印")
            {
                // ImageSharp 可以写安装到系统的字体名称（与 ImageMagick 不同）
                FontFamily = "HarmonyOS Sans SC",
                FontSize = 25,
                TextColor = new RgbaColor(200, 200, 200, 50),
                //StrokeColor = new RgbaColor(255, 255, 255, 50),
                StrokeWidth = 2,
                // BackgroundColor = new RgbaColor(0, 0, 0, 0),
            },
            // IsTite 为 true 时该项无效
            Position = WatermarkPosition.BottomRight,
            Opacity = .4,
            Suffix = "-wt",
            Rotation = 45,
            IsTile = true
        });
    }

    /*
    //[Fact]
    public async Task ImageMagickTest()
    {
        // 批量缩放
        await _imageMagickScaler.BatchScaleAsync(new ImageScaleRequest(inputPath, outputPath, "png")
        {
            ScaleMode = ScaleMode.ResizeAndCrop,
            Width = 1440,
            Height = 900,
            Quality = 80,
            //Suffix = null
        });

        // 批量转换格式
        await _imageMagickConverter.BatchConvertAsync(new ImageConvertRequest(inputPath, outputPath, "webp")
        {
            Quality = 80,
            //Suffix = null
        });

        // 文本水印
        await _imageMagickWatermarker.BatchWatermarkAsync(new ImageWatermarkRequest<TextWatermark>(inputPath, outputPath, "png")
        {
            Mode = WatermarkMode.Text,
            Watermark = new TextWatermark("授权后去除水印")
            {
                FontFamily = ImageProcessConsts.DefaultWatermarkFont,
                FontSize = 25,
                TextColor = new RgbaColor(200, 200, 200, 50),
                //StrokeColor = new RgbaColor(255, 255, 255, 50),
                StrokeWidth = 2,
                // BackgroundColor = new RgbaColor(0, 0, 0, 0),
            },
            // IsTite 为 true 时该项无效
            Position = WatermarkPosition.BottomRight,
            Opacity = .95,
            Suffix = "-wt",
            Rotation = 45,
            IsTile = true
        });

        // 图片水印
        await _imageMagickWatermarker.BatchWatermarkAsync(new ImageWatermarkRequest<ImageWatermark>(inputPath, outputPath, "png")
        {
            Mode = WatermarkMode.Image,
            Watermark = new ImageWatermark(@"D:\backup\抠丁客\logo108.png"),
            Position = WatermarkPosition.BottomRight,
            Suffix = "-wi",
            Opacity = .95,
            Rotation = 45,
            Margin = -10
        });
    }
    */
}
