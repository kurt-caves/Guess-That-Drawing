using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerList : NetworkBehaviour
{

    
    [SerializeField] private GameObject playerPanel, textObject;

    public static PlayerList Instance { get; private set; }

    [SerializeField] List<PlayerObj> playerList = new List<PlayerObj>();
 
    
    private int points;
    private string username;
    private ulong clientId;
    private bool isArtist;


    public ulong getClientId(){
        return clientId;
    }

    public void setIsArtist(bool amIArtist){
        this.isArtist = amIArtist;
    }

    public bool getIsArtist(){
        return isArtist;
    }

    private void Awake(){
        Instance = this;
       
        points = 0;
        username = Authenticate.Instance.GetPlayerName();
        clientId = RelayManager.Instance.getClientId();
        isArtist = false;

        
    }

    public void UpdateList()
    {
        
        UpdatePlayerListServerRpc(username, NetworkManager.Singleton.LocalClientId);
                
    }


    private void UpdatePlayer(string name, ulong senderPlayerId) {
        /*
        for (int i = 0; i < playerList.Count; i++){
            Destroy(playerList[i].textObject.gameObject);
            playerList.Remove(playerList[i]);
        }
*/
        
        PlayerObj newPlayer = new PlayerObj();

        newPlayer.text = name;

        GameObject  newText = Instantiate(textObject, playerPanel.transform);
        newPlayer.textObject = newText.GetComponent<TMPro.TextMeshProUGUI>();
        newPlayer.textObject.text = newPlayer.text;
       
        playerList.Add(newPlayer);
        
        
       
     
            
        
    }

    [ClientRpc]
    private void ReceivePlayerUpdateClientRpc(string name, ulong senderPlayerId) {
        UpdatePlayer(name,  senderPlayerId);
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerListServerRpc(string name, ulong senderPlayerId) {
        ReceivePlayerUpdateClientRpc(name,  senderPlayerId);
    }
}



//[System.Serializable]
public class PlayerObj
{
    public string text;
    public TMPro.TextMeshProUGUI textObject;
    
}

