using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;

/*
 
    ChatBehaviour

    Controls the behaviour of the real-time chatbox

*/
public class ChatBehaviour : NetworkBehaviour
{

    public string username;
    public int maxMessages = 25;
     public Color playerMessage, info;
    
    [SerializeField] private GameObject chatPanel, textObject;
    [SerializeField] private TMPro.TMP_InputField chatBox;
    [SerializeField] List<Message> messageList = new List<Message>();
 

    
    private void Start() {
        GameBehavior.Instance.OnTookTurn += UpdateTurn_Event; // 
    }

    private void UpdateTurn_Event(object sender, EventArgs e) {
        UpdateTurn();
    }

    /*
        Only the guessers can type into the chat box
    */
    private void UpdateTurn() {
        if(PlayerList.Instance.getIsArtist()){
            chatBox.interactable = false;
        }else{
            chatBox.interactable = true;
        }
    }


    private void Awake(){
        username = Authenticate.Instance.GetPlayerName();
    }


    private void Update()
    {
        HandleChat();
  
    }

    /*
        Checks whether the user entered the secret word. If they guessed correctly, award points. Otherwise, send 
        their message to the other players
    */
    private void HandleChat(){

        if(chatBox.text != "")
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
            
                // User guessed the secret word. Award points to the guesser
                if(chatBox.text.Equals(GameBehavior.Instance.getSecretWord(),StringComparison.OrdinalIgnoreCase)){
                    if(PlayerList.Instance.getGuessedCorrect() == false){
                        PlayerList.Instance.addPoints(2);
                        PlayerList.Instance.setGuessedCorrect(true);
                        GameBehavior.Instance.incNumGuessed();
                        SendChatMessageServerRpc(username + " guessed the word!", Message.MessageType.info, NetworkManager.Singleton.LocalClientId);
                        chatBox.text = "";
                    }
                    // User already guessed the secret word. Don't award points
                    else{
                        Message newMessage = new Message();

                        newMessage.text = "You already guessed the word!";

                        GameObject  newText = Instantiate(textObject, chatPanel.transform);
                        newMessage.textObject = newText.GetComponent<TMPro.TextMeshProUGUI>();
                        newMessage.textObject.text = newMessage.text;
                        newMessage.textObject.color = MessageTypeColor(Message.MessageType.info);

                        messageList.Add(newMessage);
                    }

                // User did not enter in the secret word. Send their message to the other players
                }else{
                    SendChatMessageServerRpc(username + ": "+chatBox.text, Message.MessageType.playerMessage, NetworkManager.Singleton.LocalClientId);
                    chatBox.text = "";
                }
                
            
            } //end of if return pressed
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

    }

    /*
        Sets the color of the chat message based on the messageType.
        Messages announcing a player guessed correctly are in green, 
        and all other chat messages are black.

        Parameters:
            messageType - the type of message being sent

        Precondition:
            messageType is either info (for announcing a player guessed correctly) 
            or playerMessage (for any other chat message)

    */
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

     /*
        Adds the user's message to the chatbox for all players to see

    */
    private void AddMessage(string text,  Message.MessageType messageType, ulong senderPlayerId) {
        
        // Too many messages. Remove the oldest one.
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

