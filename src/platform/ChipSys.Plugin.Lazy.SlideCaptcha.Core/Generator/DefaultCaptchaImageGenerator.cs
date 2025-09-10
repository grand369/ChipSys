using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Resources;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core.Generator;

public class DefaultCaptchaImageGenerator : ICaptchaImageGenerator
{
    private IResourceManager _resourceManager;
    private Random _random = new Random();
    private CaptchaOptions _options;

    public DefaultCaptchaImageGenerator(IResourceManager resourceManager, IOptionsSnapshot<CaptchaOptions> options)
    {
        _resourceManager = resourceManager;
        _options = options.Value;
    }

    /// <summary>
    /// ���㰼������
    /// ԭ�� һ��һ��ɨ�裬ÿ�в�͸��С���������γ�����С������
    /// </summary>
    /// <param name="holeTemplateImage"></param>
    /// <returns></returns>
    private static SKPath CalcHoleShape(SKBitmap holeTemplateImage)
    {
        int temp = 0;
        var path = new SKPath();
        for (int y = 0; y < holeTemplateImage.Height; y++)
        {
            for (int x = 0; x < holeTemplateImage.Width; x++)
            {
                var pixel = holeTemplateImage.GetPixel(x, y);
                if (pixel.Alpha != 0)
                {
                    if (temp == 0)
                    {
                        temp = x;
                    }
                }
                else
                {
                    if (temp != 0)
                    {
                        path.AddRect(new SKRect(temp, y, x, y + 1));
                        temp = 0;
                    }
                }
            }
        }
        return path;
    }

    public CaptchaImageData Generate()
    {
        var background = _resourceManager.RandomBackground();
        (var silder, var hole) = _resourceManager.RandomTemplate();

        using var backgroundImage = SKBitmap.FromImage(SKImage.FromEncodedData(background));
        using var sliderTemplateImage = SKBitmap.FromImage(SKImage.FromEncodedData(silder));
        using var holeTemplateImage = SKBitmap.FromImage(SKImage.FromEncodedData(hole));
        using var holeMattingImage = new SKBitmap(sliderTemplateImage.Width, sliderTemplateImage.Height);
        using var sliderBarImage = new SKBitmap(sliderTemplateImage.Width, backgroundImage.Height);

        // ����λ��
        int randomX = _random.Next(holeTemplateImage.Width + 5, backgroundImage.Width - holeTemplateImage.Width - 10);
        int randomY = _random.Next(5, backgroundImage.Height - holeTemplateImage.Height - 5);

        // ����͸���ȼ��㰼��ͼ������״(��״�ɲ�͸�������γ�)
        var holeShape = CalcHoleShape(holeTemplateImage);
        // ���ɰ��ۿ�ͼ
        using var slideCanvas = new SKCanvas(holeMattingImage);
        slideCanvas.ClipPath(holeShape, SKClipOperation.Intersect, true);
        slideCanvas.DrawBitmap(backgroundImage, -randomX, -randomY);
        // �����Ͽ�ģ��
        slideCanvas.DrawBitmap(sliderTemplateImage, 0, 0);
        // �����Ͽ���
        var slideBarCanvas = new SKCanvas(sliderBarImage);
        slideBarCanvas.DrawBitmap(holeMattingImage, 0, randomY);

        // ���ɱ���
        using var bgCanvas = new SKCanvas(backgroundImage);
        bgCanvas.DrawBitmap(holeTemplateImage, randomX, randomY);
        // ���Ƹ��ſ�
        DrawInterferences(backgroundImage, holeTemplateImage, randomX, randomY, _options.InterferenceCount);

        return new CaptchaImageData
        {
            X = randomX,
            Y = randomY,
            BackgroundImageWidth = backgroundImage.Width,
            BackgroundImageHeight = backgroundImage.Height,
            BackgroundImageBase64 = ToBase64(backgroundImage),
            SliderImageWidth = holeMattingImage.Width,
            SliderImageHeight = holeMattingImage.Height,
            SliderImageBase64 = ToBase64(sliderBarImage)
        };
    }

    /// <summary>
    /// ���ɸ��ſ�
    /// Ҫ��: ����ȱ�鲻���ص�, 
    /// </summary>
    /// <param name="backgroundImage"></param>
    /// <param name="holeTemplateImage"></param>
    /// <param name="holeX"></param>
    /// <param name="holeY"></param>
    /// <param name="count"></param>
    private void DrawInterferences(SKBitmap backgroundImage, SKBitmap holeTemplateImage, int holeX, int holeY, int count)
    {
        if (count <= 0) return;

        var holdeWidth = holeTemplateImage.Width;
        var holdeHeight = holeTemplateImage.Height;
        var minX = holdeWidth + 5;
        var maxX = backgroundImage.Width - holdeWidth - 10;
        var minY = 5;
        var maxY = backgroundImage.Height - holdeHeight - 5;

        using var canvas = new SKCanvas(backgroundImage);

        var excludeRegions = new List<SKRect>
        {
            new SKRect(holeX, holeY, holeX + holdeWidth, holeY + holdeHeight)
        };
        for (var i = 0; i < count; i++)
        {
            (var x, var y) = GenerateInterferencePosition(minX, maxX, minY, maxY, holeX, holeY, holdeWidth, holdeHeight, excludeRegions);
            if (x == 0) continue;

            canvas.DrawBitmap(holeTemplateImage, x, y);
            excludeRegions.Add(new SKRect(x, y, x + holdeWidth, y + holdeHeight));
        }
    }

    /// <summary>
    /// ���ɸ��ſ�λ��
    /// </summary>
    /// <param name="minX">�޶���Χ��Сx</param>
    /// <param name="maxX">�޶���Χ���x</param>
    /// <param name="minY">�޶���Χ��Сy</param>
    /// <param name="maxY">�޶���Χ���y</param>
    /// <param name="holeX">ȱ��X</param>
    /// <param name="holeY">ȱ��Y</param>
    /// <param name="holeWidth">ȱ����</param>
    /// <param name="holeHeight">ȱ��߶�</param>
    /// <param name="excludeRegions">�ų��ķ�Χ</param>
    /// <returns></returns>
    private (int x, int y) GenerateInterferencePosition(int minX, int maxX, int minY, int maxY, int holeX, int holeY, int holeWidth, int holeHeight, List<SKRect> excludeRegions)
    {
        var random = new Random();

        for (var i = 0; i < 100; i++)
        {
            var x = random.Next(minX, maxX);
            var y = random.Next(minY, maxY);

            // Ҫ���ȱ���Y��һ�����
            if (Math.Abs(y - holeY) < holeHeight / 2) continue;

            var region = new SKRect(x, y, x + holeWidth, y + holeHeight);
            var isIntersect = excludeRegions.Any(x => x.IntersectsWith(region));
            if (isIntersect) continue;

            return (x, y);
        }

        // ����δ�ҵ�
        return (0, 0);
    }

    private static string ToBase64(SKBitmap bitmap)
    {
        var bytes = bitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();
        return "data:image/png;base64," + Convert.ToBase64String(bytes);
    }
}
