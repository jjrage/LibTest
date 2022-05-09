using agora_gaming_rtc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StreamClient : IVideoChatClient
{
    public event Action OnJoinedSuccess;
    protected string mChannel;
    protected IRtcEngine mRtcEngine;
    private IAudioRawDataManager rawAudioManager;
    private Dropdown audioDeviceDropDown;
    private Dictionary<string, string> audioDevices = new Dictionary<string, string>();
    private IAudioRecordingDeviceManager recordingDeviceManager;

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

    public void SetAudioDeviceDropdown(Dropdown dropdown)
    {
        audioDeviceDropDown = dropdown;
        audioDeviceDropDown.onValueChanged.AddListener(ChangeAudioDevice);
    }

    private void ChangeAudioDevice(int arg0)
    {
        string deviceID = audioDevices.Keys.ElementAt(arg0);
        recordingDeviceManager.SetAudioRecordingDevice(deviceID);
        string name = string.Empty;
        string id = string.Empty;
        //recordingDeviceManager.GetCurrentRecordingDevice(ref name, ref id);
    }

    public void PushFrame(ExternalVideoFrame frame)
    {
        IRtcEngine rtc = IRtcEngine.QueryEngine();
        if (rtc != null)
        {
            int a = rtc.PushVideoFrame(frame);
        }
    }
    public void SetAudioDevice()
    {
        int enableAudio = mRtcEngine.EnableAudio();
        mRtcEngine.SetAudioProfile(AUDIO_PROFILE_TYPE.AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO, AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_SHOWROOM);
        mRtcEngine.MuteLocalAudioStream(false);
        rawAudioManager = mRtcEngine.GetAudioRawDataManager();
        int registerAudioRawDataObserver = rawAudioManager.RegisterAudioRawDataObserver();
        rawAudioManager.SetOnRecordAudioFrameCallback(OnPlayAudioFrame);
    }

    public void AudioCheck()
    {
        SetAudioDevice();
        recordingDeviceManager = mRtcEngine.GetAudioRecordingDeviceManager();
        recordingDeviceManager.CreateAAudioRecordingDeviceManager();

        int deviceCount = recordingDeviceManager.GetAudioRecordingDeviceCount();
        recordingDeviceManager.SetAudioRecordingDeviceMute(false);
        List<Dropdown.OptionData> audioDeviceOptionData = new List<Dropdown.OptionData>();

        for (int i = 0; i < deviceCount; i++)
        {
            string deviceName = string.Empty;
            string deviceid = string.Empty;
            recordingDeviceManager.GetAudioRecordingDevice(i, ref deviceName, ref deviceid);
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = deviceName;
            audioDeviceOptionData.Add(option);
            audioDevices.Add(deviceid, deviceName);
        }

        audioDeviceDropDown.AddOptions(audioDeviceOptionData);
    }

    private void OnVolumeIndication(AudioVolumeInfo[] speakers, int speakerNumber, int totalVolume)
    {
        foreach(var speaker in speakers)
        {
            Debug.Log($"Speaker {speaker.uid} volume {speaker.volume}");
        }
        Debug.Log($"Total volume {totalVolume}");
    }

    private void AudioRouteChanged(AUDIO_ROUTE route)
    {
        Debug.Log($"Audio route changed {route}");
    }

    private void AudioDeviceVolumeChanged(MEDIA_DEVICE_TYPE deviceType, int volume, bool muted)
    {
        Debug.Log($"Device {deviceType} volume changed {volume}");
    }

    private void OnPlayAudioFrame(AudioFrame audioFrame)
    {
        mRtcEngine.PushAudioFrame(audioFrame);     
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
        mRtcEngine.OnAudioDeviceVolumeChanged += AudioDeviceVolumeChanged;
        mRtcEngine.OnAudioRouteChanged += AudioRouteChanged;
        mRtcEngine.OnVolumeIndication += OnVolumeIndication;
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
        rawAudioManager.UnRegisterAudioRawDataObserver();
        recordingDeviceManager.ReleaseAAudioRecordingDeviceManager();
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
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.SetVideoProfile(VIDEO_PROFILE_TYPE.VIDEO_PROFILE_LANDSCAPE_720P_3);
        mRtcEngine.SetExternalVideoSource(true, false);
        AudioCheck();
    }
}
