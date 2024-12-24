
namespace Ke.ImageProcess.Models.Scale;

public class ImageScaleRequest : ImageProcessRequestBase
{
    /// <summary>
    /// 缩放模式
    /// </summary>
    public ScaleMode ScaleMode { get; set; } = ScaleMode.EqualRatio;
    /// <summary>
    /// 宽度
    /// </summary>
    public uint? Width { get; set; }
    /// <summary>
    /// 高度
    /// </summary>
    public uint? Height { get; set; }

    public ImageScaleRequest(ICollection<string> imageSources, string outputFilePath, string outputExtension) :
        base(imageSources, outputFilePath, outputExtension)
    {
        Suffix = "-s";
    }
}
