using agora_gaming_rtc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StreamController : MonoBehaviour
{
    [SerializeField]
    private Button _startStream;
    [SerializeField]
    private Button _leaveStream;
    [SerializeField]
    private InputField _channelName;
    [SerializeField]
    private string _appId;
    [SerializeField]
    private RenderTexture _clientVideo;
    [SerializeField]
    private RawImage _localImage;
    private StreamClient _app = null;
    private bool _joined = false;


    // Start is called before the first frame update
    void Start()
    {
        _localImage.texture = _clientVideo;
        _app = new StreamClient();
        _app.OnJoinedSuccess += SetClientVideo;
        _startStream.onClick.AddListener(StartStream); 
        _leaveStream.onClick.AddListener(LeaveStream);
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    private void OnApplicationQuit()
    {
        _app.DisableEngine();
    }

    private void LeaveStream()
    {
        _app.Leave();
        _joined = false;
    }

    private void SetClientVideo()
    {
        _app.SetVideoSource(_clientVideo);
        _joined = true;
    }

    private void StartStream()
    {
        _app.LoadEngine(_appId);
        _app.Join(_channelName.text);
    }

    internal IEnumerator CaptureFromRenderTextureGPU(RenderTexture renderTexture)
    {
        yield return new WaitForEndOfFrame();
        AsyncGPUReadback.Request(renderTexture, 0, renderTexture.graphicsFormat, OnCompleteReadback);
    }

    void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.Log("GPU readback error detected.");
            return;
        }

        // Creates a new external video frame.
        ExternalVideoFrame externalVideoFrame = new ExternalVideoFrame();
        // Sets the buffer type of the video frame.
        externalVideoFrame.type = ExternalVideoFrame.VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
        // Sets the format of the video pixel.
        externalVideoFrame.format = ExternalVideoFrame.VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_RGBA;
        // Applies raw data.
        externalVideoFrame.buffer = request.GetData<byte>().ToArray();
        // Sets the width (pixel) of the video frame.
        externalVideoFrame.stride = (int)_clientVideo.width;
        // Sets the height (pixel) of the video frame.
        externalVideoFrame.height = (int)_clientVideo.height;
        // Removes pixels from the sides of the frame
        externalVideoFrame.cropLeft = 10;
        externalVideoFrame.cropTop = 10;
        externalVideoFrame.cropRight = 10;
        externalVideoFrame.cropBottom = 10;
        // Rotates the video frame (0, 90, 180, or 270)
        externalVideoFrame.rotation = 180;
        // Calculates the video timestamp in milliseconds according to the system time.
        externalVideoFrame.timestamp = System.DateTime.Now.Ticks / 10000;
        _app.PushFrame(externalVideoFrame);
        //CustomEventBehaviour.OnCustomRawTextureSetted?.Invoke(request.GetData<byte>());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_joined)
        {
            StartCoroutine(CaptureFromRenderTextureGPU(_clientVideo));
        }
    }
}
