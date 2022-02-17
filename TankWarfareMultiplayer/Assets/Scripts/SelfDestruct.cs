using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float Timer = 5;

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if( Timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
