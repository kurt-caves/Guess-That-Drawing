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
    [SerializeField] private Button TakeTurnButton;
    [SerializeField] private TextMeshProUGUI WordToDrawText;
    
    ulong[] m_Players;
    int numPlayers;
    string secretWord;


    private int artistIndex;
    
    public static GameBehavior Instance { get; private set; }

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
        /*
        if(TestLobby.Instance.IsLobbyHost()){
            PlayerList.Instance.setIsArtist(true);
            TakeTurnButton.interactable = true;
        }else{
            PlayerList.Instance.setIsArtist(false);
            TakeTurnButton.interactable = false;
        }
        */
        TakeTurnButton.onClick.AddListener(() => {
            TakeTurn();
        
        });

        TakeTurn();
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
        
        secretWord = newWord;
        WordToDrawText.text = "Draw a "+ secretWord;
        PlayerList.Instance.setGuessedCorrect(false);

        if(RelayManager.Instance.getClientId() == m_Players[newIndex]){
            PlayerList.Instance.setIsArtist(true);
            TakeTurnButton.interactable = true;
            WordToDrawText.gameObject.SetActive(true);

        }else{
            PlayerList.Instance.setIsArtist(false);
            TakeTurnButton.interactable = false;
            WordToDrawText.gameObject.SetActive(false);
        }
        PixelArtDrawingSystem.Instance.clearGrid();
        artistIndex = newIndex;

        
        

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
      
    }
   


}
