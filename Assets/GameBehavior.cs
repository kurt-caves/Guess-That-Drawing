using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class GameBehavior : NetworkBehaviour
{

    private  NetworkVariable<int> _artistIndex = new NetworkVariable<int>();
   /*
    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();
        _artistIndex.OnValueChanged += OnValueChanged;
    }
    // Start is called before the first frame update

    private void OnValueChanged(int myArtistIndex){
        Debug.Log("The artist is "+myArtistIndex);
    }


    void OnTakeTurn()
    {
        int newIndex = 5;
        
        List<ulong> myPlayers = RelayManager.Instance.GetPlayerList();
        int newIndex = _artistIndex;
        if(newIndex > myPlayers.Count -1){
            newIndex ++;
            
        }else{
            newIndex = 0;
        }
        
        OnTurnChangedRpc(artistIndex: newIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRpc]
    private void OnTurnChangedRpc(int artistIndex){

    }
    */

}
