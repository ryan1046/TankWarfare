using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float Radius =1;
   float fadeInTime = 0.25f, fadeInTimeLeft = 0.25f;
    //float fadeInTimeLeft = fadeInTime;
    float fadeOutTime= 0.5f, fadeOutTimeLeft = 0.5f;
   // float fadeOutTimeLeft = fadeOutTime;


    public GameObject Explosion;
    //double time = 10;

    // Update is called once per frame
    void Update()
    {

        if (fadeInTimeLeft > 0)
        {
            fadeInTimeLeft = Mathf.Max(fadeInTimeLeft - Time.deltaTime, 0);
            float r = Radius * (1 - fadeInTimeLeft / fadeInTime);
            Explosion.transform.localScale = Vector3.one * r;

            return;
        }

        if (fadeOutTime > 0)
        {
            fadeOutTimeLeft = Mathf.Max(fadeOutTime - Time.deltaTime, 0);
            Color c = Explosion.GetComponent<SpriteRenderer>().color;
            c.a = (fadeOutTimeLeft / fadeOutTime);
           

            return;
        }
        //time -= Time.deltaTime;

        // Explosion.transform.localScale = Vector3.one * Radius;


        Destroy(gameObject);
        


    }
}
