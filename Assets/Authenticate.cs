using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Allows player to sign in after entering a user name
*/
public class Authenticate : MonoBehaviour {

    public static Authenticate Instance { get; private set; }

    public event EventHandler OnNameChanged;


    [SerializeField] private Button AuthenticateButton;

    [SerializeField] private TMPro.TMP_InputField nameBox;

    private string playerName;
    private int maxNameLength = 32;

    private void Update() {
        if(nameBox.text != "")
        {

            EnableButton();
        }
        else
        {
            DisableButton();
            if(!nameBox.isFocused && Input.GetKeyDown(KeyCode.Return) )
                nameBox.ActivateInputField();
        }
    }


    private void Awake() {
        Instance = this;

        nameBox.characterLimit = maxNameLength;
       
        AuthenticateButton.onClick.AddListener(() => {
            setUserName();
            LobbyManager.Instance.Authenticate(GetPlayerName());
            Hide();
            LobbySetup.Instance.Show();
        });

        
        DisableButton();
    }

    private void Start() {
        OnNameChanged += EditPlayerName_OnNameChanged;
    }
    
    private void EditPlayerName_OnNameChanged(object sender, EventArgs e) {
        LobbyManager.Instance.UpdatePlayerName(GetPlayerName());
        EnableButton();

    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public string GetPlayerName() {
        return playerName;
    }

    public void DisableButton () {
       AuthenticateButton.interactable = false;
    }

     public void EnableButton () {
       AuthenticateButton.interactable = true;
    }

    public void setUserName(){
        
        playerName = nameBox.text;
        OnNameChanged?.Invoke(this, EventArgs.Empty);
    }
     public void Show() {
        gameObject.SetActive(true);
    }
}

