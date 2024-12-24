
using Ke.ImageProcess.Models;

namespace Ke.ImageProcess.Exceptions;

/// <summary>
/// 未知目标格式异常
/// </summary>
public class UnknowTargetFormatException : ImageProcessException
{
    public UnknowTargetFormatException() : base("未知目标格式")
    {
    }

    public UnknowTargetFormatException(string message)
        : base(message) { }

    public UnknowTargetFormatException(string message, Exception innerException)
        : base(message, innerException) { }
}
