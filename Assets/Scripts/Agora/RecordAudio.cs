using FMOD;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RecordAudio : MonoBehaviour
{
    //public variables
    [Header("Choose A Microphone")]
    public int InputDeviceIndex = 0;
    [TextArea] public string InputDeviceName = null;
    [Header("How Long In Seconds Before Recording Plays")]
    public float Latency = 1f;
    [Header("Choose A Key To Play/Pause/Add Reverb To Recording")]
    public KeyCode PlayAndPause;
    public KeyCode ReverbOnOffSwitch;

    //FMOD Objects
    private FMOD.Sound sound;
    private FMOD.CREATESOUNDEXINFO exinfo;
    private FMOD.Channel channel;
    private FMOD.ChannelGroup channelGroup;

    //How many recording devices are plugged in for us to use.
    private int numOfDriversConnected = 0;
    private int numofDrivers = 0;

    //Info about the device we're recording with.
    private System.Guid InputGUID;
    private int SampleRate = 0;
    private FMOD.SPEAKERMODE FMODSpeakerMode;
    private int NumOfChannels = 0;
    private FMOD.DRIVER_STATE driverState;

    //Other variables.
    private bool dspEnabled = false;
    private bool playOrPause = true;
    private bool playOkay = false;
  

    void Start()
    {
        RuntimeManager.CoreSystem.getDriverInfo(InputDeviceIndex, out InputDeviceName, 50,
            out InputGUID, out SampleRate, out FMODSpeakerMode, out NumOfChannels);

        exinfo.cbsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
        exinfo.numchannels = NumOfChannels;
        exinfo.format = FMOD.SOUND_FORMAT.PCM16;
        exinfo.defaultfrequency = SampleRate;
        exinfo.length = (uint)SampleRate * sizeof(short) * (uint)NumOfChannels;

        RuntimeManager.CoreSystem.createStream("testStream", FMOD.MODE.CREATESTREAM | FMOD.MODE.OPENUSER, ref exinfo ,out sound);
        DSP dsp;
        RuntimeManager.CoreSystem.createDSPByType(DSP_TYPE.CHANNELMIX, out dsp);
        DSP_DESCRIPTION dspd = new DSP_DESCRIPTION();
        
        //RuntimeManager.CoreSystem.createSound(exinfo.userdata, FMOD.MODE.LOOP_NORMAL | FMOD.MODE.OPENUSER,
    }

    private void GetAudioData()
    {
        IntPtr data = new IntPtr();
        uint read, lenght;
        sound.getLength(out lenght, FMOD.TIMEUNIT.PCM);
        var result = sound.readData(data, lenght, out read);
    }

    void Update()
    {
        if (!playOrPause)
        {
            GetAudioData();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(Latency);
        //RuntimeManager.CoreSystem.playSound(sound, channelGroup, true, out channel);
        playOkay = true;
        Debug.Log("Ready To Play");
        SOUND_TYPE type;
        SOUND_FORMAT format;
        int channels;
        int bits;
        sound.getFormat(out type, out format, out channels, out bits);
        Debug.Log($"Sound format {type}, {format},{channels},{bits}");
    }

}
