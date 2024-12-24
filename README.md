# 图片处理库

## 使用方式

项目的 `.csproj` 文件中添加项目引用

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <ItemGroup>
        <ProjectReference
        Include="ImageProcess/Ke.ImageProcess.ImageSharp/Ke.ImageProcess.ImageSharp.csproj" />
    </ItemGroup>
</Project>
```

注册服务

```cs
void RegisterServices(IServiceCollection services)
{
    services.AddImageSharp();
}
```

使用服务

```cs
public class Service
{
    private readonly IImageConverter _converter;
    private readonly IImageScaler _scaler;
    private readonly IImageWatermarker _imageWatermarker

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="converter">图片格式转换服务对象</param>
    /// <param name="scaler">图片缩放服务对象</param>
    /// <param name="watermarker">图片水印服务对象</param>
    public Service(IImageConverter converter, IImageScaler scaler, IImageWatermarker watermarker)
    {
        _converter = converter;
        _scaler = scaler;
        _imageWatermarker = watermarker;
    }

    /// <summary>
    /// 图片格式转换
    /// </summary>
    private async Task ImageConvertAsync()
    {
        // 注册图片格式转换完成事件
        _converter.OnConverted += (sender, e) =>
        {
            // 图片格式转换完成，设置进度为 100
            SetProgress(100);
        };

        // 执行转换
        await _converter.ConvertAsync(new ImageConvertRequest(["C:/1.jpg", "C:/2.jpg"], "D:/Output/", "png")
        {
            Quality = 90
        });
    }

    /// <summary>
    /// 图片缩放
    /// </summary>
    private async Task ImageScaleAsync()
    {
        // 缩放执行完毕事件
        _scaler.OnScaled += (sender, args) =>
        {
            SetProgress(100);
        };

        await _scaler.ScaleAsync(new ImageScaleRequest(["C:/1.jpg", "C:/2.jpg"], "D:/Output/", "png")
        {
            ScaleMode = ScaleMode.EqualRatio, // 等比缩放
            Quality = 90,
            Width = 200,
            Height = 200,
        });
    }

    /// <summary>
    /// 图片水印
    /// </summary>
    private async Task ImageWatermarkAsync()
    {
        // 水印执行完毕事件
        _imageWatermarker.OnWatermarked += (sender, args) =>
        {
            SetProgress(100);
        };

        // 图片水印
        await _imageWatermarker.WatermarkAsync(new ImageWatermarkRequest<ImageWatermark>(["C:/1.jpg", "C:/2.jpg"], "D:/Output/", "png")
        {
            Mode = WatermarkMode.Image, // 图片水印
            Watermark = new ImageWatermark("C:/watermark.png"),
            // IsTite 为 true 时该项无效
            Position = WatermarkPosition.BottomRight,
            Opacity = (double)argments.Opacity / 100, // 透明度
            Suffix = "-wt", // 输出文件后缀
            Rotation = 45, // 水印旋转角度
            IsTile = false // 是否平铺水印
        });

        // 文本水印
        await _imageWatermarker.WatermarkAsync(new ImageWatermarkRequest<TextWatermark>(["C:/1.jpg", "C:/2.jpg"], "D:/Output/", "png")
        {
            Mode = WatermarkMode.Text, // 文本水印
            Watermark = new TextWatermark(args.WatermarkText ?? string.Empty)
            {
                // ImageSharp 可以写安装到系统的字体名称
                FontFamily = "Microsoft YaHei",
                FontSize = 16, // 字体大小
                TextColor = new RgbaColor(200, 200, 200, 50), // 文本颜色
                //StrokeColor = new RgbaColor(255, 255, 255, 50),
                StrokeWidth = 2,
                // BackgroundColor = new RgbaColor(0, 0, 0, 0),
            },
            // IsTite 为 true 时该项无效
            Position = WatermarkPosition.BottomRight,
            Opacity = (double)argments.Opacity / 100, // 透明度
            Suffix = "-wt", // 输出文件后缀
            Rotation = 45, // 水印旋转角度
            IsTile = false // 是否平铺水印
        });
    }
}
```
