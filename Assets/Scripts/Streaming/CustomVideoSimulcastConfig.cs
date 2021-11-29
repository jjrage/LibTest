#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

using FM.LiveSwitch.Vp8;
using System;
using System.Collections.Generic;

namespace FM.LiveSwitch
{
    internal class CustomVideoSimulcastConfig : CustomSimulcastConfig
    {
        private double _BitsPerPixel;

        public double BitsPerPixel
        {
            get
            {
                return _BitsPerPixel;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new Exception("Bits-per-pixel must be greater than or equal to zero.");
                }
                _BitsPerPixel = value;
            }
        }

        public VideoDegradationPreference DegradationPreference
        {
            get;
            set;
        }

        public CustomVideoSimulcastConfig(int encodingCount, int preferredBitrate)
            : this(encodingCount, preferredBitrate, 0.0)
        {
        }

        public CustomVideoSimulcastConfig(int encodingCount, int preferredBitrate, double bitsPerPixel)
            : base(encodingCount, preferredBitrate)
        {
            BitsPerPixel = bitsPerPixel;
            DegradationPreference = VideoDegradationPreference.Automatic;
        }

        public CustomVideoEncodingConfig[] GetEncodingConfigs(VideoType sourceType, int sourceWidth, int sourceHeight, double sourceFrameRate)
        {
            List<CustomVideoEncodingConfig> list = new List<CustomVideoEncodingConfig>();
            if (base.Disabled)
            {
                list.Add(new CustomVideoEncodingConfig());
            }
            else
            {
                Format format = new Format();
                int num = MathAssistant.Min(MathAssistant.Max(format.MinBitrate, VideoUtility.GetBitrate(base.PreferredBitrate, sourceWidth, sourceHeight, sourceFrameRate, BitsPerPixel)), format.MaxBitrate);
                VideoDegradationPreference degradationPreference = VideoUtility.ProcessDegradationPreference(DegradationPreference, sourceType);
                for (int i = 0; i < base.EncodingCount; i++)
                {
                    double num2 = MathAssistant.Pow(1.0 / MathAssistant.Pow(2.0, i), VideoUtility.BitratePowerScale);
                    CustomVideoEncodingConfig videoEncodingConfig = new CustomVideoEncodingConfig
                    {
                        Bitrate = (int)MathAssistant.Ceil((double)num * num2)
                    };
                    VideoUtility.UpdateEncodingConfig(videoEncodingConfig, degradationPreference, num2, sourceFrameRate);
                    if (videoEncodingConfig.Bitrate >= format.MinBitrate)
                    {
                        list.Add(videoEncodingConfig);
                    }
                }
            }
            return list.ToArray();
        }
    }
}
