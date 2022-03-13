using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Mirror;

public class PlayerLobby : NetworkBehaviour
{
    [SyncVar]
    public GameObject myTank;


    bool hasSpawned = false;

    bool winner = false;

    [SyncVar]
    public Player myPlayer;

    [SyncVar]
    public int tankNum;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((SceneManager.GetActiveScene().name == "Game"))
        {
            Player[] Players = GameObject.FindObjectsOfType<Player>();
            foreach (Player player in Players)
            {
                if (player.hasAuthority)
                {
                    changePlayer(player);
                   
                }

            }
           
            if (!hasSpawned)
            {
                if (myPlayer != null)
                {
                    if (isServer)
                    {
                        myPlayer.setPlayer(2);
                    }
                    else
                    {
                        myPlayer.setPlayer(1);
                    }
                    ChangePlayerTank();
                    myPlayer.SpawnTank();
                    hasSpawned = true;
                }
                
            }
        
        }

    }

    [Command]
    public void changePlayer(Player player)
    {
        myPlayer = player;
    }

    [Command]
    public void ChangeTank(GameObject newTank, int i)
    {
        tankNum = i;
        myTank = newTank;
    }

    [Command]
    public void ChangePlayerTank()
    {
        myPlayer.ChangeTank(myTank);
    }


}
