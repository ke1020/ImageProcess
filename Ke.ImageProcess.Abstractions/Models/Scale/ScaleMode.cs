
namespace Ke.ImageProcess.Models.Scale;

/// <summary>
/// 缩放模式枚举
/// </summary>
public enum ScaleMode
{
    None = 0,
    /// <summary>
    /// 等比缩放
    /// </summary>
    EqualRatio,
    /// <summary>
    /// 居中裁剪 (如果原图小于目标大小，则先放大)
    /// </summary>
    ResizeAndCrop,
}
