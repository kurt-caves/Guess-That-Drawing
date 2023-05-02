using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerList : NetworkBehaviour
{

    
    [SerializeField] private GameObject playerPanel, textObject;

    public static PlayerList Instance { get; private set; }

    [SerializeField] List<PlayerObj> playerList = new List<PlayerObj>();

 
    
    private int points;
    private string username;
    private ulong clientId;
    private bool isArtist;
    private bool guessedCorrect;

    public bool getGuessedCorrect(){
        return guessedCorrect;
    }

    public void setGuessedCorrect(bool guessed){
        guessedCorrect = guessed;
    }

    public ulong getClientId(){
        return clientId;
    }

    public void setIsArtist(bool amIArtist){
        this.isArtist = amIArtist;
        UpdatePlayerServerRpc(username, points, isArtist, NetworkManager.Singleton.LocalClientId);
    }

    public bool getIsArtist(){
        return isArtist;
    }


    public void addPoints(int numPoints){
        points = points+ numPoints;
         UpdatePlayerServerRpc(username, points, isArtist, NetworkManager.Singleton.LocalClientId);
    }

    public int getPoints(){
        return points;
    }

    private void Awake(){
        Instance = this;
       
        points = 0;
        username = Authenticate.Instance.GetPlayerName();
        clientId = RelayManager.Instance.getClientId();
        isArtist = false;

        
    }
/*
    public void AddList()
    {
        
        AddPlayerServerRpc(username, NetworkManager.Singleton.LocalClientId);
       
                
    }

*/


    [ClientRpc]
    private void ReceivePlayerUpdateClientRpc(string name, int points, bool isArtist, ulong senderPlayerId) {
        UpdatePlayer(name, points, isArtist,  senderPlayerId);
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerServerRpc(string name, int points, bool isArtist, ulong senderPlayerId) {
        ReceivePlayerUpdateClientRpc(name, points, isArtist,  senderPlayerId);
    }

    private void UpdatePlayer(string name, int points, bool isArtist, ulong senderPlayerId) {
        
        PlayerObj newPlayer = new PlayerObj();
       
       if(isArtist){
            newPlayer.text = name + " (artist) " + points;
        }else{
            newPlayer.text = name + " (guesser) " + points;
        }
      
       

        GameObject  newText = Instantiate(textObject, playerPanel.transform);
        newPlayer.textObject = newText.GetComponent<TMPro.TextMeshProUGUI>();
        newPlayer.textObject.text = newPlayer.text;
       
        playerList.Add(newPlayer);


       
        
    }
    /*
    [ClientRpc]
    private void ReceivePlayerAddClientRpc(string name, ulong senderPlayerId) {
        AddPlayer(name,  senderPlayerId);
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddPlayerServerRpc(string name, ulong senderPlayerId) {
        ReceivePlayerAddClientRpc(name,  senderPlayerId);
    }

    private void AddPlayer(string name, ulong senderPlayerId) {
        
        PlayerObj newPlayer = new PlayerObj();
       
        newPlayer.text = name;
      
       

        GameObject  newText = Instantiate(textObject, playerPanel.transform);
        newPlayer.textObject = newText.GetComponent<TMPro.TextMeshProUGUI>();
        newPlayer.textObject.text = newPlayer.text;
       
        playerList.Add(newPlayer);
       
        
    }
    */


    

   
}



//[System.Serializable]
public class PlayerObj
{
    public string text;
    public TMPro.TextMeshProUGUI textObject;
    
}

