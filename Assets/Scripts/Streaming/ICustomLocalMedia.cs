#region сборка FM.LiveSwitch, Version=1.11.3.40505, Culture=neutral, PublicKeyToken=null
// C:\Unity\Projects\LibTest\LibTest\Assets\Resources\Not separate dlls\FM.LiveSwitch.dll
// Decompiled with ICSharpCode.Decompiler 4.0.0.4521
#endregion

namespace FM.LiveSwitch
{
    public interface ICustomLocalMedia<TCustomLocalMedia, TIAudioTrack, TIVideoTrack> : IMedia<TIAudioTrack, TIVideoTrack> where TCustomLocalMedia : class where TIAudioTrack : IAudioTrack where TIVideoTrack : IVideoTrack
    {
        LocalMediaState State
        {
            get;
        }

        SourceInput AudioSourceInput
        {
            get;
            set;
        }

        SourceInput VideoSourceInput
        {
            get;
            set;
        }

        CustomAudioEncodingConfig AudioEncoding
        {
            get;
        }

        CustomAudioEncodingConfig[] AudioEncodings
        {
            get;
            set;
        }

        CustomVideoEncodingConfig VideoEncoding
        {
            get;
        }

        CustomVideoEncodingConfig[] VideoEncodings
        {
            get;
            set;
        }

        bool AudioSimulcastDisabled
        {
            get;
            set;
        }

        int AudioSimulcastEncodingCount
        {
            get;
            set;
        }

        int AudioSimulcastPreferredBitrate
        {
            get;
            set;
        }

        bool VideoSimulcastDisabled
        {
            get;
            set;
        }

        int VideoSimulcastEncodingCount
        {
            get;
            set;
        }

        int VideoSimulcastPreferredBitrate
        {
            get;
            set;
        }

        double VideoSimulcastBitsPerPixel
        {
            get;
            set;
        }

        VideoDegradationPreference VideoSimulcastDegradationPreference
        {
            get;
            set;
        }

        event Action0 OnAudioStarted;

        event Action0 OnVideoStarted;

        event Action0 OnAudioStopped;

        event Action0 OnVideoStopped;

        Future<SourceInput[]> GetAudioSourceInputs();

        Future<SourceInput[]> GetVideoSourceInputs();

        Future<object> ChangeAudioSourceInput(SourceInput audioSourceInput);

        Future<object> ChangeVideoSourceInput(SourceInput videoSourceInput);

        Future<TCustomLocalMedia> Start();

        Future<TCustomLocalMedia> Stop();
    }
}
