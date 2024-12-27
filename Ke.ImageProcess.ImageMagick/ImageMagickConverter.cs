
using ImageMagick;

using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.Models.Convert;

namespace Ke.ImageProcess.ImageMagick;

public class ImageMagickConverter : IImageConverter
{
    public event EventHandler<ConvertEventArgs>? OnConverted;

    public async Task ConvertAsync(ImageConvertRequest req, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // 获取输出格式
        var outputFormat = ImageMagickHelper.GetOutputFormat(req.OutputExtension, (int)req.Quality);
        // 
        int i = 0;
        // 遍历文件集合进行处理
        foreach (var file in req.ImageSources)
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

            OnConverted?.Invoke(this, new ConvertEventArgs(i));
            i++;
        }
    }
}
