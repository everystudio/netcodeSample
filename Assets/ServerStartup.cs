using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Multiplay;
using Unity.Services.Matchmaker;

public class ServerStartup : MonoBehaviour
{
    //private string externalConnetionString => $"{externalServerIP}:{serverPort}";
    private IServerQueryHandler serverQueryHandler;

    async private void Start()
    {
        bool isServer = false;
        ushort serverPort = 7777;
        string externalServerIP = "0.0.0.0";

        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-dedicatedServer")
            {
                isServer = true;
            }
            else if (args[i] == "-port" && i + 1 < args.Length)
            {
                serverPort = ushort.Parse(args[i + 1]);
            }
            else if (args[i] == "-ip" && i + 1 < args.Length)
            {
                externalServerIP = args[i + 1];
            }
        }
        if (isServer)
        {
            StartServer(externalServerIP, serverPort);
            await StartServerServices();
        }
    }

    private void StartServer(string ip, ushort port)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port);
        NetworkManager.Singleton.StartServer();
    }

    async Task StartServerServices()
    {
        await UnityServices.InitializeAsync();
        try
        {
            // ここはサーバーにアクセス出来るプレイヤーの合計数を指定
            // 現在は仮として2人を設定してますが、ゲームによって変更する必要あり
            ushort maxPlayers = 2;
            serverQueryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(maxPlayers, "n/a", "n/a", "0", "n/a");
        }
        catch (UnityException e)
        {
            Debug.LogError(e.Message);
        }
    }
    private void Update()
    {
        if (serverQueryHandler != null)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                serverQueryHandler.CurrentPlayers = (ushort)NetworkManager.Singleton.ConnectedClientsIds.Count;
            }
            serverQueryHandler.UpdateServerCheck();
        }

    }

}
