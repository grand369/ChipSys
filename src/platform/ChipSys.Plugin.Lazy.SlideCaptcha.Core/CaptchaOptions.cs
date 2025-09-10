using ChipSys.Plugin.Lazy.SlideCaptcha.Core.Resources;

namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core
{
    public class CaptchaOptions
    {
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public int ExpirySeconds { get; set; } = 60;

        /// <summary>
        /// �洢��ǰ׺
        /// </summary>
        public string StoreageKeyPrefix { get; set; } = "slide-captcha";

        /// <summary>
        /// �ݴ�ֵ(У��ʱ�ã�ȱ��λ����ʵ�ʻ���λ��ƥ���ݴ�Χ)
        /// </summary>
        public float Tolerant { get; set; } = 0.02f;

        /// <summary>
        /// ���ſ�����
        /// </summary>
        public int InterferenceCount { get; set; } = 0;

        /// <summary>
        /// ����ͼ
        /// </summary>
        public List<Resource> Backgrounds { get; set; } = new List<Resource>();

        /// <summary>
        /// ģ��ͼ(������slider,hole��˳�����γ���)
        /// </summary>
        public List<TemplatePair> Templates { get; set; } = new List<TemplatePair>();
    }
}
