
using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.Models;
using Ke.ImageProcess.Models.Scale;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Ke.ImageProcess.ImageSharp;

public class ImageSharpScaler(IImageProcessHelper imageProcessHelper) : IImageScaler
{
    private readonly IImageProcessHelper _imageProcessHelper = imageProcessHelper;
    /// <summary>
    /// 缩放完成之后
    /// </summary>
    public event EventHandler<ScaleEventArgs>? OnScaled;

    /// <summary>
    /// 缩放处理器字典
    /// </summary>
    private readonly IDictionary<ScaleMode, Action<Image, uint?, uint?>> _scaleProcessors = new Dictionary<ScaleMode, Action<Image, uint?, uint?>>
    {
        {
            ScaleMode.EqualRatio, (image, w, h) =>
            {
                int targetWidth = 0;
                int targetHeight = 0;

                // 计算新的高度或宽度
                if (w.HasValue)
                {
                    targetHeight = (int)(w.Value * image.Height / image.Width);
                }
                else if (h.HasValue)
                {
                    targetWidth = (int)(h.Value * image.Width / image.Height);
                }

                // 缩放图片
                image.Mutate(x => x.Resize(targetWidth, targetHeight));
            }
        },
        {
            ScaleMode.ResizeAndCrop, (image, w, h) =>
            {
                // 目标宽高
                int targetWidth = (int)(w ?? ImageProcessConsts.DefaultWidth);
                int targetHeight = (int)(h ?? ImageProcessConsts.DefaultHeight);

                int originalWidth = image.Width;
                int originalHeight = image.Height;

                // 计算缩放比例
                double scaleX = (double)targetWidth / originalWidth;
                double scaleY = (double)targetHeight / originalHeight;
                double scale = Math.Max(scaleX, scaleY);

                // 计算缩放后的尺寸
                int scaledWidth = (int)(originalWidth * scale);
                int scaledHeight = (int)(originalHeight * scale);

                // 计算裁剪区域
                int cropX = (scaledWidth - targetWidth) / 2;
                int cropY = (scaledHeight - targetHeight) / 2;

                // 确保裁剪区域不会超出缩放后的图像边界
                cropX = Math.Max(0, cropX);
                cropY = Math.Max(0, cropY);
                targetWidth = Math.Min(targetWidth, scaledWidth - cropX);
                targetHeight =Math.Min(targetHeight, scaledHeight - cropY);

                // 缩放图片
                image.Mutate(x => x.Resize(scaledWidth, scaledHeight));

                // 裁剪图片
                image.Mutate(x => x.Crop(new Rectangle(cropX, cropY, targetWidth, targetHeight)));

                /*
                // 创建内存流
                var memoryStream = new MemoryStream();

                if (cacheFile != null)
                {
                    image.Save(cacheFile, imageEncoder);
                }

                // 将图片保存到内存流
                await image.SaveAsync(memoryStream, GetOutputFormat(Path.GetExtension(configuration.OutputFile)));

                // 重置流的位置
                memoryStream.Position = 0;

                return memoryStream;
                */

                //await image.SaveAsync(outputFile, format);
            }
        }
    };

    public async Task ScaleAsync(ImageScaleRequest req, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // 获取输出格式
        var outputFormat = ImageSharpHelper.GetOutputFormat(req.OutputExtension, (int)req.Quality);

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

            // 加载图片
            using var image = Image.Load(file);

            if (_scaleProcessors.TryGetValue(req.ScaleMode, out var processor))
            {
                processor(image, req.Width, req.Height);

                // 保存图片
                await image.SaveAsync(outputFile, outputFormat);

                OnScaled?.Invoke(this, new ScaleEventArgs(i));
            }

            i++;
        }
    }

    /// <summary>
    /// 根据源图缩放并返回 PNG 格式的内存流
    /// </summary>
    /// <param name="imageSource">源图</param>
    /// <param name="width">目标宽度</param>
    /// <param name="height">目标高度</param>
    /// <returns></returns>
    public async Task<Stream?> GetScaleStreamAsync(string imageSource, uint? width, uint? height, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_imageProcessHelper.IsImage(imageSource))
        {
            return null;
        }

        // 加载图片
        using var image = Image.Load(imageSource);

        if (_scaleProcessors.TryGetValue(ScaleMode.ResizeAndCrop, out var processor))
        {
            processor(image, width, height);
        }

        var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new PngEncoder());
        memoryStream.Position = 0;
        return memoryStream;
    }
}
