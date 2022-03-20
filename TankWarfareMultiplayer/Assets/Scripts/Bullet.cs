using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isServer)
        {
            GameManager.Instance().RegisterResolutionObject(gameObject);
        }
    }




    public float armTime = 3;
    Rigidbody2D rb;

    public float Radius = 2f;
    public float Damage = 10f;
    public bool DamageFallsOff = true;
    public Tank SourceTank;

    public float LifeSpan = 5;

    public bool RotatesWithVelocity = true;

    public GameObject ExplosionPrefab;


    // Update is called once per frame
    void Update()
    {
        if (RotatesWithVelocity == true)
        { 
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        }

     

        if ( isServer == true)
        {
            armTime -= 1;
            LifeSpan -= Time.deltaTime;
            if(LifeSpan <= 0)
            {
                Die();
            }
        }

    }



    //[ClientRpc]
    void RpcDoExplosion(Vector2 position)
    {
       GameObject go = Instantiate(ExplosionPrefab, position, Quaternion.identity);
        // go.GetComponent<BulletExplosion>().Radius = Radius;
        NetworkServer.Spawn(go);

    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);

    }



    void OnTriggerEnter2D(Collider2D collider)
    {
        if(isServer == false || armTime > 0)
        {
            return;
        }

        if (GetComponent<Rigidbody2D>() == collider.attachedRigidbody)
        {
            return;
        }

        Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, Radius);
        foreach(Collider2D col in cols)
        {
            if( col.attachedRigidbody == null)
            {
                continue;
            }


            Health h = col.attachedRigidbody.GetComponent<Health>();

            if (h != null)
            {
                Debug.Log(col.attachedRigidbody.gameObject.name);
               
                h.CmdChangeHealth(-Damage);
               
            }
        }
       
        RpcDoExplosion(this.transform.position);
        //terrainDestroyer.instance.DestroyTerrain(this.transform.position, 2);
       
            DestroyTerrain(this.transform.position, 2);
        
       
            RPCDestroyTerrain(this.transform.position, 2);
        
        // Destroy(gameObject);
        Die();
    }

    void DestroyTerrain(Vector3 location, int radius)
    {
        TerrainDestroyer.instance.DestroyTerrain(location, radius);
    }

    [ClientRpc]
    void RPCDestroyTerrain(Vector3 location, int radius)
    {
        TerrainDestroyer.instance.DestroyTerrain(location, radius);
    }

      void Die()
    {
        // Should Die() be the thing that triggers the damage explosion?
        // i.e. if we time-out in midair (or during bouncing)

        if (isServer)
        {
            GameManager.Instance().UnregisterResolutionObject(gameObject);
        }
        

        // Remove ourselves from the game
        Destroy(gameObject);

    }


}
