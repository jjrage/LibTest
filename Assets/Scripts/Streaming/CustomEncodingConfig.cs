#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

using System.Collections.Generic;

namespace FM.LiveSwitch
{
    public abstract class CustomEncodingConfig
    {
        public string RtpStreamId
        {
            get;
            set;
        }

        public long SynchronizationSource
        {
            get;
            set;
        }

        public bool Deactivated
        {
            get;
            set;
        }

        public int Bitrate
        {
            get;
            set;
        }

        public CustomEncodingConfig()
        {
            RtpStreamId = null;
            SynchronizationSource = -1L;
            Deactivated = false;
            Bitrate = -1;
        }

        public CustomEncodingConfig(EncodingInfo encoding)
        {
            RtpStreamId = encoding.RtpStreamId;
            SynchronizationSource = encoding.SynchronizationSource;
            Deactivated = encoding.Deactivated;
            Bitrate = encoding.Bitrate;
        }

        protected virtual void SerializeProperties(Dictionary<string, string> jsonObject)
        {
            if (RtpStreamId != null)
            {
                jsonObject["rtpStreamId"] = JsonSerializer.SerializeString(RtpStreamId);
            }
            if (SynchronizationSource != -1)
            {
                jsonObject["synchronizationSource"] = JsonSerializer.SerializeLong(SynchronizationSource);
            }
            if (Deactivated)
            {
                jsonObject["deactivated"] = JsonSerializer.SerializeBoolean(Deactivated);
            }
            if (Bitrate != -1)
            {
                jsonObject["bitrate"] = JsonSerializer.SerializeInteger(Bitrate);
            }
        }

        protected virtual void DeserializeProperties(string key, string valueJson)
        {
            if (!(key == "rtpStreamId"))
            {
                if (!(key == "synchronizationSource"))
                {
                    if (!(key == "deactivated"))
                    {
                        if (key == "bitrate")
                        {
                            Bitrate = JsonSerializer.DeserializeInteger(valueJson).Value;
                        }
                    }
                    else
                    {
                        Deactivated = JsonSerializer.DeserializeBoolean(valueJson).Value;
                    }
                }
                else
                {
                    SynchronizationSource = JsonSerializer.DeserializeLong(valueJson).Value;
                }
            }
            else
            {
                RtpStreamId = JsonSerializer.DeserializeString(valueJson);
            }
        }
    }
}
