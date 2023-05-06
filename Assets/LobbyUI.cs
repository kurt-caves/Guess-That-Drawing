using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {


    public static LobbyUI Instance { get; private set; }


    
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
            LobbyManager.Instance.LeaveLobby();
        });

        StartButton.onClick.AddListener(() => {
            LobbyManager.Instance.StartGame();
        });
    }

    private void Start() {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        //TestLobby.Instance.OnLeftLobby += TestLobby_OnLeftLobby;
      //  TestLobby.Instance.OnKickedFromLobby += TestLobby_OnLeftLobby;

        Hide();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e) {
        //ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby) {
    
        lobbyNameText.text = "Lobby Name: " + lobby.Name;
        playerCountText.text = "Number of players: " + lobby.Players.Count;
       
        Show();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

}