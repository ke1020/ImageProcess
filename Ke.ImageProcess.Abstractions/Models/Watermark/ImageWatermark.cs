
namespace Ke.ImageProcess.Models.Watermark;

/// <summary>
/// 图片水印
/// </summary>
/// <param name="fileName">水印图片未知</param>
public class ImageWatermark(string fileName) : WatermarkBase
{
    /// <summary>
    /// 水印图片地址
    /// </summary>
    public string FileName { get; set; } = fileName;
}
