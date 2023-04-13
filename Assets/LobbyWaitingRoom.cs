using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyWaitingRoom : MonoBehaviour {


    public static LobbyWaitingRoom Instance { get; private set; }


    
    //[SerializeField] private Transform container;
    //[SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private Button leaveButton;
    [SerializeField] private Button StartButton;
    


    private void Awake() {
        Instance = this;

      //  playerSingleTemplate.gameObject.SetActive(false);

        leaveButton.onClick.AddListener(() => {
            TestLobby.Instance.LeaveLobby();
            Hide();
            LobbySetup.Instance.Show();

        });

        StartButton.onClick.AddListener(() => {
            TestLobby.Instance.StartGame();
            Hide();
        });
    }

    private void Start() {
        TestLobby.Instance.OnJoinedLobby += UpdateLobby_Event;
        TestLobby.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
       
        

        Hide();
    }


    private void UpdateLobby_Event(object sender, TestLobby.LobbyEventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        UpdateLobby(TestLobby.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby) {
        if(RelayManager.Instance.getInGame() == true){
            Hide();
            InGame.Instance.Show();
        }
        
        if(lobby != null){

            if(lobby.Players.Count < TestLobby.Instance.GetMinPlayers()){
                DisableButton();
            }else{
                EnableButton();
            }

            if(lobby.HostId == AuthenticationService.Instance.PlayerId){
                StartButton.gameObject.SetActive(true);
            }else{
                StartButton.gameObject.SetActive(false);
            }

            lobbyNameText.text = "Lobby Name: " + lobby.Name;
            playerCountText.text = lobby.Players.Count + "/"+ + TestLobby.Instance.GetMinPlayers() + " players needed";

        }
        

        
     
    }


    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void DisableButton () {
       StartButton.interactable = false;
    }

    public void EnableButton () {
       StartButton.interactable = true;
    }

}