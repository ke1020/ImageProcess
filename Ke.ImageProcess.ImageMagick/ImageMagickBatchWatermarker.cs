using System.Text;

using ImageMagick;
using ImageMagick.Drawing;

using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.Models;
using Ke.ImageProcess.Models.Watermark;

namespace Ke.ImageProcess.ImageMagick;

public class ImageMagickBatchWatermarker : IImageWatermarker
{
    /// <summary>
    /// 水印位置映射
    /// </summary>
    private readonly IDictionary<WatermarkPosition, Gravity> _watermarkPositionGravityMaps = new Dictionary<WatermarkPosition, Gravity>
    {
        { WatermarkPosition.TopLeft, Gravity.Northwest  },
        { WatermarkPosition.TopCenter, Gravity.North  },
        { WatermarkPosition.TopRight, Gravity.Northeast },
        { WatermarkPosition.MiddleLeft, Gravity.West },
        { WatermarkPosition.Center, Gravity.Center },
        { WatermarkPosition.MiddleRight, Gravity.East },
        { WatermarkPosition.BottomLeft, Gravity.Southwest },
        { WatermarkPosition.BottomCenter, Gravity.South },
        { WatermarkPosition.BottomRight, Gravity.Southeast }
    };

    public event EventHandler<WatermarkEventArgs>? OnWatermarked;

    /// <summary>
    /// 将 RgbaColor 转为 MagickColor，输入对象为空时返回透明色
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static MagickColor FromRgba(RgbaColor? color)
    {
        return MagickColor.FromRgba(color?.Red ?? 0, color?.Green ?? 0, color?.Blue ?? 0, (byte)((float)(color?.Alpha ?? 0) / 100 * 255));
    }

    /// <summary>
    /// 获取文本水印
    /// </summary>
    /// <param name="textWatermark"></param>
    /// <returns></returns>
    private MagickImage GetTextWatermark<T>(ImageWatermarkRequest<T> req) where T : WatermarkBase
    {
        if (req.Watermark is not TextWatermark watermark)
        {
            throw new WatermarkNullException(nameof(TextWatermark));
        }

        // 创建MagickDrawables对象，用于绘制文本
        var drawables = new Drawables()
            // 指定编码
            .TextEncoding(Encoding.UTF8)
            // 抗锯齿
            .EnableTextAntialias()
            // 指定字体
            .Font(watermark.FontFamily ?? ImageProcessConsts.DefaultWatermarkFont)
            // 字体大小
            .FontPointSize(watermark.FontSize ?? ImageProcessConsts.DefaultWatermarkFontSize)
            .FillColor(FromRgba(watermark.TextColor)) // 设置字体颜色
            .Gravity(Gravity.Center) // 设置文本对齐方式
            ;

        // 计算文本的宽度和高度
        var textMetrics = drawables.FontTypeMetrics(watermark.Text);
        uint textWidth = (uint)textMetrics?.TextWidth!;
        uint textHeight = (uint)textMetrics.TextHeight;

        drawables = drawables.Text(0, 0, watermark.Text);
        // 边框颜色
        drawables = watermark.StrokeColor != null ? drawables.StrokeColor(FromRgba(watermark.StrokeColor)) : drawables;
        // 边框宽度
        drawables = watermark.StrokeWidth != null ? drawables.StrokeWidth(watermark.StrokeWidth ?? 0) : drawables;

        var image = new MagickImage(FromRgba(watermark.BackgroundColor), textWidth, textHeight);
        image.Draw(drawables);
        return image;
    }

    public async Task WatermarkAsync<T>(ImageWatermarkRequest<T> req, CancellationToken cancellationToken = default) where T : WatermarkBase
    {
        cancellationToken.ThrowIfCancellationRequested();
        // 获取输出格式
        var outputFormat = ImageMagickHelper.GetOutputFormat(req.OutputExtension, (int)req.Quality);
        // 
        int i = 0;
        // 加载水印图片
        using var watermark = req?.Mode switch
        {
            WatermarkMode.Text => GetTextWatermark(req),
            WatermarkMode.Image => new MagickImage((req.Watermark as ImageWatermark)?.FileName!, new MagickReadSettings
            {
                BackgroundColor = MagickColors.Transparent
            }),
            _ => throw new UnknowWatermarkModeException("WatermarkMode")
        };

        if (req.Rotation.HasValue)
        {
            watermark.Rotate(req.Rotation.Value);
        }

        if (req.Opacity.HasValue)
        {
            // 可选，设置水印透明度
            watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 1.0 / req.Opacity.Value);
        }

        // 遍历文件集合进行处理
        foreach (var file in req.ImageSources)
        {
            // 获取没有扩展名的文件名称
            var fileName = Path.GetFileNameWithoutExtension(file);
            // 结果输出路径
            var outputFile = Path.Combine(req?.OutputFilePath!, $"{fileName}{req?.Suffix ?? ""}.{req?.OutputExtension}");
            // 加载原图
            using var image = new MagickImage(file);

            if (req?.IsTile == true)
            {
                // 将 水印 图像以平铺的方式叠加到 image 上
                image.Tile(watermark, CompositeOperator.Over);
            }
            else
            {
                // 边距
                int offset = req?.Margin ?? 0;
                // 在指定位置绘制水印。CompositeOperator.Over 模式表示透明部分不会覆盖底层图像
                image.Composite(watermark,
                    _watermarkPositionGravityMaps[req?.Position ?? WatermarkPosition.Center],
                    offset, // offset X
                    offset, // offset Y
                    CompositeOperator.Over)
                    ;
            }
            // Save the result
            await image.WriteAsync(outputFile);

            OnWatermarked?.Invoke(this, new WatermarkEventArgs(i));
            i++;
        }
    }
}
