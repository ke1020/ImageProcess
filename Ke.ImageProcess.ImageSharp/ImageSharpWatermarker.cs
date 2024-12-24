using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.Models;
using Ke.ImageProcess.Models.Watermark;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Ke.ImageProcess.ImageSharp;

public class ImageSharpWatermarker(IImageProcessHelper imageProcessHelper) : IImageWatermarker
{
    private readonly IImageProcessHelper _imageProcessHelper = imageProcessHelper;
    /// <summary>
    /// 添加水印之后
    /// </summary>
    public event EventHandler<WatermarkEventArgs>? OnWatermarked;

    /// <summary>
    /// 批量添加水印
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task WatermarkAsync<T>(ImageWatermarkRequest<T> req) where T : WatermarkBase
    {
        // 获取输出格式
        var format = ImageSharpHelper.GetOutputFormat(req.OutputExtension, (int)req.Quality);

        using var watermarkImage = GetWatermarkImage(req);
        if (watermarkImage is null)
        {
            return;
        }

        // 获取到旋转后图像的大小
        var rotatedSize = watermarkImage.Size;

        int i = 0;
        // 遍历文件集合进行处理
        foreach (var file in req.ImageSources)
        {
            if (!_imageProcessHelper.IsImage(file))
            {
                continue;
            }

            // 获取没有扩展名的文件名称
            var fileName = Path.GetFileNameWithoutExtension(file);
            // 输出路径
            var outputFile = Path.Combine(req.OutputFilePath, $"{fileName}{req.Suffix ?? ""}.{req.OutputExtension}");
            // 加载原图
            using var image = Image.Load(file);

            if (req.IsTile == true)
            {
                // 计算平铺的位置
                for (int y = 0; y < image.Height; y += rotatedSize.Height)
                {
                    for (int x = 0; x < image.Width; x += rotatedSize.Width)
                    {
                        image.Mutate(ctx => ctx.DrawImage(watermarkImage!, new Point(x, y), (float)(req.Opacity ?? 1)));
                    }
                }
            }
            else
            {
                // 指定绘制位置
                var twp = GetWatermarkPosition(image, req, rotatedSize.Width, rotatedSize.Height);
                // 绘制水印
                image.Mutate(ctx => ctx.DrawImage(watermarkImage!, new Point(twp.Item1, twp.Item2), (float)(req.Opacity ?? 1)));
            }

            // 保存图片
            await image.SaveAsync(outputFile, format);

            OnWatermarked?.Invoke(file, new WatermarkEventArgs(i));

            i++;
        }

    }

    private Image<Rgba32>? GetWatermarkImage<T>(ImageWatermarkRequest<T> req) where T : WatermarkBase
    {
        Image<Rgba32>? watermarkImage = null; // 存储绘制的水印图片
        float rotation = (float)(req.Rotation ?? 0); // 旋转角度
        switch (req.Mode)
        {
            case WatermarkMode.Text:
                if (req.Watermark is not TextWatermark textWatermark)
                {
                    throw new WatermarkNullException(nameof(TextWatermark));
                }

                // 创建字体
                var font = SystemFonts.CreateFont(
                    textWatermark.FontFamily ?? ImageProcessConsts.DefaultWatermarkFont,
                    textWatermark?.FontSize ?? ImageProcessConsts.DefaultWatermarkFontSize
                    )
                    ;
                var textOptions = new TextOptions(font);
                var text = textWatermark?.Text ?? string.Empty;
                // 计算文本大小
                var textSize = TextMeasurer.MeasureSize(text, textOptions);
                int textWidth = (int)textSize.Width;
                int textHeight = (int)textSize.Height;

                var drawingOptions = new DrawingOptions();

                // 将文本绘制为一张水印图片
                watermarkImage = new Image<Rgba32>(textWidth, textHeight);
                // 绘制
                watermarkImage.Mutate(x => x.BackgroundColor(FromRgba(textWatermark?.BackgroundColor))
                    .DrawText(text, font, FromRgba(textWatermark?.TextColor), new Point(0, 0))
                    .Rotate(rotation))
                    ;
                break;
            case WatermarkMode.Image:
                if (req.Watermark is not ImageWatermark watermarkData)
                {
                    throw new WatermarkNullException(nameof(ImageWatermark));
                }

                if (!File.Exists(watermarkData.FileName))
                {
                    throw new WatermarkImageNotExistsException(nameof(watermarkData.FileName));
                }

                // 从图片加载
                watermarkImage = Image.Load<Rgba32>(watermarkData.FileName);
                // 旋转
                watermarkImage.Mutate(x => x.Rotate(rotation));
                break;
        }

        return watermarkImage;
    }

    /// <summary>
    /// 将 RgbaColor 转为 Color，输入对象为空时返回透明色
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static Color FromRgba(RgbaColor? color)
    {
        return Color.FromRgba(color?.Red ?? 0, color?.Green ?? 0, color?.Blue ?? 0, (byte)((float)(color?.Alpha ?? 0) / 100 * 255));
    }

    private Tuple<int, int> GetWatermarkPosition<T>(Image image, ImageWatermarkRequest<T> req, int wWidth, int wHeight) where T : WatermarkBase
    {
        // 设置水印位置
        WatermarkPosition position = req?.Position ?? WatermarkPosition.Center;

        // 计算水印位置
        int x, y;
        int margin = req?.Margin ?? ImageProcessConsts.DefaultWatermarkMargin; // 边距
        switch (position)
        {
            case WatermarkPosition.TopLeft:
                x = margin;
                y = margin;
                break;
            case WatermarkPosition.TopCenter:
                x = (image.Width - wWidth) / 2;
                y = margin;
                break;
            case WatermarkPosition.TopRight:
                x = image.Width - wWidth - margin;
                y = margin;
                break;
            case WatermarkPosition.MiddleLeft:
                x = margin;
                y = (image.Height - wHeight) / 2;
                break;
            case WatermarkPosition.Center:
                x = (image.Width - wWidth) / 2;
                y = (image.Height - wHeight) / 2;
                break;
            case WatermarkPosition.MiddleRight:
                x = image.Width - wWidth - margin;
                y = (image.Height - wHeight) / 2;
                break;
            case WatermarkPosition.BottomLeft:
                x = margin;
                y = image.Height - wHeight - margin;
                break;
            case WatermarkPosition.BottomCenter:
                x = (image.Width - wWidth) / 2;
                y = image.Height - wHeight - margin;
                break;
            case WatermarkPosition.BottomRight:
                x = image.Width - wWidth - margin;
                y = image.Height - wHeight - margin;
                break;
            default:
                x = margin;
                y = margin;
                break;
        }

        return new Tuple<int, int>(x, y);
    }
}
