using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame updat
    StreamingLibrary.Streaming streaming;

    [SerializeField]
    private Dropdown m_videoDropdown;
    [SerializeField]
    private Camera m_camera;
    private bool _started = false;
    private JoinInfoBase m_joinInfoBase;

    public static string Name = "huilo";
    public static string ChannelId = "123";
    public static string AppId = "OnixApp";
    public static string SharedKey = "684b55f4fab34b7abd47b8210660a9635798a0560cb949a0a61129c7ec4f3658";
    public static string Gateway = "http://localhost:8080/sync";
    public static bool AudioOnly = false;
    public static bool ReceiveOnly = false;
    public static bool Simulcast = false;
    public static bool CaptureScreen = false;
    public static bool CaptureWithUnityCamera = true;
    public static Mode Mode = Mode.Sfu;

    void Start()
    {
        m_joinInfoBase = new JoinInfoBase();
        m_joinInfoBase.Name = Name;
        m_joinInfoBase.ChannelId = ChannelId;
        m_joinInfoBase.AppId = AppId;
        m_joinInfoBase.SharedKey = SharedKey;
        m_joinInfoBase.Gateway = Gateway;
        m_joinInfoBase.AudioOnly = AudioOnly;
        m_joinInfoBase.ReceiveOnly = ReceiveOnly;
        m_joinInfoBase.Simulcast = Simulcast;
        m_joinInfoBase.CaptureScreen = CaptureScreen;
        m_joinInfoBase.CaptureWithUnityCamera = CaptureWithUnityCamera;
        m_joinInfoBase.Mode = Mode.Sfu;
         streaming = new StreamingLibrary.Streaming();
    }

    public void StartStreamAction()
    {
        streaming.SetCamera(m_camera);
        streaming.StartStream(this, m_joinInfoBase);
        _started = true;
    }
    
    public void SourceInputHandler()
    {
        streaming.AddVideoSourceInputsDropdown(m_videoDropdown);
    }

    // Update is called once per frame
    void Update()
    {
        if (_started)
        {
            streaming.Update();
        }

    }

    private void LateUpdate()
    {
        if (_started)
        {
            streaming.LateUpdate();
        }
    }
}
