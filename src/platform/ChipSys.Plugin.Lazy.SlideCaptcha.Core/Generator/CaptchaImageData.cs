using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Exceptions;

namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core.Generator;

public class CaptchaImageData
{
    /// <summary>
    /// ����x����
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// ����y����
    /// </summary>
    public int Y { get; set; }
    /// <summary>
    /// ����ͼ��
    /// </summary>
    public int BackgroundImageWidth { get; set; }
    /// <summary>
    /// ����ͼ��
    /// </summary>
    public int BackgroundImageHeight { get; set; }
    /// <summary>
    /// ����ͼ
    /// </summary>
    public string BackgroundImageBase64 { get; set; }
    /// <summary>
    /// ������ͼ��
    /// </summary>
    public int SliderImageWidth { get; set; }
    /// <summary>
    /// ������ͼ��
    /// </summary>
    public int SliderImageHeight { get; set; }
    /// <summary>
    /// ������ͼ
    /// </summary>
    public string SliderImageBase64 { get; set; }

    /// <summary>
    /// ����λ�ðٷֱ�
    /// </summary>
    public float Percent
    {
        get
        {
            if (BackgroundImageWidth <= 0) return 0;
            return 1.0f * X / BackgroundImageWidth;
        }
    }

    public void Check()
    {
        // У��
        if (this.X <= 0) throw new SlideCaptchaException($"CaptchaImageData�����쳣: {nameof(X)}С�ڵ���0");
        if (this.Y <= 0) throw new SlideCaptchaException($"CaptchaImageData�����쳣: {nameof(Y)}С�ڵ���0");
        if (this.BackgroundImageWidth <= 0) throw new SlideCaptchaException($"CaptchaImageData�����쳣: {nameof(BackgroundImageWidth)}С�ڵ���0");
        if (this.BackgroundImageHeight <= 0) throw new SlideCaptchaException($"CaptchaImageData�����쳣: {nameof(BackgroundImageHeight)}С�ڵ���0");
        if (this.SliderImageWidth <= 0) throw new SlideCaptchaException($"CaptchaImageData�����쳣: {nameof(SliderImageWidth)}С�ڵ���0");
        if (this.SliderImageHeight <= 0) throw new SlideCaptchaException($"CaptchaImageData�����쳣: {nameof(SliderImageHeight)}С�ڵ���0");
        if(string.IsNullOrWhiteSpace(this.BackgroundImageBase64)) throw new SlideCaptchaException($"CaptchaImageData�����쳣: {nameof(BackgroundImageBase64)}Ϊ��");
        if (string.IsNullOrWhiteSpace(this.SliderImageBase64)) throw new SlideCaptchaException($"CaptchaImageData�����쳣: {nameof(SliderImageBase64)}Ϊ��");
    }
}
