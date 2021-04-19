using StreamingLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ReceivingTest : MonoBehaviour
{
    #region Editor variables
    [SerializeField]
    private InputField _chatInput;
    [SerializeField]
    private InputField _userNameInput;
    [SerializeField]
    private InputField _channelNameInput;
    [SerializeField]
    private ChatController _chatController;
    [SerializeField]
    private Button _startStreamButton;
    [SerializeField]
    private Button _leaveStreamButton;
    [SerializeField]
    private Button _sendMessageButton;
    [SerializeField]
    private Dropdown _audioSelect;
    [SerializeField]
    private Material _materialForDisplay;
    #endregion

    #region Private variables
    private Receiving _receiving;
    private int _videoHeight = 0; //Height of captured data. For now FullHD is recomended for perfomance reasons.
    private int _videoWidth = 0; //Width of captured data. For now FullHD is recomended for perfomance reasons.
    private int _videoBitrate = 6000; //Prefered Bitrate value. This approximate value and it depends on general perfomance.
    private int _videoFPS = 30; //Prefered FPS value. This approximate value and it depends on general perfomance.
    private int _audioBitrate = 3000; //Prefered Audio bitrate value. This approximate value and it depends on general perfomance.
    #endregion

    #region Private merhods

    #region Monobehaviour methods
    private void OnEnable()
    {
        _startStreamButton.onClick.AddListener(JoinStream);
        _leaveStreamButton.onClick.AddListener(Leave);
        _sendMessageButton.onClick.AddListener(SendMessageToChat);
    }

    private void OnDisable()
    {
        if (_receiving != null)
        {
            Leave();
            _receiving.Dispose();
        }
    }
    #endregion

    private void JoinStream()
    {
        if (!string.IsNullOrEmpty(_userNameInput.text) && !string.IsNullOrEmpty(_channelNameInput.text))
        {
            StreamingLibrary.AudioSettings audioSettings = new StreamingLibrary.AudioSettings();
            ReceivingServiceSettings receivingServiceSettings = new ReceivingServiceSettings(_materialForDisplay, audioSettings);
            receivingServiceSettings.ReceiveOwnMessages = true;
            if (_receiving == null)
            {
                _receiving = new Receiving(receivingServiceSettings); //Streaming service initialization with prefered settings
            }

            //Callback that is invoked every time the streaming channel got a message
            _receiving.OnMessageReceived += _chatController.AddReceivedMessageToChat;
            //Method for starting stream. Please, start stream only with valid username and channel name
            _receiving.JoinChannel(_userNameInput.text, _channelNameInput.text);
        }
    }

    private void SendMessageToChat()
    {
        _receiving.SendMainChannelMessage(_chatInput.text); //Send string message to main data channel
        _chatInput.text = string.Empty;
    }

    //Leave streaming channel. Please, leave streaming channel before exit playmode
    private void Leave()
    {
        _receiving.Leave();
        _receiving.OnMessageReceived -= _chatController.AddReceivedMessageToChat;
    }
    #endregion
}
