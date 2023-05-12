using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
    LobbySetup

    Controls the GUI for the lobby set up
*/
public class LobbySetup : MonoBehaviour {


    public static LobbySetup Instance { get; private set; }


    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button QuickJoinButton;
   
    private string lobbyName;
    private bool isPrivate;
    private int maxPlayers;
   

    private void Awake() {
        Instance = this;

        // creates a new lobby when clicked
        createPublicButton.onClick.AddListener(() => {

            LobbyManager.Instance.CreateLobby(
                lobbyName,
                isPrivate
            );
            Hide();
            LobbyWaitingRoom.Instance.Show();
        });

        // player joins a new lobby when clicked 
	    QuickJoinButton.onClick.AddListener(() => {
            LobbyManager.Instance.QuickJoinLobby();
            Hide();
            LobbyWaitingRoom.Instance.Show();
           
        });
        
        DisableButton();
       
    }

    private void Start() {
        LobbyManager.Instance.OnLobbyListChanged += UpdateLobbyCount_Event;
        RelayManager.Instance.OnLeftGame += LeaveGame_Event;
        Hide();
    }
    
    private void LeaveGame_Event(object sender, EventArgs e) {
        LeaveGame();
    }
    
     private void LeaveGame() {
 
            Hide();
            LobbySetup.Instance.Show();
       
    }

    private void UpdateLobbyCount_Event(object sender, LobbyManager.OnLobbyListChangedEventArgs e) {
        UpdateLobbyCount();
    }

    // disables the "join lobby" button if there are no lobbies to join
    private void UpdateLobbyCount() {
        if(LobbyManager.Instance.GetLobbyCount() < 1){
            DisableButton();
        }
        else{
            EnableButton();
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
        lobbyName = "MyLobby"+ LobbyManager.Instance.GetLobbyCount();
        isPrivate = false;
    }

    public void DisableButton () {
       QuickJoinButton.interactable = false;
    }

    public void EnableButton () {
       QuickJoinButton.interactable = true;
    }

}
