
namespace Ke.ImageProcess.Models;

/// <summary>
/// 图片处理事件参数
/// </summary>
public class ImageProcessEventArgs
{
    public ImageProcessEventArgs(int index)
    {
        Index = index;
    }

    /// <summary>
    /// 当前处理项的索引
    /// </summary>
    public int Index { get; set; }
}
