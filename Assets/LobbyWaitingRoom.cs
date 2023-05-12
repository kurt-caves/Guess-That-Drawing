using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

/*
    LobbyWaitingRoom

    Controls the GUI for the lobby waiting room
*/
public class LobbyWaitingRoom : MonoBehaviour {


    public static LobbyWaitingRoom Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private Button leaveButton;
    [SerializeField] private Button StartButton;
    


    private void Awake() {
        Instance = this;

        // click to leave the lobby
        leaveButton.onClick.AddListener(() => {
            LobbyManager.Instance.LeaveLobby();
            Hide();
            LobbySetup.Instance.Show();

        });

        // click to start the game
        StartButton.onClick.AddListener(() => {
            LobbyManager.Instance.StartGame();
            Hide();
        });
    }

    private void Start() {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
       
        Hide();
    }


    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    /*
        Makes the "start game" button visible to the lobby host.
        Displays the name of the lobby and the number of players in it.
        Runs everytime someone joins or leaves the lobby.
    */
    private void UpdateLobby(Lobby lobby) {
        
        // hide waiting room if game already started
        if(RelayManager.Instance.getInGame() == true){
            Hide();
        }
        
        if(lobby != null){

            // can't start the game until minimum number of players joined
            if(lobby.Players.Count < LobbyManager.Instance.GetMinPlayers()){
                DisableButton();
            }else{
                EnableButton();
            }

            // only host can start the game
            if(lobby.HostId == AuthenticationService.Instance.PlayerId){
                StartButton.gameObject.SetActive(true);
            }else{
                StartButton.gameObject.SetActive(false);
            }

            // display lobby name and number of players
            lobbyNameText.text = "Lobby Name: " + lobby.Name;
            playerCountText.text = lobby.Players.Count + "/"+ + LobbyManager.Instance.GetMinPlayers() + " players needed";

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