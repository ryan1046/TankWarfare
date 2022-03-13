using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class Player : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (isServer == true)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            // SpawnTank();
            {

            }
        }
    }

    [SyncVar]
    public GameObject TankPrefab;

    
    public GameObject myTank;


    [Tooltip("Diagnostic flag indicating whether this player is ready for the game to begin")]
    [SyncVar]
    public bool readyToBegin;


    [SyncVar]
    public int score = 0;

    [SyncVar]
    public int playerNum;

    [Command]
    public void ChangeTank(GameObject newTankPrefab)
    {
        TankPrefab = newTankPrefab;
    }

    [Command]
    public void setPlayer(int num)
    {
        playerNum = num;
    }

    public void addScore()
    {
        score += 1;
    }



    public void DestroyTank()
    {
        NetworkServer.Destroy(myTank);
    }



    public void SpawnTank()
    {
        if( isServer == false)
        {
            return;
        }


        //GameObject go = Instantiate(TankPrefab);

        // NetworkServer.Spawn(go, connectionToClient);
        //NetworkServer.Spawn(go);

        //myTank = Instantiate(TankPrefab);

        if (playerNum == 1)
        {
            myTank = Instantiate(TankPrefab, new Vector2(-16, 11), Quaternion.Euler(0, 0, 0));
        }
        if (playerNum == 2)
        {
            myTank = Instantiate(TankPrefab, new Vector2(29, 5), Quaternion.Euler(0, 0, 0));
        }



        // myTank.GetComponent<Tank>().ChangePosition(new Vector3(-8, -3, 0));

        NetworkServer.Spawn(myTank, connectionToClient);
        
       /* if (isServer)
        {
            myTank.GetComponent<Tank>().ChangePosition(new Vector3(-16, 11, 0));
        }
        else
        {
            myTank.GetComponent<Tank>().ChangePosition(new Vector3(29, 5, 0));
      
        }
       */
     
    }



    // Update is called once per frame

    void Update()
    {
        
        if (Input.GetKey(KeyCode.Escape))
        {

            if (isServer)
            {
                NetworkManager.singleton.StopHost();

            }
            NetworkManager.singleton.StopClient();


        }
        if (Input.GetKey(KeyCode.Return))
        {
          
        }
    }
   

    }
