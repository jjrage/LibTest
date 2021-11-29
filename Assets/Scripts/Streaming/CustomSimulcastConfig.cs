#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

using System;

namespace FM.LiveSwitch
{
    internal abstract class CustomSimulcastConfig
    {
        private int _EncodingCount;

        private int _PreferredBitrate;

        public bool Disabled
        {
            get;
            set;
        }

        public int EncodingCount
        {
            get
            {
                return _EncodingCount;
            }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Encoding count must be a positive integer.");
                }
                _EncodingCount = value;
            }
        }

        public int PreferredBitrate
        {
            get
            {
                return _PreferredBitrate;
            }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Preferred bitrate must be a positive integer.");
                }
                _PreferredBitrate = value;
            }
        }

        public CustomSimulcastConfig(int encodingCount, int preferredBitrate)
        {
            EncodingCount = encodingCount;
            PreferredBitrate = preferredBitrate;
        }
    }
}
