
using ImageMagick;
using Ke.ImageProcess.Exceptions;

namespace Ke.ImageProcess.ImageMagick;

internal static class ImageProcessHelper
{
    /// <summary>
    /// 从配置获取要处理的文件集合
    /// </summary>
    /// <param name="inputPath">输入目录</param>
    /// <param name="searchExtensions">要查找的扩展名数组</param>
    /// <returns></returns>
    public static IEnumerable<string> GetFiles(string inputPath, string[] searchExtensions)
    {
        // 查找 InputFilePath 目录及子目录下所有指定 InputExtensions 扩展名的文件
        var files = Directory.GetFiles(inputPath, "*.*", SearchOption.AllDirectories)
            .Where(file => searchExtensions?.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) == true)
            ;

        if (!(files?.Any() == true))
        {
            return [];
        }

        return files;
    }

    /// <summary>
    /// 从配置获取输出格式
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UnknowTargetFormatException"></exception>
    public static MagickFormat GetOutputFormat(string outputExtension)
    {
        // 判断输出扩展是否受支持
        if (!Enum.TryParse<MagickFormat>(outputExtension, true, out var target))
        {
            throw new UnknowTargetFormatException(nameof(outputExtension));
        }

        return target;
    }
}
