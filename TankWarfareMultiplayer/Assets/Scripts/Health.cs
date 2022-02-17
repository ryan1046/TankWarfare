using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(isServer)
        {
            Hitpoints = 100;
        }    
    }


    [SyncVar]
    float Hitpoints;

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetHitpoints()
    {
        return Hitpoints;
    }





    [Command(requiresAuthority = false)] //THIS IS FUCKING BAD NEEDS BE CHANGE BUT WORKS FOR NOW!!!!!!!!!!!
    public void CmdChangeHealth(float amount)
    {
        Hitpoints += amount;

        //Checks if the use at zero hp
        if (Hitpoints <= 0)
        {
            Die();
        }
    }



    void Die()
    {
        if( isServer == false)
        {
            Debug.Log("Client Called Die");
            return;
        }


        Debug.Log("DEAD");
        Destroy(gameObject);
    }


}
