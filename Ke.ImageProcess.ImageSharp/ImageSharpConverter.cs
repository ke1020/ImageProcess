using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.Models.Convert;

using SixLabors.ImageSharp;

namespace Ke.ImageProcess.ImageSharp;

public class ImageSharpConverter(IImageProcessHelper imageProcessHelper) : IImageConverter
{
    private readonly IImageProcessHelper _imageProcessHelper = imageProcessHelper;
    /// <summary>
    /// 转换完成之后
    /// </summary>
    public event EventHandler<ConvertEventArgs>? OnConverted;

    /// <summary>
    /// 批量图片格式转换
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task ConvertAsync(ImageConvertRequest req, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // 获取输出格式
        var format = ImageSharpHelper.GetOutputFormat(req.OutputExtension, (int)req.Quality);
        int i = 0;
        // 遍历文件集合进行处理
        foreach (var file in req.ImageSources)
        {
            if (!_imageProcessHelper.IsImage(file))
            {
                continue;
            }

            // 获取没有扩展名的文件名称
            var fileName = Path.GetFileNameWithoutExtension(file);
            // 输出路径
            var outputFile = Path.Combine(req.OutputFilePath, $"{fileName}{req.Suffix ?? ""}.{req.OutputExtension}");
            // 载入图像
            using var image = Image.Load(file);
            // 保存
            await image.SaveAsync(outputFile, format, cancellationToken);

            OnConverted?.Invoke(this, new ConvertEventArgs(i));
            i++;
        }
    }
}
