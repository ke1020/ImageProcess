

namespace Ke.ImageProcess.Models;

/// <summary>
/// 图片处理异常基类
/// </summary>
public class ImageProcessException : Exception
{
    public ImageProcessException() { }

    public ImageProcessException(string message)
        : base(message) { }

    public ImageProcessException(string message, Exception innerException)
        : base(message, innerException) { }
}
