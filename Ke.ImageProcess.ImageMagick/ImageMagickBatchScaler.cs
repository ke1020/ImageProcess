
using ImageMagick;
using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.Models;
using Ke.ImageProcess.Models.Scale;

namespace Ke.ImageProcess.ImageMagick;

public class ImageMagickBatchScaler : IBatchScaler
{
    /// <summary>
    /// 缩放处理器字典
    /// </summary>
    private readonly IDictionary<ScaleMode, Func<ImageScaleRequest, string, string, Task>> _scaleProcessors = new Dictionary<ScaleMode, Func<ImageScaleRequest, string, string, Task>>
    {
        {
            // 等比缩放
            ScaleMode.EqualRatio, async (req, inputFile, outputFile) =>
            {
                // 加载图像
                using var image = new MagickImage(inputFile);
                // 质量
                image.Quality = req.Quality;
                // MagickGeometry 构造函数的第一个参数是宽度，第二个参数是高度。如果只指定宽度，高度设为 0，会自动计算保持比例的高度。
                image.Resize(new MagickGeometry(req.Width ?? 0,req.Height ?? 0));
                // 执行输出
                await image.WriteAsync(outputFile);
            }
        },
        {
            ScaleMode.ResizeAndCrop, async (req, inputFile, outputFile) =>
            {
                using var image = new MagickImage(inputFile);
                // 质量
                image.Quality = req.Quality;

                // 获取原始图像的宽度和高度
                uint originalWidth = image.Width;
                uint originalHeight = image.Height;
                uint targetWidth = req.Width ?? ImageProcessConsts.DefaultWidth;
                uint targetHeight = req.Height ?? ImageProcessConsts.DefaultHeight;

                // 计算放大比例，确保图像至少达到目标尺寸
                double scaleX = (double)targetWidth / originalWidth;
                double scaleY = (double)targetHeight / originalHeight;
                double scale = Math.Max(scaleX, scaleY); // 选择最大比例进行放大

                // 如果需要放大，则按比例缩放图像
                if (scale > 1)
                {
                    image.Resize((uint)(originalWidth * scale), (uint)(originalHeight * scale));
                }

                // 计算裁剪区域的起始位置，居中裁剪
                uint cropX = (image.Width - targetWidth) / 2;
                uint cropY = (image.Height - targetHeight) / 2;

                // 执行裁剪
                image.Crop(new MagickGeometry((int)cropX, (int)cropY, targetWidth, targetHeight));

                // 保存裁剪后的图像
                await image.WriteAsync(outputFile);
            }
        }
    };

    public async Task BatchScaleAsync(ImageScaleRequest req)
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
            // 从处理器集合获取对应处理方法
            if (_scaleProcessors.TryGetValue(req.ScaleMode, out var processor))
            {
                try
                {
                    await processor(req, file, outputFile);
                }
                catch (MagickException e)
                {
                    throw new ImageProcessException(e.Message);
                }
            }
        }
    }
}
