using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using NobleConnect.Mirror;

public class Menu : MonoBehaviour
{

    public NobleNetworkManager networkManager;
    public GameObject menuPanel;


    // The relay ip and port from the GUI text box
    string hostIP = "";
    string hostPort = "";

    // Used to determine which GUI to display
    bool isHost, isClient;

    public void Host()
    {
     

        isHost = true;
        isClient = false;
     
        networkManager.StartHost();
        //Debug.Log(networkManager.HostEndPoint.Address.ToString());
       // Debug.Log(networkManager.HostEndPoint.Port.ToString());
        menuPanel.SetActive(false);
       
        //GUIHost();
    }

    public void SetIP(string ip)
    {
        networkManager.networkAddress = ip;
    }

    public void SetPort(string hostPort)
    {
        networkManager.networkPort = ushort.Parse(hostPort);
    }

    public void Join()
    {
        networkManager.StartClient();
        isHost = false;
        isClient = true;
        menuPanel.SetActive(false);
    }


    void GUIHost()
    {
        // Display host addresss
        if (networkManager.HostEndPoint != null)
        {
            GUI.Label(new Rect(10, 10, 150, 22), "Host IP:");
            GUI.TextField(new Rect(170, 10, 420, 22), networkManager.HostEndPoint.Address.ToString(), "Label");
            GUI.Label(new Rect(10, 37, 150, 22), "Host Port:");
            GUI.TextField(new Rect(170, 37, 160, 22), networkManager.HostEndPoint.Port.ToString(), "Label");
        }

        // Disconnect Button
        if (GUI.Button(new Rect(10, 81, 110, 30), "Disconnect"))
        {
            networkManager.StopHost();
            isHost = false;
        }

        if (!NobleServer.active) isHost = false;
    }


    void GUIClient()
    {
        if (!networkManager.isNetworkActive)
        {
            // Text boxes for entering host's address
            GUI.Label(new Rect(10, 10, 150, 22), "Host IP:");
            hostIP = GUI.TextField(new Rect(170, 10, 420, 22), hostIP);
            GUI.Label(new Rect(10, 37, 150, 22), "Host Port:");
            hostPort = GUI.TextField(new Rect(170, 37, 160, 22), hostPort);

            // Connect button
            if (GUI.Button(new Rect(115, 81, 120, 30), "Connect"))
            {
                networkManager.networkAddress = hostIP;
                networkManager.networkPort = ushort.Parse(hostPort);
                networkManager.StartClient();
                menuPanel.SetActive(false);
            }
            
            // Back button
            if (GUI.Button(new Rect(10, 81, 95, 30), "Back"))
            {
                isClient = false;
            }
        }
        else if (networkManager.client != null)
        {
            // Disconnect button
            GUI.Label(new Rect(10, 10, 150, 22), "Connection type: " + networkManager.client.latestConnectionType);
            if (GUI.Button(new Rect(10, 50, 110, 30), "Disconnect"))
            {
                if (networkManager.client.isConnected)
                {
                    // If we are already connected it is best to quit gracefully by sending
                    // a disconnect message to the host.
                    networkManager.client.Disconnect();
                }
                else
                {
                    // If the connection is still in progress StopClient will cancel it
                    networkManager.StopClient();
                }
                isClient = false;
            }
        }
    }


    private void OnGUI()
    {
        if (!isHost && !isClient)
        {
            // Host button
            /*if (GUI.Button(new Rect(10, 10, 100, 30), "Host"))
            {
                isHost = true;
                isClient = false;

                networkManager.StartHost();
            }
            */

            // Client button
            if (GUI.Button(new Rect(10, 50, 100, 30), "Client"))
            {
                networkManager.InitClient();
                isHost = false;
                isClient = true;
            }
        }
        else

        {
            // Host or client GUI
            if (isHost) GUIHost();
            else if (isClient) GUIClient();
        }
          
    }





    // Start is called before the first frame update
    void Start()
    {
       // networkManager = (NobleNetworkManager)NetworkManager.singleton;
        menuPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
