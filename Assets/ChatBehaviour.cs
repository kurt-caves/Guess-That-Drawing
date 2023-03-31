using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ChatBehaviour : NetworkBehaviour
{

    public string username;

    public int maxMessages = 25;
    
    [SerializeField] private GameObject chatPanel, textObject;
    [SerializeField] private TMPro.TMP_InputField chatBox;


    public Color playerMessage, info;

    [SerializeField] List<Message> messageList = new List<Message>();
 

    private void Update()
    {
    	username = Authenticate.Instance.GetPlayerName();
       
        if(chatBox.text != "")
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SendChatMessageServerRpc(username + ": "+chatBox.text, Message.MessageType.playerMessage, NetworkManager.Singleton.LocalClientId);
                chatBox.text = "";
            }
        }
        else
        {
            if(!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return) )
                chatBox.ActivateInputField();
        }
        if(!chatBox.isFocused)
        {
            if(Input.GetKeyDown(KeyCode.Space)){
               // SendChatMessageServerRpc("You pressed the space key!", Message.MessageType.info, NetworkManager.Singleton.LocalClientId);
              //  Debug.Log("Space");
            }
        }

    }


    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;
        
        switch(messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;

        }
        return color;
    }

    private void AddMessage(string text,  Message.MessageType messageType, ulong senderPlayerId) {
        
        if(messageList.Count >= maxMessages){
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }



        Message newMessage = new Message();

        newMessage.text = text;

        GameObject  newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TMPro.TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
        
        
       
     
            
        
    }

    [ClientRpc]
    private void ReceiveChatMessageClientRpc(string message,  Message.MessageType messageType, ulong senderPlayerId) {
        AddMessage(message, messageType,  senderPlayerId);
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(string message,  Message.MessageType messageType, ulong senderPlayerId) {
        ReceiveChatMessageClientRpc(message, messageType,  senderPlayerId);
    }
}



//[System.Serializable]
public class Message
{
    public string text;
    public TMPro.TextMeshProUGUI textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info
    }

}

