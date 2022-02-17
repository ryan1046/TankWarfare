using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;



public class HealthUI : NetworkBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

     
            text = GetComponent<Text>();
        
       
    }

 
    Text text;
   
    Health health;

    // Update is called once per frame
    void Update()
    {
        
        if (health == null)
        {

           /* if (Tank.LocalTank != null)
            { 
               health = Tank.LocalTank.GetComponentInParent<Health>();
           }*/

            if (GetComponentInParent<Tank>() != null)
            {
                health = GetComponentInParent<Tank>().GetComponentInParent<Health>();
            }
            if(GetComponentInParent<Tank>() == null)
            {
                health = Tank.LocalTank.GetComponent<Health>();
            }

            if (health == null)
            {

                text.text = "dead";
                return;
            }
        }
        
        text.text = health.GetHitpoints().ToString("0");
    }
}
