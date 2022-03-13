using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);

    }


    void OnTriggerEnter2D(Collider2D collider)
    {

        //Health h = col.attachedRigidbody.GetComponent<Health>();
      
            if (collider.attachedRigidbody != null)
            {
            Health h = collider.attachedRigidbody.GetComponent<Health>();

            if (h != null)
            {
                Debug.Log(collider.attachedRigidbody.gameObject.name);

                h.CmdChangeHealth(-10000000);

            }
        }
        

     
           
    }
}
