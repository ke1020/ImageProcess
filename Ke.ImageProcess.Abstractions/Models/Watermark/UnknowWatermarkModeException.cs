
namespace Ke.ImageProcess.Models.Watermark;

/// <summary>
/// 未知水印模式异常
/// </summary>
public class UnknowWatermarkModeException : ImageProcessException
{
    public UnknowWatermarkModeException() : base("未知水印模式")
    {
    }

    public UnknowWatermarkModeException(string message)
        : base(message) { }

    public UnknowWatermarkModeException(string message, Exception innerException)
        : base(message, innerException) { }
}
