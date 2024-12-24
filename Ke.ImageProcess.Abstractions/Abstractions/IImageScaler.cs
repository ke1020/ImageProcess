using Ke.ImageProcess.Models.Scale;

namespace Ke.ImageProcess.Abstractions;

/// <summary>
/// 图片缩放接口
/// </summary>
public interface IImageScaler
{
    /// <summary>
    /// 缩放完成事件
    /// </summary>
    event EventHandler<ScaleEventArgs>? OnScaled;
    /// <summary>
    /// 缩放
    /// </summary>
    /// <param name="req">缩放请求对象</param>
    /// <returns></returns>
    Task ScaleAsync(ImageScaleRequest req);
    /// <summary>
    /// 根据源图缩放并返回 PNG 格式的内存流
    /// </summary>
    /// <param name="imageSource">源图</param>
    /// <param name="width">目标宽度</param>
    /// <param name="height">目标高度</param>
    /// <returns></returns>
    Task<Stream?> GetScaleStreamAsync(string imageSource, uint? width, uint? height);
}
