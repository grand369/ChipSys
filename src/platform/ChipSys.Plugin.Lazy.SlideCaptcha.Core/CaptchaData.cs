namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core;

public class CaptchaData
{
    public CaptchaData(string id, string backgroundImage, string sliderImage)
    {
        Id = id;
        BackgroundImage = backgroundImage;
        SliderImage = sliderImage;
    }

    /// <summary>
    /// id
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// ±³¾°Í¼(º¬°¼²Û)
    /// </summary>
    public string BackgroundImage { get; set; }
    /// <summary>
    /// »¬¶¯¿éÍ¼
    /// </summary>
    public string SliderImage { get; set; }
}
