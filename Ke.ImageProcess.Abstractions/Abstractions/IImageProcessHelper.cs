
namespace Ke.ImageProcess.Abstractions;

/// <summary>
/// 相关助手接口
/// </summary>
public interface IImageProcessHelper
{
    /// <summary>
    /// 判断输入文件，是可以处理的图片类型
    /// </summary>
    bool IsImage(string imageSource);
}