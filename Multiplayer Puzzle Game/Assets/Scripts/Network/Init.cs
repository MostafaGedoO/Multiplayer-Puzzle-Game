using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Matchmaker.Models;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();

        if (UnityServices.State != ServicesInitializationState.Initialized) return;

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        if (!AuthenticationService.Instance.IsSignedIn) return;

        string _userName = PlayerPrefs.GetString("Username");

        if (_userName == "")
        {
            _userName = "Player" + Random.Range(1, 1000);
            PlayerPrefs.SetString("Username", _userName);
        }

        SceneManager.LoadSceneAsync("Main Menu");
    }
}
