using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (isServer == true)
        {
            SpawnTank();
        }
    }

    public GameObject TankPrefab;
       public GameObject myTank;

    public int playerNum;



    public void setPlayer(int num)
    {
        playerNum = num;
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


        myTank = Instantiate(TankPrefab);

         NetworkServer.Spawn(myTank, connectionToClient);
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
