
namespace Ke.ImageProcess.Models.Watermark;

/// <summary>
/// 水印对象为 NULL
/// </summary>
public class WatermarkNullException : ImageProcessException
{
    public WatermarkNullException() : base("水印对象为 NULL")
    {
    }

    public WatermarkNullException(string message)
        : base(message) { }

    public WatermarkNullException(string message, Exception innerException)
        : base(message, innerException) { }
}
