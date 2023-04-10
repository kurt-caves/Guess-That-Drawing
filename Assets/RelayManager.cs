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
    private ulong clientId;

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

    private void Update(){
        if(NetworkManager.Singleton.ShutdownInProgress){
            //TestLobby.Instance.GoBackToLobby(true);
        }
    }

    private void OnClientDisconnected(ulong clientId){
        
        if(NetworkManager.Singleton.LocalClientId == clientId){
            NetworkManager.Singleton.Shutdown();
            
        }


    }
   
    private void OnClientConnected(ulong clientId){
        Debug.Log("Player connected with client ID {"+clientId+"}");
        RegisterPlayer(clientId);

        /*
        if(NetworkManager.Singleton.hasAuthority == true){
             Debug.Log("Total number of people is " + NetworkManager.Singleton.ConnectedClients.Count);

        }
       */
       /*
        this.clientId = clientId;

        if(TestLobby.Instance.getClientId() == this.clientId){
            RegisterPlayer(TestLobby.Instance.getPlayerName());
          
        }
        */
        
        

    }

    /*
    public ulong getClientId(){
        return clientId;
    }
    */

    static List<ulong> m_Players  = new List<ulong>();
    
    public static void RegisterPlayer(ulong clientId)
    {
        m_Players.Add(clientId);

    }


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
