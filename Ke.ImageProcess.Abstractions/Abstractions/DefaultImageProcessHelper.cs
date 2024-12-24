using Ke.ImageProcess.Models;

namespace Ke.ImageProcess.Abstractions;

/// <summary>
/// 默认图片处理助手类
/// </summary>
public class DefaultImageProcessHelper : IImageProcessHelper
{
    /// <summary>
    /// 判断是否图片类型
    /// </summary>
    /// <param name="imageSource"></param>
    /// <returns></returns>
    public bool IsImage(string imageSource)
    {
        return ImageProcessConsts.AvailableImageFormats.Split(',').Any(x => imageSource.EndsWith(x, StringComparison.OrdinalIgnoreCase));
    }
}
