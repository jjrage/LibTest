using agora_gaming_rtc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamClient : IVideoChatClient
{
    public event Action OnJoinedSuccess;
    private RenderTexture _videoSource;
    protected IRtcEngine mRtcEngine;
    protected string mChannel;
    private AudioRawDataManager mAudioManager;

    protected virtual void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
        OnJoinedSuccess?.Invoke();
    }
    protected virtual void OnUserJoined(uint uid, int elapsed)
    {
        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);

        // find a game object to render video stream from 'uid'
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }

        //// create a GameObject and assign to this new user
        //VideoSurface videoSurface = makeImageSurface(uid.ToString());
        //if (!ReferenceEquals(videoSurface, null))
        //{
        //    // configure videoSurface
        //    videoSurface.SetForUser(uid);
        //    videoSurface.SetEnable(true);
        //    videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        //    videoSurface.SetGameFps(30);
        //    videoSurface.EnableFilpTextureApply(enableFlipHorizontal: true, enableFlipVertical: false);
        //    UserVideoDict[uid] = videoSurface;
        //    Vector2 pos = AgoraUIUtils.GetRandomPosition(100);
        //    videoSurface.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        //}
    }

    internal void DisableEngine()
    {
        IRtcEngine.Destroy();
    }

    public void EnableVideo(bool enable)
    {
        if (mRtcEngine != null)
        {
            mRtcEngine.EnableVideo();
        }
    }

    public void SetVideoSource(RenderTexture texture)
    {
        _videoSource = texture;
    }

    public void PushFrame(ExternalVideoFrame frame)
    {
        IRtcEngine rtc = IRtcEngine.QueryEngine();
        if (rtc != null)
        {
            int a = rtc.PushVideoFrame(frame);
            Debug.Log($"Frame pushed {a}");
        }
    }
    public void SetAudioDevice()
    {
        int enableAudio = mRtcEngine.EnableAudio();
        int enableLocalAudio = mRtcEngine.EnableLocalAudio(true);
        Debug.Log($"audio {enableAudio} local audio {enableLocalAudio}");

        var rawAudioManager = mRtcEngine.GetAudioRawDataManager();
        rawAudioManager.SetOnPlaybackAudioFrameCallback(OnPlayAudioFrame);
        int registerAudioRawDataObserver = rawAudioManager.RegisterAudioRawDataObserver();

        Debug.Log($"raw audio data observer {registerAudioRawDataObserver}");
        //mRtcEngine.SetExternalAudioSource(true, 48000, 2);
        mRtcEngine.SetAudioProfile(AUDIO_PROFILE_TYPE.AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO, AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_GAME_STREAMING);
    }

    public IEnumerator AudioCheck()
    {
        yield return new WaitForSeconds(3);
        var audioPlaybackDeviceManager = mRtcEngine.GetAudioPlaybackDeviceManager();
        audioPlaybackDeviceManager.CreateAAudioPlaybackDeviceManager();
        int deviceCount = audioPlaybackDeviceManager.GetAudioPlaybackDeviceCount();
        string deviceID = "{0.0.0.00000000}.{4e6ff308-4316-4add-90eb-5a6582b123b0}";
        int setAudioPlaybackDevice = audioPlaybackDeviceManager.SetAudioPlaybackDevice(deviceID);
        int getAudioPlaybackDevice = audioPlaybackDeviceManager.GetCurrentPlaybackDevice(ref deviceID);

        Debug.Log($"device count {deviceCount}, {setAudioPlaybackDevice}, {getAudioPlaybackDevice}");

        for (int i = 0; i < deviceCount; i++)
        {
            string deviceName = string.Empty;
            string deviceid = string.Empty;
            audioPlaybackDeviceManager.GetAudioPlaybackDevice(i, ref deviceName, ref deviceid);
            Debug.Log($"Device name {deviceName}, device id {deviceid}");
        }

        var audioRecordingDeviceManager = mRtcEngine.GetAudioRecordingDeviceManager();
        audioRecordingDeviceManager.CreateAAudioRecordingDeviceManager();
        int recordingDeviceCount = audioRecordingDeviceManager.GetAudioRecordingDeviceCount();

        Debug.Log($"device count {recordingDeviceCount}");
        for (int i = 0; i < recordingDeviceCount; i++)
        {
            string deviceName = string.Empty;
            string deviceid = string.Empty;
            audioPlaybackDeviceManager.GetAudioPlaybackDevice(i, ref deviceName, ref deviceid);
            Debug.Log($"Device name {deviceName}, device id {deviceid}");
        }
    }

    private void OnPlayAudioFrame(AudioFrame audioFrame)
    {
        Debug.Log($"buffer.Length {audioFrame.buffer.Length}");
        Debug.Log($"bytesPerSample {audioFrame.bytesPerSample}");
        Debug.Log($"channels {audioFrame.channels}");
        Debug.Log($"samples {audioFrame.samples}");
        Debug.Log($"samplesPerSec {audioFrame.samplesPerSec}");
        Debug.Log($"type {audioFrame.type}");

        mRtcEngine.PushAudioFrame(audioFrame);
        
    }

    private void OnRecordAudioFrameHandler(AudioFrame audioFrame)
    {
        Debug.Log(audioFrame.buffer.Length);
    }

    public void Join(string channel)
    {
        Debug.Log("calling join (channel = " + channel + ")");

        if (mRtcEngine == null)
            return;

        mChannel = channel;

        // set callbacks (optional)
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccess;
        mRtcEngine.OnUserJoined = OnUserJoined;
        // Calling virtual setup function
        PrepareToJoin();

        // join channel
        mRtcEngine.JoinChannel(channel, null, 0);

        Debug.Log("initializeEngine done");
    }

    public void Leave()
    {
        Debug.Log("calling leave");

        if (mRtcEngine == null)
            return;

        // leave channel
        mRtcEngine.LeaveChannel();
        // deregister video frame observers in native-c code
        mRtcEngine.DisableVideoObserver();
    }

    public void LoadEngine(string appId)
    {
        mRtcEngine = IRtcEngine.GetEngine(appId);

        mRtcEngine.OnError = (code, msg) =>
        {
            Debug.LogErrorFormat("RTC Error:{0}, msg:{1}", code, IRtcEngine.GetErrorDescription(code));
        };

        mRtcEngine.OnWarning = (code, msg) =>
        {
            Debug.LogWarningFormat("RTC Warning:{0}, msg:{1}", code, IRtcEngine.GetErrorDescription(code));
        };

        // mRtcEngine.SetLogFile(logFilepath);
        // enable log
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
        SetAudioDevice();
    }

    public void UnloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
        }
    }

    protected virtual void PrepareToJoin()
    {
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // enable video
        mRtcEngine.EnableVideo();
        // allow camera output callback
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.SetVideoProfile(VIDEO_PROFILE_TYPE.VIDEO_PROFILE_LANDSCAPE_720P_3);
        mRtcEngine.SetExternalVideoSource(true, false);
        
    }
}
