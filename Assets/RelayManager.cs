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

public class RelayManager : MonoBehaviour
{
    
    public static RelayManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public async Task<string> CreateRelay() {

        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); //Create allocation. host + 3 players 

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); //Get join code

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

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

        } catch (RelayServiceException e){
            Debug.Log(e);
        }
        
    }
    

   
}
