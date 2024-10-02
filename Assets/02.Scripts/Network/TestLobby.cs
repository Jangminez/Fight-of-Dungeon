using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class TestLobby : MonoBehaviour
{
    Lobby hostLobby;
    float heartbeatTimer;

    [Header("MyUI")]
    public Text roomCode;

    private async void Start()
    {
        // 코드 비동기 실행
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        // 익명 로그인
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    void Update()
    {
        HandleLobbyHeartBeat();
    }

    private async void HandleLobbyHeartBeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 2;

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            hostLobby = lobby;
            roomCode.text = lobby.LobbyCode;

            Debug.Log("Create Lobby! " + lobby.LobbyCode);
        }

        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder> {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }

        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }

        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);

            Debug.Log("Joined Lobby with code " + lobbyCode);
        }
        
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
