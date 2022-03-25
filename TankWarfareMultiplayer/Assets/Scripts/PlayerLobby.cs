using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;


public class PlayerLobby : NetworkBehaviour
{
   // [SyncVar]
   // public GameObject myTank;

   // public GameObject currentTank;

   


    bool hasSpawned = false;

    bool winner = false;

    [SyncVar]
    public Player myPlayer;

    [SyncVar]
    public int tankNum = 1;


    [SyncVar]
    public int mapNum = 1;

    [SyncVar]
    public int score;


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
                    myPlayer.setLobbyPlayer(this);
                    myPlayer.ChangeMyTank(tankNum);
                    myPlayer.ChangeMyMap(mapNum);
                    new WaitForSeconds(10f);
                    myPlayer.SpawnTank();
                    hasSpawned = true;
                }
                
            }
        
        }

    }

    public void updateScore(int newScore)
    {
        score = newScore;
    }



    [Command]
    public void changePlayer(Player player)
    {
        myPlayer = player;
    }

    [Command]
    public void ChangeMap(int i)
    {
        mapNum = i;
    }



    //[Server]
    // [Command(requiresAuthority = false)]
    // [Server]
    [Command(requiresAuthority = false)]
    public void ChangeTank( int i)
    {
        //Debug.Log(newTank + "playerlobby");
        tankNum = i;
       //currentTank = newTank;
       // myTank = newTank;
       // TankChange(currentTank);
       // RPCTankChange(currentTank);
    }

    // [Server]
    [Command(requiresAuthority = false)]
    public void TankChange(GameObject newTank)
    {
      //  currentTank = newTank;
    }


    [ClientRpc]
    public void RPCTankChange(GameObject newTank)
    {
       // currentTank = newTank;

    }



    [Command(requiresAuthority = false)]
    public void ChangePlayerTank()
    {
      //  myPlayer.ChangeTank(myTank);
    }


}
