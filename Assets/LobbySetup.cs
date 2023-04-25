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

            TestLobby.Instance.CreateLobby(
                lobbyName,
               // maxPlayers,
                isPrivate
              //gameMode
            );
            Hide();
            LobbyWaitingRoom.Instance.Show();
        });



	    QuickJoinButton.onClick.AddListener(() => {
            TestLobby.Instance.QuickJoinLobby();
            Hide();
            LobbyWaitingRoom.Instance.Show();

           
        });
        
        DisableButton();
       
    }

    private void Start() {
        TestLobby.Instance.OnLobbyListChanged += UpdateLobbyCount_Event;
        
        
       
      
        Hide();
    }
    
    private void UpdateLobbyCount_Event(object sender, TestLobby.OnLobbyListChangedEventArgs e) {
        UpdateLobbyCount();
    }

    private void UpdateLobbyCount() {
        
        if(TestLobby.Instance.GetLobbyCount() < 1){
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
        
        lobbyName = "MyLobby"+ TestLobby.Instance.GetLobbyCount();
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
