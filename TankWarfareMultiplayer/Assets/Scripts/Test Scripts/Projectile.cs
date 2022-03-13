using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float detonateTime = 0;
    public float radius = 1;

    public LayerMask terrainLayer;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(TickBomb());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TickBomb()
    {
      Invoke("Detonate", detonateTime);
       while(true)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
    }

    void Detonate()
    {
        TerrainDestroyer.instance.DestroyTerrain(transform.position, radius);

        Destroy(gameObject);
    }

}
