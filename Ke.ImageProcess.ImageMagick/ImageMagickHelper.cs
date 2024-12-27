
using ImageMagick;
using Ke.ImageProcess.Exceptions;

namespace Ke.ImageProcess.ImageMagick;

internal static class ImageMagickHelper
{
    /// <summary>
    /// 从根据输出扩展获取输出格式
    /// </summary>
    /// <returns></returns>
    public static MagickFormat GetOutputFormat(string? outputExtension, int quality = 90)
    {
        // 判断输出扩展是否受支持
        if (!Enum.TryParse<MagickFormat>(outputExtension, true, out var target))
        {
            throw new UnknowTargetFormatException(nameof(outputExtension));
        }

        return target;
    }
}
