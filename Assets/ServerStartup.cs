using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStartup : MonoBehaviour
{
    private bool isServer = false;
    private ushort serverPort = 7777;
    private string externalServerIP = "0.0.0.0";
    private string externalConnetionString => $"{externalServerIP}:{serverPort}";

    private void Start()
    {
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
    }
}
