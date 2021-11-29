#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

using System.Collections.Generic;

namespace FM.LiveSwitch
{
    public class CustomVideoEncodingConfig : CustomEncodingConfig
    {
        public double FrameRate
        {
            get;
            set;
        }

        public double Scale
        {
            get;
            set;
        }

        public CustomVideoEncodingConfig()
        {
            FrameRate = -1.0;
            Scale = -1.0;
        }

        public CustomVideoEncodingConfig(EncodingInfo encoding)
            : base(encoding)
        {
            FrameRate = encoding.FrameRate;
            Scale = encoding.Scale;
        }

        public string ToJson()
        {
            return ToJson(this);
        }

        public static string ToJson(CustomVideoEncodingConfig encodingConfig)
        {
            return JsonSerializer.SerializeObject(encodingConfig, delegate (CustomVideoEncodingConfig info, Dictionary<string, string> dict)
            {
                info.SerializeProperties(dict);
            });
        }

        public static CustomVideoEncodingConfig FromJson(string encodingConfigJson)
        {
            return JsonSerializer.DeserializeObject(encodingConfigJson, () => new CustomVideoEncodingConfig(), delegate (CustomVideoEncodingConfig info, string key, string value)
            {
                info.DeserializeProperties(key, value);
            });
        }

        public static string ToJsonArray(CustomVideoEncodingConfig[] encodingConfigs)
        {
            return JsonSerializer.SerializeObjectArray(encodingConfigs, ToJson);
        }

        public static CustomVideoEncodingConfig[] FromJsonArray(string encodingConfigsJson)
        {
            return JsonSerializer.DeserializeObjectArray(encodingConfigsJson, FromJson)?.ToArray();
        }

        protected override void SerializeProperties(Dictionary<string, string> jsonObject)
        {
            base.SerializeProperties(jsonObject);
            if (FrameRate != -1.0)
            {
                jsonObject["frameRate"] = JsonSerializer.SerializeDouble(FrameRate);
            }
            if (Scale != -1.0)
            {
                jsonObject["scale"] = JsonSerializer.SerializeDouble(Scale);
            }
        }

        protected override void DeserializeProperties(string key, string valueJson)
        {
            base.DeserializeProperties(key, valueJson);
            if (!(key == "frameRate"))
            {
                if (key == "scale")
                {
                    Scale = JsonSerializer.DeserializeDouble(valueJson).Value;
                }
            }
            else
            {
                FrameRate = JsonSerializer.DeserializeDouble(valueJson).Value;
            }
        }

        public override string ToString()
        {
            bool flag = false;
            List<string> list = new List<string>();
            string rtpStreamId = base.RtpStreamId;
            if (rtpStreamId != null)
            {
                list.Add($"RTP Stream ID: {rtpStreamId}");
            }
            long synchronizationSource = base.SynchronizationSource;
            if (synchronizationSource != -1)
            {
                list.Add($"Synchronization Source: {synchronizationSource.ToString()}");
            }
            bool deactivated = base.Deactivated;
            if (deactivated)
            {
                list.Add($"Deactivated: {deactivated.ToString()}");
            }
            int bitrate = base.Bitrate;
            if (bitrate != -1)
            {
                flag = true;
                list.Add($"Bitrate: {bitrate.ToString()}");
            }
            double frameRate = FrameRate;
            if (frameRate != -1.0)
            {
                flag = true;
                list.Add($"Frame Rate: {frameRate.ToString()}");
            }
            double scale = Scale;
            if (scale != -1.0)
            {
                flag = true;
                list.Add($"Scale: {scale.ToString()}");
            }
            string text = string.Join(", ", list.ToArray());
            if (!flag)
            {
                text = $"{text} [Unrestricted]".Trim();
            }
            return text;
        }
    }
}
