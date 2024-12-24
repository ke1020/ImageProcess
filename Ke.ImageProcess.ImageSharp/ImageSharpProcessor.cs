using Ke.ImageProcess.Abstractions;
using Ke.ImageProcess.Models.Convert;
using Ke.ImageProcess.Models.Scale;
using Ke.ImageProcess.Models.Watermark;

namespace Ke.ImageProcess.ImageSharp;

/// <summary>
/// 图片处理器
/// </summary>
/// <param name="converter">格式转换器</param>
/// <param name="watermarker">水印处理器</param>
/// <param name="scaler">缩放处理器</param>
public class ImageSharpProcessor(IImageConverter converter, IImageWatermarker watermarker, IImageScaler scaler) : IImageProcessor
{
    private readonly IImageConverter _converter = converter;
    private readonly IImageWatermarker _watermarker = watermarker;
    private readonly IImageScaler _scaler = scaler;
    public event EventHandler<ConvertEventArgs>? OnConverted;
    public event EventHandler<ScaleEventArgs>? OnScaled;
    public event EventHandler<WatermarkEventArgs>? OnWatermarked;

    public Task ConvertAsync(ImageConvertRequest req, CancellationToken cancellationToken = default)
    {
        _converter.OnConverted += OnConverted;
        return _converter.ConvertAsync(req, cancellationToken);
    }

    public Task<Stream?> GetScaleStreamAsync(string imageSource, uint? width, uint? height, CancellationToken cancellationToken = default) =>
        _scaler.GetScaleStreamAsync(imageSource, width, height, cancellationToken);

    public Task ScaleAsync(ImageScaleRequest req, CancellationToken cancellationToken = default)
    {
        _scaler.OnScaled += OnScaled;
        return _scaler.ScaleAsync(req, cancellationToken);
    }

    public Task WatermarkAsync<T>(ImageWatermarkRequest<T> req, CancellationToken cancellationToken = default) where T : WatermarkBase
    {
        _watermarker.OnWatermarked += OnWatermarked;
        return _watermarker.WatermarkAsync(req, cancellationToken);
    }
}
