using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Qoi;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;

public static class ImageSharpHelper
{
    /// <summary>
    /// 从根据输出扩展获取输出格式
    /// </summary>
    /// <returns></returns>
    public static IImageEncoder GetOutputFormat(string? outputExtensions, int quality = 90)
    {
        var extensions = outputExtensions?.StartsWith('.') == true ? outputExtensions[1..] : outputExtensions;
        return extensions?.ToLower() switch
        {
            "png" => new PngEncoder(),
            //"jpg" => new JpegEncoder(),
            "gif" => new GifEncoder(),
            "bmp" => new BmpEncoder(),
            "pbm" => new PbmEncoder(),
            "tga" => new TgaEncoder(),
            "tiff" => new TiffEncoder(),
            "webp" => new WebpEncoder()
            {
                Quality = quality
            },
            "qoi" => new QoiEncoder(),
            _ => new JpegEncoder()
            {
                Quality = quality
            }
        };
    }
}