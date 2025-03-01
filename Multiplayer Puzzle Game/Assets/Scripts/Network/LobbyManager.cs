using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }
    
    private Lobby currentLobby;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<bool> CreateLobbyAsync(int _maxPlayers, Dictionary<string, string> _data)
    {
        Dictionary<string, PlayerDataObject> _playerData = SerializePlayerData(_data);
        Player _player = new Player(AuthenticationService.Instance.PlayerId, null, _playerData);

        CreateLobbyOptions _options = new CreateLobbyOptions()
        {
            IsPrivate = false,
            Player = _player
        };

        try
        {
            currentLobby = await LobbyService.Instance.CreateLobbyAsync("Game Lobby", _maxPlayers, _options);
        }
        catch (LobbyServiceException e)
        {
            return false;
        }

        Debug.Log("Lobby Created With ID:" + currentLobby.Id);

        StartCoroutine(HeartBeat());
        StartCoroutine(RefreshLobby());
        
        return true;
    }

    private IEnumerator HeartBeat()
    {
        WaitForSecondsRealtime _wait = new WaitForSecondsRealtime(15);

        while (true)
        {
            yield return _wait;
            LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
        }
    }  
    
    private IEnumerator RefreshLobby()
    {
        WaitForSecondsRealtime _wait = new WaitForSecondsRealtime(2);

        while (true)
        {
            yield return _wait;
            Task<Lobby> _lobbyTask = LobbyService.Instance.GetLobbyAsync(currentLobby.Id);
            yield return new WaitUntil(() => _lobbyTask.IsCompleted);
            Lobby _newLobby = _lobbyTask.Result;
            if (_newLobby.LastUpdated > currentLobby.LastUpdated)
            {
                currentLobby = _newLobby;
            }
        }
    }

    private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> _data)
    {
        Dictionary<string, PlayerDataObject> _PlayerData = new Dictionary<string, PlayerDataObject>();
        foreach (var (key, Value) in _data)
        {
            _PlayerData.Add(key,new PlayerDataObject(
                visibility: PlayerDataObject.VisibilityOptions.Member,
                value: Value
                ));
        }

        return _PlayerData;
    }

    private void OnApplicationQuit()
    {
        if (currentLobby != null)
        {
            if (currentLobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                LobbyService.Instance.DeleteLobbyAsync(currentLobby.Id);
            }
        }
    }

    public string GetLobbyJoinCode()
    {
        return currentLobby?.LobbyCode;
    }
}
