using StreamingLibrary;

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
    [SerializeField]
    private MeshRenderer m_objectRenderer;
    [SerializeField]
    private InputField m_chatInput;
    [SerializeField]
    private InputField m_userName;
    [SerializeField]
    private InputField m_channelName;
    [SerializeField]
    private Text m_outputText;
    [SerializeField]
    private Toggle m_isBroadcaster;
    private bool _started = false;
    private JoinInfoBase m_joinInfoBase;
    [SerializeField]
    private GameObject m_messagePrefab;
    [SerializeField]
    private Transform m_chatContainer;
    [SerializeField]
    private Material m_displayMaterial;

    public static string Name = "vasya";
    public static string ChannelId = "123";
    public static string AppId = "StreamingApp";
    public static string SharedKey = "61cbc74ee19a42d3a926797309166829335d16c1379a4d8f96421886c7ca8e10";
    public static string Gateway = "http://10.10.150.68:8080/sync";
    public static bool AudioOnly = false;
    public static bool ReceiveOnly = false;
    public static bool Simulcast = false;
    public static bool CaptureScreen = false;
    public static bool CaptureWithUnityCamera = true;
    public static Mode Mode = Mode.Sfu;
    private object LockObject;


    void Start()
    {
        streaming = new StreamingLibrary.Streaming();
    }

    public void StartStreamAction()
    {
        m_joinInfoBase = new JoinInfoBase();
        m_joinInfoBase.Name = m_userName.text;
        m_joinInfoBase.ChannelId = m_channelName.text;
        Debug.Log($"chat id {m_channelName.text}");
        m_joinInfoBase.AppId = AppId;
        m_joinInfoBase.SharedKey = SharedKey;
        m_joinInfoBase.Gateway = Gateway;
        m_joinInfoBase.AudioOnly = AudioOnly;
        m_joinInfoBase.ReceiveOnly = ReceiveOnly;
        m_joinInfoBase.Simulcast = Simulcast;
        m_joinInfoBase.CaptureScreen = CaptureScreen;
        m_joinInfoBase.CaptureWithUnityCamera = CaptureWithUnityCamera;
        m_joinInfoBase.Mode = Mode.Sfu;
        m_joinInfoBase.IsBroadcaster = m_isBroadcaster.isOn;

        //streaming.AddOnReceivedMessageHandler(AddMessageToChat);
        //streaming.RemoveOnReceivedMessageHandler(AddMessageToChat);
        //StreamingLibrary.Streaming.OnMessageReceived += AddMessageToChat;
        streaming.OnMessageSended += AddMessageToChat;
        streaming.OnMessageReceived += AddMessageToChat;
        streaming.SetMaterialToDisplay(m_displayMaterial);
        streaming.SetCamera(m_camera);
        streaming.SetConnectionInfo(m_joinInfoBase);
        streaming.SetMonoBehaviourProxy(this);
        streaming.StartStream();
        _started = true;
    }

    public void SendMessageToChat()
    {
        streaming.SendChatMessage(m_chatInput.text);
    }
    
    private void AddMessageToChat(MessageData messageData)
    {
        Debug.Log($"message received on unity client {messageData.Message}");
        try
        {          
            GameObject messageObj = Instantiate(m_messagePrefab, m_chatContainer);
            var messageUI = messageObj.GetComponent<MessageUI>();
            Debug.Log($"messageui {messageObj.name}");
            messageUI.SetMessageUI(messageData);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    [ContextMenu("Create som F")]
    public void AddMessageToChat()
    {

        Debug.Log($"message received on unity client AAAAAAA");
        //GameObject messageObj = Instantiate(m_messagePrefab, m_chatContainer);

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
    public void Leave()
    {
        streaming.Leave();
    }
    private void LateUpdate()
    {
        if (_started)
        {
            streaming.LateUpdate();
        }
    }
}
