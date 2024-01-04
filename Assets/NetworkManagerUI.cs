using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;

using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("34.143.223.103", 9000);
            NetworkManager.Singleton.StartClient();
        });
    }
    /*
    private void Start()
    {
        //SignIn();
    }

    private async void SignIn()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        clientButton.interactable = true;
    }
    */

}
