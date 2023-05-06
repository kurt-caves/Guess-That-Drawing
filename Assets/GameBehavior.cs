using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

/*
   
    GameBehaviour

    Controls the mechanics of the game
  
*/
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

    // returns the number of players who guessed correctly
    public int getNumGuessed(){
        return numGuessed;
    }

    // returns the total number of players in the lobby
    public int getNumPlayers(){
        return numPlayers;
    }

    // returns the secret word
    public string getSecretWord(){
        return secretWord;
    }
    

    private void Awake()
    {
        Instance = this;
        artistIndex = 0; 
    }

    // Increments NumGuessed when another player guesses correctly
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
   
   /*
        Share the new value for numGuessed with all other players.
        Give one point to the artist.
        If everyone guessed correctly, make someone else the artist.

   */
    private void UpdateNumGuessed(int numGuessed, ulong senderPlayerId) {
        this.numGuessed = numGuessed;
        if(RelayManager.Instance.getClientId() == m_Players[artistIndex]){
            PlayerList.Instance.addPoints(1);

            // Everyone guessed correctly. Take turn
            if (numGuessed >= numPlayers-1){
                TakeTurn();
            }
        }
       
    }


    /*
        Handles transfering who the artist is
    */
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
   

    /*
        Handles switching turns

        Parameters:
            newIndex - index of the new artist in m_Players
            newWord - the new word to draw
            senderPlayerId - Id of the player who called this method
    */
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
   
    /*
        Updates the list of player client Id's whenever a someone new joins the game
    */
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
