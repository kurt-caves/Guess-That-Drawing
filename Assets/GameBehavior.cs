using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class GameBehavior : NetworkBehaviour
{
    /*
    private  NetworkVariable<int> _artistIndex = new NetworkVariable<int>();
    private const int  _startIndex = 0;

    public override void OnNetworkSpawn(){
        if (IsServer){
            _artistIndex.Value = _startIndex;
           
        
        }
        else{
            _artistIndex.OnValueChanged += OnSomeValueChanged;
        }
        
    }
    

    private void OnSomeValueChanged(int previous, int current){
       // Debug.Log("The artist is "+myArtistIndex);
        Debug.Log($"Detected NetworkVariable Change: Previous: {previous} | Current: {current}");
    }


    void OnTakeTurn()
    {
        int newIndex = 5;
        
        List<ulong> myPlayers = RelayManager.Instance.GetPlayerList();
        //int newIndex = _artistIndex;
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
