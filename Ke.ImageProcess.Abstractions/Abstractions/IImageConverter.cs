
using Ke.ImageProcess.Models.Convert;

namespace Ke.ImageProcess.Abstractions;

/// <summary>
/// 图片格式转换接口
/// </summary>
public interface IImageConverter
{
    /// <summary>
    /// 转换完成事件
    /// </summary>
    event EventHandler<ConvertEventArgs>? OnConverted;
    /// <summary>
    /// 批量格式转换
    /// </summary>
    /// <param name="req">文件名后缀，为 null 时不添加后缀</param>
    /// <returns></returns>
    /// <exception cref="UnknowTargetFormatException"></exception>
    Task ConvertAsync(ImageConvertRequest req, CancellationToken cancellationToken = default);
}
