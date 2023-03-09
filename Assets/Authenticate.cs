using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authenticate : MonoBehaviour {


    [SerializeField] private Button AuthenticateButton;


    private void Awake() {
        AuthenticateButton.onClick.AddListener(() => {
            TestLobby.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
            Hide();
            LobbySetup.Instance.Show();
        });
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
