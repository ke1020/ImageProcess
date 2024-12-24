
using Ke.ImageProcess.Exceptions;

namespace Ke.ImageProcess.Models;

public class ImageProcessRequestBase
{
    /// <summary>
    /// 图片地址集合
    /// </summary>
    public ICollection<string> ImageSources { get; set; }
    /// <summary>
    /// 输出文件目录
    /// </summary>
    public string OutputFilePath { get; set; } = null!;
    /// <summary>
    /// 输出扩展
    /// </summary>
    public string OutputExtension { get; set; } = ".jpg";
    /// <summary>
    /// 输出质量
    /// </summary>
    public uint Quality { get; set; } = ImageProcessConsts.DefaultQuality;
    /// <summary>
    /// 输出文件名后缀。示例：prod-w.jpg 中的 -w
    /// </summary>
    public string? Suffix { get; set; }


    public ImageProcessRequestBase(ICollection<string> imageSources, string outputFilePath, string outputExtension)
    {
        ArgumentNullException.ThrowIfNull(outputFilePath);

        if (string.IsNullOrWhiteSpace(outputExtension))
        {
            throw new UnknowTargetFormatException(nameof(OutputExtension));
        }

        if (!Directory.Exists(outputFilePath))
        {
            Directory.CreateDirectory(outputFilePath);
        }

        ImageSources = imageSources ?? [];
        OutputFilePath = outputFilePath;
        OutputExtension = outputExtension;
    }
}
