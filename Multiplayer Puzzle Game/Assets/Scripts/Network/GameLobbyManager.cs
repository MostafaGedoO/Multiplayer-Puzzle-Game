using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameLobbyManager : MonoBehaviour
{
    public static GameLobbyManager Instance { get; private set; }

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

    public async Task<bool> CreateLobby()
    {
        Dictionary<string,string> _playerData = new Dictionary<string, string>()
        {
            {"GamerTage","HostPlayer"}
        };
        bool _succeeded = await LobbyManager.Instance.CreateLobbyAsync(4, _playerData);
        return _succeeded;
    }

    public string GetLobbyJoinCode()
    {
        return LobbyManager.Instance.GetLobbyJoinCode();
    }
}
