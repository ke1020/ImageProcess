namespace Ke.ImageProcess.Models;

public struct RgbaColor
{
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    /// <summary>
    /// 值 0-100
    /// </summary>
    public byte Alpha { get; set; }

    public RgbaColor(byte red, byte green, byte blue, byte alpha)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public override string ToString()
    {
        return $"R:{Red}, G:{Green}, B:{Blue}, A:{Alpha}";
    }
}
