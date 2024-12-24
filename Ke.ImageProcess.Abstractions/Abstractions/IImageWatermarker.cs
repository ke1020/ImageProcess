
using Ke.ImageProcess.Models.Watermark;

namespace Ke.ImageProcess.Abstractions;

/// <summary>
/// 图片水印接口
/// </summary>
public interface IImageWatermarker
{
    /// <summary>
    /// 添加水印完成事件
    /// </summary>
    event EventHandler<WatermarkEventArgs>? OnWatermarked;
    /// <summary>
    /// 添加水印
    /// </summary>
    /// <param name="req">添加水印请求对象</param>
    /// <returns></returns>
    Task WatermarkAsync<T>(ImageWatermarkRequest<T> req) where T : WatermarkBase;
}
