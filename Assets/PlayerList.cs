using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

/*
 
    PlayerList

    Sets and displays relevant information about each player including: username, 
    points, and whether the user is a guesser or artist.

*/
public class PlayerList : NetworkBehaviour
{

    
    [SerializeField] private GameObject playerPanel, textObject;
    
    [SerializeField] List<PlayerObj> playerList = new List<PlayerObj>();

    public static PlayerList Instance { get; private set; }

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

    [ClientRpc]
    private void ReceivePlayerUpdateClientRpc(string name, int points, bool isArtist, ulong senderPlayerId) {
        UpdatePlayer(name, points, isArtist,  senderPlayerId);
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerServerRpc(string name, int points, bool isArtist, ulong senderPlayerId) {
        ReceivePlayerUpdateClientRpc(name, points, isArtist,  senderPlayerId);
    }


    /*
        Displays the name, number of points, and role of each player (artist or guesser)
    */
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
  

}


public class PlayerObj
{
    public string text;
    public TMPro.TextMeshProUGUI textObject;
    
}

