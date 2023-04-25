using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using Unity.Netcode;

public class InGame : MonoBehaviour
{
    public static InGame Instance { get; private set; }
    

    [SerializeField] private Button WordBankButton;
    [SerializeField] private TextMeshProUGUI WordToDrawText;
    
   
    
     private void Start() {
        TestLobby.Instance.OnJoinedLobby += UpdateLobby_Event;
        TestLobby.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        RelayManager.Instance.OnLeftGame += LeaveGame_Event;
        RelayManager.Instance.OnUpdatedPlayerList +=  UpdatePlayerList_Event;

        Hide();
    }

    private void LeaveGame_Event(object sender, EventArgs e) {
        LeaveGame();
    }
    
     private void LeaveGame() {
        
        
            Hide();
            LobbySetup.Instance.Show();
       
    }

     private void UpdatePlayerList_Event(object sender, EventArgs e) {
        UpdatePlayerList();
    }
    
    private void  UpdatePlayerList() {
        PlayerList.Instance.UpdateList();
     
    }

    private void Awake()
    {
        Instance = this;
        
        WordBankButton.onClick.AddListener(() => {
           WordToDrawText.text = "Draw a "+ WordBank.Instance.GetRandomWord("easy");
        
        
     });
    }
    



     private void UpdateLobby_Event(object sender, TestLobby.LobbyEventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        UpdateLobby(TestLobby.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby) {
    
            /*
            if(lobby.Players.Count < TestLobby.Instance.GetMinPlayers() && TestLobby.Instance.GetInGame() == true){
                TestLobby.Instance.LeaveLobby();
                Debug.Log("Not enough players. Left lobby.");
                Hide();
                LobbySetup.Instance.Show();
                
            }
            */

        }  
     
    


    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

}
