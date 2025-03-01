using System;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyCodeText;

    private void Start()
    {
        lobbyCodeText.text = "Lobby Code: " + GameLobbyManager.Instance.GetLobbyJoinCode();
    }
}
