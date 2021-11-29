#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

using FM.LiveSwitch.Opus;
using System.Collections.Generic;

namespace FM.LiveSwitch
{
    internal class CustomAudioSimulcastConfig : CustomSimulcastConfig
    {
        public CustomAudioSimulcastConfig(int encodingCount, int preferredBitrate)
            : base(encodingCount, preferredBitrate)
        {
        }

        internal CustomAudioEncodingConfig[] GetEncodingConfigs()
        {
            List<CustomAudioEncodingConfig> list = new List<CustomAudioEncodingConfig>();
            if (base.Disabled)
            {
                list.Add(new CustomAudioEncodingConfig());
            }
            else
            {
                Format format = new Format();
                int num = MathAssistant.Min(MathAssistant.Max(format.MinBitrate, base.PreferredBitrate), format.MaxBitrate);
                for (int i = 0; i < base.EncodingCount; i++)
                {
                    double num2 = 1.0 / MathAssistant.Pow(2.0, i);
                    CustomAudioEncodingConfig audioEncodingConfig = new CustomAudioEncodingConfig
                    {
                        Bitrate = (int)MathAssistant.Ceil((double)num * num2)
                    };
                    if (audioEncodingConfig.Bitrate >= format.MinBitrate)
                    {
                        list.Add(audioEncodingConfig);
                    }
                }
            }
            return list.ToArray();
        }
    }
}
