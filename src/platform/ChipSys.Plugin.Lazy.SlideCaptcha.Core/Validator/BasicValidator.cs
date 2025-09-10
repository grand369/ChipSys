namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core.Validator;

public class BasicValidator : BaseValidator, IValidator
{
    public override bool ValidateCore(SlideTrack slideTrack, CaptchaValidateData captchaValidateData)
    {
        slideTrack.CheckTracks();

        // ������Ϊ�켣���
        var startSlidingTime = slideTrack.StartTime.ToFileTimeUtc();
        long endSlidingTime = slideTrack.EndTime.ToFileTimeUtc();
        var bgImageWidth = slideTrack.BackgroundImageWidth;
        var trackList = slideTrack.Tracks;

        // check1: ����ʱ�����С��300���� ����false
        if (startSlidingTime + 300 > endSlidingTime)
        {
            return false;
        }

        // check2:�켣����Ҫ������10�����ߴ��ڱ�����ȵ��屶 ����false
        if (trackList.Count < 10 || trackList.Count > bgImageWidth * 5)
        {
            return false;
        }

        // check3:x���y��Ӧ���Ǵ�0��ʼ�ģ�Ҫ��һ��ʼx���y�����ܣ�����false
        var firstTrack = trackList[0];
        if (firstTrack.X > 10 || firstTrack.X < -10 || firstTrack.Y > 10 || firstTrack.Y < -10)
        {
            return false;
        }

        // check4: ���y������ͬ�ģ���Ȼ�ǻ���������ֱ�ӷ���false ����ʱȥ��������ʧ�ܣ�
        // check5��x�����y��ֱ�ӵ�������Ծ����Ļ����� false ����ʱȥ��������ʧ�ܣ�
        // check6: ���x�ᳬ��ͼƬ��ȵ�Ƶ�ʹ��ߣ�����false
        int check4 = 1;
        int check6 = 0;
        for (int i = 1; i < trackList.Count; i++)
        {
            var track = trackList[i];
            // check4
            if (firstTrack.Y == track.Y) check4++;

            // check7
            if (track.X >= bgImageWidth) check6++;

            // check5
            var preTrack = trackList[i - 1];
            if ((track.X - preTrack.X) > 50 || (track.Y - preTrack.Y) > 50) return false; // ���������϶����ܵ���������֤��ͨ��
        }
        if (check4 == trackList.Count || check6 > 200)
        {
            return false;
        }

        return true;

        //// check7: x��Ӧ�����ɿ쵽���ģ� Ҫ������һ�£�����false
        //int splitPos = (int)(trackList.Count * 0.7);
        //var splitPostTrack = trackList[splitPos - 1];
        //int posTime = splitPostTrack.T;
        //float startAvgPosTime = posTime / (float)splitPos;

        //var lastTrack = trackList[trackList.Count - 1];
        //float endAvgPosTime = (lastTrack.T - posTime) / (float)(trackList.Count - splitPos);

        //return endAvgPosTime > startAvgPosTime;
    }
}
