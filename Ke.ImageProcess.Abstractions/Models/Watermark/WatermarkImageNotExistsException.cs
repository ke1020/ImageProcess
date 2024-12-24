
namespace Ke.ImageProcess.Models.Watermark;

public class WatermarkImageNotExistsException : ImageProcessException
{
    public WatermarkImageNotExistsException() : base("水印图片不存在")
    {
    }

    public WatermarkImageNotExistsException(string message)
        : base(message) { }

    public WatermarkImageNotExistsException(string message, Exception innerException)
        : base(message, innerException) { }
}
