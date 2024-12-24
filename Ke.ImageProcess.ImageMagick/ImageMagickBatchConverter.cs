using ImageMagick;
using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.Exceptions;
using Ke.ImageProcess.Models.Convert;

namespace Ke.ImageProcess.ImageMagick;

public class ImageMagickBatchConverter : IBatchConverter
{
    /// <summary>
    /// 批量图片格式转换
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task BatchConvertAsync(ImageConvertRequest req)
    {
        // 获取要处理的文件集合
        var files = ImageProcessHelper.GetFiles(req.InputFilePath, req.SearchExtensions);
        // 获取输出格式
        var outputFormat = ImageProcessHelper.GetOutputFormat(req.OutputExtension);
        // 遍历文件集合进行处理
        foreach (var file in files)
        {
            // 获取没有扩展名的文件名称
            var fileName = Path.GetFileNameWithoutExtension(file);
            // 输出路径
            var outputFile = Path.Combine(req.OutputFilePath, $"{fileName}{req.Suffix ?? ""}.{req.OutputExtension}");
            // 初始化处理对象
            using var image = new MagickImage(file);
            // 指定输出格式
            image.Format = outputFormat;
            // 输出质量
            image.Quality = req.Quality;
            // 写入文件
            await image.WriteAsync(outputFile);
        }
    }
}
