using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Mirror;


public class GameManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //StartNewMatch();
        //Note: Start() runs before anyone connects to a server
    }

    [SyncVar]
    float _TimeLeft = 3;
    public float TimeLeft
    {
        get { return _TimeLeft; }
        set { _TimeLeft = value; }
    }


    [SyncVar]
    int _Round = 1;
    public int Round
    {
        get { return _Round; }
        set { _Round = value; }
    }


    [SyncVar]
    string _PlayerPhase = null;
    public string PlayerPhase
    {
        get { return _PlayerPhase; }
        set { _PlayerPhase = value; }
    }

    //public static GameManager Instance;

    public enum TURNSTATE { MOVE, SHOOT, RESOLVE}

    [SyncVar]
    TURNSTATE _TurnState;
    public TURNSTATE TurnState
    {
        get { return _TurnState; }
        protected set { _TurnState = value; }
    }

    public int TurnNumber { get; protected set; }

    [SyncVar, System.NonSerialized]
    bool matchHasStarted = false;

    


    [SyncVar, System.NonSerialized]
    public bool matchHasFinished = false;

    bool haveFireBullets = false;
    bool bulletsHaveSpawned = false;
    List<GameObject> activeResolutionsObjects;

    
    Queue<GameObject> eventQueue;

    
    GameObject currentEvent;
    public GameObject NextGame;

    public GameObject NewTurnAnimationPrefab;
    
    public GameObject terrain;


  


    // Update is called once per frame
    void Update()
    {
      
        if (isServer == false)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Escape))
        {

            matchHasStarted = false;
            matchHasFinished = false;
            TimeLeft = 3;
        }

            Tank[] tanks = GetAllTanks();
        if (tanks.Length != 2)
        {
            if (matchHasStarted == false)
            {

                return;
            }
            matchHasFinished = true;
            if (Round == 3)
            {

                Debug.Log("GAME OVER");
                if (Input.GetKey(KeyCode.Return))
                 {
                    NetworkManager.singleton.StopHost();
                    NetworkManager.singleton.StopClient();
                }
            }
            if (Input.GetKey(KeyCode.Return))
            {
                /*  Debug.Log("New Game");
                  matchHasStarted = false;
                  TimeLeft = 3;
                 // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                 if(isLocalPlayer & isServer)
                  {
                      NetworkManager.singleton.StopHost();
                  }
                 else
                  {
                      NetworkManager.singleton.StopHost();
                      NetworkManager.singleton.StopClient();
                  }
                
                if (matchHasStarted == false)
                {
                    return;
                }
                matchHasFinshed = true;
                */
                Debug.Log("New Game");
                ResetGame();
            }
       // }
            return;
            
            
        }
       
      


        if(ProcessEvent())
        {
            return;
        }


        TimeLeft -= Time.deltaTime;

        if (matchHasStarted == false )
        {
            if (TimeLeft > 0)
            {
                return;
            }
            else
            {
                StartNewMatch();
            }
        }

        if( TurnState == TURNSTATE.RESOLVE)
        {
            if( ProcessResolvePhase() == false)
            {
                SwapTurn();
                AdvanceTurnPhase();
            }
        }
        else
        {
            if(TimeLeft <=0 || IsPhaseLocked())
            {
                AdvanceTurnPhase();
            }
        }
        

        if (tanks[0].tankTurn == true)
        {
            UIPlayerTurn(1);
        }
        if (tanks[1].tankTurn == true)
        {
            UIPlayerTurn(0);
        }


        /* if ((TurnState != TURNSTATE.RESOLVE && (TimeLeft <= 0 || IsPhaseLocked())) /* TurnState == TURNSTATE.RESOLVE && ResolvePhaseIsCompleted()  )
         {
             AdvanceTurnPhase();
         }
         */
    }

    static public GameManager Instance()
    {
        return GameObject.FindObjectOfType<GameManager>();
    }

    public void EnqueueEvent( GameObject go)
    {
        if(eventQueue == null)
        {
            eventQueue = new Queue<GameObject>();
        }
        go.SetActive(false);
        eventQueue.Enqueue(go);
    }


    public bool IsProcessingEvent()
    {
        if (currentEvent == null)
        {
            return false; 
        }
        return true;
    }


    bool ProcessEvent()
    {
        if(currentEvent!= null)
        {
            return true; ;
        }
        if(eventQueue == null || eventQueue.Count ==0 )
        {
            return false; 
        }
        currentEvent = eventQueue.Dequeue();
        currentEvent.SetActive(true);
        return true;
    }
   




    public void RegisterResolutionObject( GameObject o)
    {
       
        activeResolutionsObjects.Add(o);
    }

    public void UnregisterResolutionObject( GameObject o)
    {
        activeResolutionsObjects.Remove(o);
    }



    bool ProcessResolvePhase()
    {

        Tank[] tanks = GetAllTanks();
        int num = 0;

        if(haveFireBullets == false)
        {
            activeResolutionsObjects = new List<GameObject>();
            bulletsHaveSpawned = false; 
            foreach (Tank tank in tanks)
            {
                num++;
                Debug.Log("tanks:"+num);
                tank.Fire();
            }
            haveFireBullets = true;

           
        }
        if (activeResolutionsObjects.Count > 0 )
        {
            bulletsHaveSpawned = true;
        }

      //Debug.Log(activeResolutionsObjects.Count);

        if (bulletsHaveSpawned && activeResolutionsObjects.Count ==0)
        {
            Debug.Log(activeResolutionsObjects.Count);
            return false;
        }
        return true;

    }


    public void ResetGame()
    {
        //Application.LoadLevel(Application.loadedLevel);
        // SceneManager.LoadScene("Game");
        //Destroy(gameObject);
        Round += 1;
         terrain.GetComponent<TerrainDestroyer>().RestartMap();
         Player[] players = GameObject.FindObjectsOfType<Player>();
         Tank[] tanks = GetAllTanks();
         foreach (Tank tank in tanks)
         {
             tank.DestroyTank();
         }
         matchHasStarted = false;
         matchHasFinished = false;
         TimeLeft = 3;
        /*
         foreach (Player player in players)
         {
             player.DestroyTank();
             player.SpawnTank();
         }
        */

        players[1].DestroyTank();
        players[0].DestroyTank();
        players[1].SpawnTank();

       
        players[0].SpawnTank();





    }





    public bool TankCanMove(Tank tank)
    {
        return matchHasStarted == true && TurnState == TURNSTATE.MOVE;
    }

    public bool TankCanShoot(Tank tank)
    {
        return matchHasStarted == true && TurnState == TURNSTATE.SHOOT;
    }



    void StartNewMatch()
    {
        matchHasStarted = true;
        TurnNumber = 0;
     
        //tanks[0].transform.position= new Vector3(-16, 9, 0); 
        // tanks[1].transform.position = new Vector3(29, 4, 0);
     
        Player[] players = GetAllPlayer();
        players[0].setPlayer(1);
        players[1].setPlayer(2);

       /*foreach (Player player in players)
        {
         
            player.SpawnTank();
        }
       */
       
        Tank[] tanks = GetAllTanks();
        tanks[1].ChangePosition(new Vector3(-16, 11, 0));
        tanks[0].ChangePosition(new Vector3(29, 5, 0));
     

        tanks[0].tankTurn = false;
        
        tanks[1].tankTurn = true;
        



        StartNewTurn();
    }

    void StartNewTurn()
    {
        TurnNumber++;
        TurnState = TURNSTATE.MOVE;
        TimeLeft = 10;
        haveFireBullets = false;
        Debug.Log("Starting Turn: " + TurnNumber);

        

        GameObject ntgo = Instantiate(NewTurnAnimationPrefab);
        EnqueueEvent(ntgo);
    }

    void SwapTurn()
    {
        Tank[] tanks = GetAllTanks();
        tanks[0].CmdChangeTurn();
        tanks[1].CmdChangeTurn();

    }



    void AdvanceTurnPhase()
    {
        switch (TurnState)
        {
            case TURNSTATE.MOVE: 
                TurnState = TURNSTATE.SHOOT;
                TimeLeft = 10;
                break;
            case TURNSTATE.SHOOT:
                TurnState = TURNSTATE.RESOLVE;
                TimeLeft = 0;
                break;
            case TURNSTATE.RESOLVE:
                StartNewTurn(); //fire bullets         
                break;
            default:
                Debug.LogError("UNKNOWN TURNSTATE!!");
                break;
        }
     
        Debug.Log("New Phase Started: " + TurnState.ToString());


        Tank[] tanks = GetAllTanks();
        foreach ( Tank tank in tanks)
        {
            tank.RpcNewPhase();
        }
    }


    void UIPlayerTurn(int num)
    {
        num = num + 1;
      //GameObject pt_go = GameObject.Find("Phase");
        PlayerPhase= "Player " + num + ":" + TurnState.ToString();
      //pt_go.GetComponent<UnityEngine.UI.Text>().text = "Player " + num+1 + ":" + TurnState.ToString();

    }

    Tank[] GetAllTanks()
    {
        return GameObject.FindObjectsOfType<Tank>();
    }

    public Player[] GetAllPlayer()
    {
        return GameObject.FindObjectsOfType<Player>();
    }

    bool IsPhaseLocked()
    {
        Tank[] tanks = GetAllTanks();

        if (tanks == null || tanks.Length == 0)
        {
            Debug.Log("No tanks");
            return false;
        }

        foreach (Tank tank in tanks)
        {
            if (tank.tankTurn == true)
            {
                if (tank.TankIsLockedIn == false)
                {
                    return false;
                }
            }
        }


        return true;
    }


}
