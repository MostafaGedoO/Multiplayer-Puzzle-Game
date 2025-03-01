using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(async () =>
        {
            bool isSucceed = await GameLobbyManager.Instance.CreateLobby();
            if(isSucceed) SceneManager.LoadSceneAsync("Lobby");
        });
        
        joinButton.onClick.AddListener(() =>
        {
            
        });
    }
}
