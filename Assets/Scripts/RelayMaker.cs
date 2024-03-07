using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Threading.Tasks;
using Unity.Services.Lobbies;

public class RelayMaker : MonoBehaviour
{
    public static RelayMaker Instance { get; private set; }
    private void Awake() { Instance = this; }

    //FOR SINGLEPLAYER CAMPAIGN
    public async Task<string> CreateLocalRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(0);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            Debug.Log("Successfully created a lobby.");
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    //IF NO SERVERS EXIST, MAKE ONE.
    public async Task<string> CreateServerRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(7);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartServer();

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void LeaveRelay()
    {
        try
        {
            LobbyManager.Instance.LeaveLobby();

            AuthenticationService.Instance.SignOut();
            NetworkManager.Singleton.Shutdown();
        }
        catch(RelayServiceException e)
        {

        }
    }

    private void OnApplicationQuit()
    {
        LeaveRelay();
    }
}
