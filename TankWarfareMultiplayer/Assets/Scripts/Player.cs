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


    [SyncVar]
    public GameObject BalancePrefab;

    [SyncVar]
    public GameObject SpeedPrefab;

    [SyncVar]
    public GameObject HeavyPrefab;

    [SyncVar]
    public int tankNum = 1;

    public GameObject myTank;


    public GameObject currentTank;

    [SyncVar]
    public int mapNum;



    [Tooltip("Diagnostic flag indicating whether this player is ready for the game to begin")]
    [SyncVar]
    public bool readyToBegin;

    [SyncVar]
    public Transform SpawnLocation;


    [SyncVar]
    public int score;

    [SyncVar]
    public int playerNum;

    
    [Command(requiresAuthority = false)]
    public void ChangeTank(GameObject newTankPrefab)
    {
        TankPrefab = newTankPrefab;
    }

    [Command]
    public void setPlayer(int num)
    {
        playerNum = num;
    }



    
    public void SpawnLoc(Transform Location)
    {
        SpawnLocation = Location;
    }


    //[Command]
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

        //NetworkManager.GetStartPosition();
        if (SpawnLocation == null)
        {
            SpawnLoc(NetworkManager.singleton.GetStartPosition());
        }
        SpawnMyTank();
        myTank = Instantiate(currentTank, SpawnLocation.transform.position, Quaternion.Euler(0, 0, 0));


        //myTank = Instantiate(currentTank);
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


    public void SpawnMyTank() //Changes the prefab depending on what tank they selected
    {
        if(tankNum == 1)
        {
            currentTank = BalancePrefab;
        }
        if (tankNum == 2)
        {
            currentTank = SpeedPrefab;
        }
        if (tankNum == 3)
        {
            currentTank = HeavyPrefab;
        }
    }



    [Command(requiresAuthority = false)]
    public void ChangeMyTank( int i)
    {
      
        tankNum = i;
    }


    [Command(requiresAuthority = false)]
    public void ChangeMyMap(int i)
    {

       mapNum = i;
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
