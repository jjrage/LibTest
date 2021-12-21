using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudienceController : MonoBehaviour
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
    private RawImage _localImage;
    private AudienceClient _app = null;
    private bool _joined = false;


    // Start is called before the first frame update
    void Start()
    {
        _app = new AudienceClient();
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

    private void StartStream()
    {
        _app.LoadEngine(_appId);
        _app.Join(_channelName.text);
    }
}
