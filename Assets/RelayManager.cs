using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;


public class RelayManager : MonoBehaviour
{
   
    public static RelayManager Instance { get; private set; }

    private bool _inGame = false;
   
    public event EventHandler OnLeftGame;
    public event EventHandler OnUpdatedPlayerList;

    private int arrLength = 0;
    private ulong[] pArray;
    
    

     public enum ConnectionStatus
    {
        Connected,
        Disconnected
    }

    
    public event Action<ulong, ConnectionStatus> OnClientConnectionNotification;


    private void OnClientConnectedCallback(ulong clientId)
    {
        OnClientConnectionNotification?.Invoke(clientId, ConnectionStatus.Connected);
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        OnClientConnectionNotification?.Invoke(clientId, ConnectionStatus.Disconnected);
    }


    private void Awake() {
        Instance = this;
        pArray = new ulong[TestLobby.Instance.GetMaxPlayers()];
        for (int i = 0; i < pArray.Length; i++ ) {
            pArray[i] = 1000000;
         }
    }

    public void Start()
    {
       
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

        NetworkManager.Singleton.LogLevel = LogLevel.Developer;
        NetworkManager.Singleton.NetworkConfig.EnableNetworkLogs = true;
        
    }

    public void OnDestroy()  {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }
    

    public async Task<string> CreateRelay() {

        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(TestLobby.Instance.GetMaxPlayers()); //Create allocation. host + 3 players 

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); //Get join code

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            
            _inGame = true;
            

            return joinCode;
        } catch(RelayServiceException e) {
            Debug.Log(e);
            return null;
        }
       
    }

    public async void JoinRelay(string joinCode) {
        try{

            
            Debug.Log("Joining relay with "+ joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

             RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); 
            NetworkManager.Singleton.StartClient();
     
            _inGame = true;

        } catch (RelayServiceException e){
            Debug.Log(e);
        }
        
    }

    public bool getInGame(){
        return _inGame;
    }

    
    //need to handle players disconnected
    public void OnClientDisconnected(ulong clientId){
        
       // m_Players.Remove(clientId);
       TestLobby.Instance.LeaveLobby();
        NetworkManager.Singleton.Shutdown();
        _inGame = false;
        OnLeftGame?.Invoke(this, EventArgs.Empty);
        /*
        if((NetworkManager.Singleton.LocalClientId == clientId)){
            TestLobby.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
           
        }
        
        if(GameBehavior.Instance.getNumPlayers() < TestLobby.Instance.GetMinPlayers()){
            TestLobby.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            _inGame = false;
            OnLeftGame?.Invoke(this, EventArgs.Empty);
        }
        else{
            
            if(TestLobby.Instance.IsLobbyHost()){
                arrLength --;
                GameBehavior.Instance.UpdateList(pArray, arrLength);
            }
            
        }
        */
        

        


    }
    
    
    private void OnClientConnected(ulong clientId){
        Debug.Log("Player connected with client ID {"+clientId+"}");
        
        
        if(TestLobby.Instance.IsLobbyHost()){
            pArray[arrLength] = clientId;
            arrLength ++;
            GameBehavior.Instance.UpdateList(pArray, arrLength);
            
            
        
        }

        OnUpdatedPlayerList?.Invoke(this, EventArgs.Empty);
        

    }

    
    public ulong getClientId(){
        return NetworkManager.Singleton.LocalClientId;
    }
    

    
 /*
    public static void RegisterPlayer(ulong clientId)
    {
        m_Players.Add(clientId);

    }
*/
/*
    public List<ulong> GetPlayerList(){
        return m_Players;
    }
    */
    //added
    /*
    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }

    *   /


    //added
    /*
    public void Cleanup()
    {
        if (NetworkManager.Singleton != null)
        {   
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }
*/

    

   
}
