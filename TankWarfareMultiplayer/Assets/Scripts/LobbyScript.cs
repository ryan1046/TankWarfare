using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;


public class LobbyScript : NetworkBehaviour
{

    //public GameObject BalancePrefab;

    //public GameObject SpeedPrefab;

   // public GameObject HeavyPrefab;

    public GameObject Map1;
    public GameObject Map2;
    public GameObject Map3;

    public GameObject myTank;
    public GameObject myPlayer;

    // Start is called before the first frame update
    void Start()
    {
       // myPlayer = Instantiate(PlayerPrefab);

       // if (SceneManager.GetActiveScene().name == "Scene_Lobby")
       // {
        //    NetworkServer.Spawn(myPlayer, connectionToClient);
       // }
    }

    // Update is called once per frame
    void Update()
    {
        if(isServer)
        {
            Map1.SetActive(true);
            Map2.SetActive(true);
            Map3.SetActive(true);
        }
    }


    public void ChangeBalanceTank()
    {
        changeTank(1);
    }

    public void ChangeSpeedTank()
    {
        changeTank(2);
    }

    public void ChangeHeavyTank()
    {
        changeTank(3);
    }

    public void ChangeFloatMap()
    {
        changeMap(1);
    }

    public void ChangeDesertMap()
    {
        changeMap(2);
    }

    public void ChangeIceMap()
    {
        changeMap(3);
    }



    //[Command(requiresAuthority = false)]
    //[Server]
    //  [Command(requiresAuthority = false)]
    //[Server]
    //[Command]
    void changeTank(int i)
    {
       PlayerLobby[] Players = GameObject.FindObjectsOfType<PlayerLobby>();
        //myTank = Instantiate(TankPrefab);
        //myTank = TankPrefab;
      // Debug.Log(myTank);
        foreach (PlayerLobby player in Players)
        {
            if (player.hasAuthority)
            {
                player.ChangeTank(i);
            }

        }
    }

     void changeMap(int i)
    {
        PlayerLobby[] Players = GameObject.FindObjectsOfType<PlayerLobby>();
        //myTank = Instantiate(TankPrefab);
        //myTank = TankPrefab;
        // Debug.Log(myTank);
        foreach (PlayerLobby player in Players)
        {
            if (player.hasAuthority)
            {
                player.ChangeMap(i);
            }

        }
    }



}
