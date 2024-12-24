
namespace Ke.ImageProcess.Models;

public class ImageProcessConsts
{
    /// <summary>
    /// 默认输出质量
    /// </summary>
    public const int DefaultQuality = 90;
    /// <summary>
    /// 没有指定宽度时的默认值
    /// </summary>
    public const int DefaultWidth = 400;
    /// <summary>
    /// 没有指定高度时的默认值
    /// </summary>
    public const int DefaultHeight = 200;
    /// <summary>
    /// 默认水印边距
    /// </summary>
    public const int DefaultWatermarkMargin = 50;
    /// <summary>
    /// 默认水印字体
    /// </summary>
    public const string DefaultWatermarkFont = "Microsoft YaHei & Microsoft YaHei UI";
    /// <summary>
    /// 默认水印字体大小
    /// </summary>
    public const int DefaultWatermarkFontSize = 40;
    /// <summary>
    /// 可处理的图片类型
    /// </summary>
    public const string AvailableImageFormats = ".jpg,.png,.bmp,.webp,.gif";
}
