using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbySetup : MonoBehaviour {


    public static LobbySetup Instance { get; private set; }


    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button QuickJoinButton;
   
    private string lobbyName;
    private bool isPrivate;
    private int maxPlayers;
   

    private void Awake() {
        Instance = this;

        createPublicButton.onClick.AddListener(() => {

            LobbyManager.Instance.CreateLobby(
                lobbyName,
               // maxPlayers,
                isPrivate
              //gameMode
            );
            Hide();
            LobbyWaitingRoom.Instance.Show();
        });



	    QuickJoinButton.onClick.AddListener(() => {
            LobbyManager.Instance.QuickJoinLobby();
            Hide();
            LobbyWaitingRoom.Instance.Show();

           
        });
        
        DisableButton();
       
    }

    private void Start() {
        LobbyManager.Instance.OnLobbyListChanged += UpdateLobbyCount_Event;
        
        
       
      
        Hide();
    }
    
    private void UpdateLobbyCount_Event(object sender, LobbyManager.OnLobbyListChangedEventArgs e) {
        UpdateLobbyCount();
    }

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
        //maxPlayers = 6;
       // gameMode = LobbyManager.GameMode.CaptureTheFlag;

       // UpdateText();
    }

    public void DisableButton () {
       QuickJoinButton.interactable = false;
    }

     public void EnableButton () {
       QuickJoinButton.interactable = true;
    }

}
