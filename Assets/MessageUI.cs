using StreamingLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text message;

    public void SetMessageUI(MessageData messageData)
    {
        Debug.Log("message data setted");
        name.text = messageData.Sender;
        message.text = messageData.Message;
    }
}
