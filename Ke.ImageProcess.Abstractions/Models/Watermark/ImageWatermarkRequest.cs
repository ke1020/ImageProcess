
namespace Ke.ImageProcess.Models.Watermark;

public class ImageWatermarkRequest<T> : ImageProcessRequestBase
{
    /// <summary>
    /// 水印模式
    /// </summary>
    public WatermarkMode Mode { get; set; } = WatermarkMode.Text;
    /// <summary>
    /// 水印对象
    /// </summary>
    public T? Watermark { get; set; }
    /// <summary>
    /// 是否平铺
    /// </summary>
    public bool? IsTile { get; set; }
    /// <summary>
    /// 水印位置
    /// </summary>
    public WatermarkPosition Position { get; set; }
    /// <summary>
    /// 不透明度 0-1 之间
    /// </summary>
    public double? Opacity { get; set; }
    /// <summary>
    /// 水印边距 (可以为负值)
    /// </summary>
    public int Margin { get; set; } = 50;
    /// <summary>
    /// 旋转角度
    /// </summary>
    public double? Rotation { get; set; }

    public ImageWatermarkRequest(ICollection<string> imageSources, string outputFilePath, string outputExtension) :
        base(imageSources, outputFilePath, outputExtension)
    {
        Suffix = "-w";
    }
}
