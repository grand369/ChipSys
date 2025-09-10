using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Exceptions;

namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core.Validator;

/// <summary>
/// �����켣
/// </summary>
public class SlideTrack
{
    /// <summary>
    /// ����ͼƬ���(���ܾ������ţ�����ԭʼͼƬ���)
    /// </summary>
    public int BackgroundImageWidth { get; set; }
    /// <summary>
    /// ����ͼƬ�߶�(���ܾ������ţ�����ԭʼͼƬ���)
    /// </summary>
    public int BackgroundImageHeight { get; set; }
    /// <summary>
    /// ����ͼƬ���(���ܾ������ţ�����ԭʼͼƬ���)
    /// </summary>
    public int SliderImageWidth { get; set; }
    /// <summary>
    /// ����ͼƬ�߶�(���ܾ������ţ�����ԭʼͼƬ���)
    /// </summary>
    public int SliderImageHeight { get; set; }
    /// <summary>
    /// ������ʼʱ��(���ܾ������ţ�����ԭʼͼƬ���)
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// ��������ʱ��
    /// </summary>
    public DateTime EndTime { get; set; }
    /// <summary>
    /// �켣
    /// </summary>
    public List<Track> Tracks { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public float Percent
    {
        get
        {
            if (this.BackgroundImageWidth <= 0) return -1;
            if (this.Tracks.Count <= 0) return -1;

            var lastTrack = this.Tracks[this.Tracks.Count - 1];
            return 1.0f * lastTrack.X / this.BackgroundImageWidth;
        }
    }

    public void Check()
    {
        // У��
        if (this.BackgroundImageWidth <= 0) throw new SlideCaptchaException($"SlideTrack�����쳣: {nameof(BackgroundImageWidth)}С�ڵ���0");
        if (this.BackgroundImageHeight <= 0) throw new SlideCaptchaException($"SlideTrack�����쳣: {nameof(BackgroundImageHeight)}С�ڵ���0");
        if (this.SliderImageWidth <= 0) throw new SlideCaptchaException($"SlideTrack�����쳣: {nameof(SliderImageWidth)}С�ڵ���0");
        if (this.SliderImageHeight <= 0) throw new SlideCaptchaException($"SlideTrack�����쳣: {nameof(SliderImageHeight)}С�ڵ���0");
        if (this.StartTime == DateTime.MinValue) throw new SlideCaptchaException($"SlideTrack�����쳣: {nameof(StartTime)}Ϊ��");
        if (this.EndTime == DateTime.MinValue) throw new SlideCaptchaException($"SlideTrack�����쳣: {nameof(EndTime)}Ϊ��");
    }

    public void CheckTracks()
    {
        if(this.Tracks == null || this.Tracks.Count == 0) throw new SlideCaptchaException($"SlideTrack�����쳣: {nameof(Tracks)}Ϊ��");
    }
}

public class Track
{
    public int X { get; set; }
    public int Y { get; set; }
    public int T { get; set; }
}
