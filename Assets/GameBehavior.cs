using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class GameBehavior : NetworkBehaviour
{
    public event EventHandler OnTookTurn;
    [SerializeField] private TextMeshProUGUI WordToDrawText;
    bool startedGame = false;
    ulong[] m_Players;
    int numPlayers;
    string secretWord;
    int numGuessed;

    private int artistIndex;
    
    public static GameBehavior Instance { get; private set; }

    public int getNumGuessed(){
        return numGuessed;
    }

    public int getNumPlayers(){
        return numPlayers;
    }

    public string getSecretWord(){
        return secretWord;
    }
    
    private void Awake()
    {
        Instance = this;
        artistIndex = 0;
        
    }

    public void incNumGuessed(){
        UpdateNumGuessedServerRpc(numGuessed + 1, NetworkManager.Singleton.LocalClientId);
    }


    [ServerRpc(RequireOwnership = false)]
    private void   UpdateNumGuessedServerRpc(int numGuessed, ulong senderPlayerId) {
        ReceiveNumGuessedUpdateClientRpc(numGuessed,  senderPlayerId);
        
    }

    [ClientRpc]
    private void ReceiveNumGuessedUpdateClientRpc( int numGuessed, ulong senderPlayerId) {
        UpdateNumGuessed(numGuessed, senderPlayerId);
        
    }
   
    private void UpdateNumGuessed(int numGuessed, ulong senderPlayerId) {
        this.numGuessed = numGuessed;
        if(RelayManager.Instance.getClientId() == m_Players[artistIndex]){
            PlayerList.Instance.addPoints(1);


            if (numGuessed >= numPlayers-1){
                TakeTurn();
            }
        }
       
    }



    public void TakeTurn()
    {
        if (numPlayers >= 1){
            
            int newIndex = artistIndex + 1;
            if(newIndex >= numPlayers){
                newIndex = 0;
                
            }
            string newWord = WordBank.Instance.GetRandomWord("easy");
            UpdateTurnServerRpc(newIndex, newWord, NetworkManager.Singleton.LocalClientId);
           

        }
        
     
       
    }

    [ServerRpc(RequireOwnership = false)]
    private void  UpdateTurnServerRpc(int newIndex, string newWord, ulong senderPlayerId) {
        ReceiveTurnUpdateClientRpc(newIndex, newWord,  senderPlayerId);
        
    }

    [ClientRpc]
    private void ReceiveTurnUpdateClientRpc( int newIndex, string newWord, ulong senderPlayerId) {
        UpdateTurn(newIndex, newWord,  senderPlayerId);
        
    }
   
    private void UpdateTurn(int newIndex, string newWord, ulong senderPlayerId) {
        
        //pick a new secret word
        secretWord = newWord; 
        
        PlayerList.Instance.setGuessedCorrect(false);
        
        // choose a new artist
        if(RelayManager.Instance.getClientId() == m_Players[newIndex]){
            PlayerList.Instance.setIsArtist(true);
            WordToDrawText.text = "Draw a "+ secretWord;
            //WordToDrawText.gameObject.SetActive(true);

        }else{
            PlayerList.Instance.setIsArtist(false);
            //WordToDrawText.gameObject.SetActive(false);
             WordToDrawText.text = "You're the guesser!";
        }

        // clear the drawing board
        PixelArtDrawingSystem.Instance.clearGrid(); 
        artistIndex = newIndex; 

        //reset the number of correct guesses
        numGuessed = 0;

        //reset the timer
        Timer.Instance.StartTimer();

        

        OnTookTurn?.Invoke(this, EventArgs.Empty);
    }
   
    public void UpdateList(ulong[] pArray, int arrLength) {
        
        UpdateListServerRpc(pArray, arrLength, NetworkManager.Singleton.LocalClientId);
        
    }
    

    [ServerRpc(RequireOwnership = false)]
    private void  UpdateListServerRpc( ulong[] pArray, int arrLength, ulong senderPlayerId) {
        ReceiveListUpdateClientRpc(pArray, arrLength, senderPlayerId);
        
    }

    [ClientRpc]
    private void ReceiveListUpdateClientRpc(  ulong[] pArray, int arrLength, ulong senderPlayerId) {
        UpdateList(pArray, arrLength,  senderPlayerId);
        
    }
   
    private void UpdateList(ulong[] pArray, int arrLength, ulong senderPlayerId) {
        m_Players = pArray;
        numPlayers = arrLength;

        Debug.Log("My list");
        for(int i = 0; i < numPlayers; i++)
        {
            Debug.Log(m_Players[i]);
        }

        if(numPlayers >=  TestLobby.Instance.GetJoinedLobby().Players.Count&& TestLobby.Instance.IsLobbyHost() && startedGame == false){
            startedGame = true;
            TakeTurn();
            
        }
      
    }
   


}
