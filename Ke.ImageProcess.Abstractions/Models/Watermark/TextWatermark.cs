namespace Ke.ImageProcess.Models.Watermark;

public class TextWatermark(string text) : WatermarkBase
{
    /// <summary>
    /// 水印文本
    /// </summary>
    public string Text { get; set; } = text;
    /// <summary>
    /// 字体
    /// </summary>
    public string? FontFamily { get; set; }
    /// <summary>
    /// 字体大小
    /// </summary>
    public int? FontSize { get; set; } = 20;
    /// <summary>
    /// 文本颜色
    /// </summary>
    public RgbaColor TextColor { get; set; }
    /// <summary>
    ///
    /// </summary>
    public RgbaColor? BackgroundColor { get; set; } = new RgbaColor(0, 0, 0, 0);
    /// <summary>
    /// 边框颜色
    /// </summary>
    public RgbaColor? StrokeColor { get; set; }
    /// <summary>
    /// 边框宽度
    /// </summary>
    public double? StrokeWidth { get; set; }
}
