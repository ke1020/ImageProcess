
namespace Ke.ImageProcess.Models.Convert;

public class ImageConvertRequest : ImageProcessRequestBase
{
    public ImageConvertRequest(ICollection<string> imageSources, string outputFilePath, string outputExtension) :
        base(imageSources, outputFilePath, outputExtension)
    {
        Suffix = "-c";
    }
}
