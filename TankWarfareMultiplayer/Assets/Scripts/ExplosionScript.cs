using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        time = 1;
      

    }
    public GameObject Explosion;

    double time;

    // Update is called once per frame
    void Update()
    {


        time -= Time.deltaTime;

        if (time <= 0)
        {
            Destroy(Explosion);
        }
       


    }
}
