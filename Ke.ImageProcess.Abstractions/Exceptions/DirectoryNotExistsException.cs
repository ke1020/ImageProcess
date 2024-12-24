
using Ke.ImageProcess.Models;

namespace Ke.ImageProcess.Exceptions;

/// <summary>
/// 目录不存在异常
/// </summary>
public class DirectoryNotExistsException : ImageProcessException
{
    public DirectoryNotExistsException() : base("目录不存在")
    {
    }

    public DirectoryNotExistsException(string message)
        : base(message) { }

    public DirectoryNotExistsException(string message, Exception innerException)
        : base(message, innerException) { }
}
