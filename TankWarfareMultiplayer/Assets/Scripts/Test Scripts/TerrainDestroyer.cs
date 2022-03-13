using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

public class TerrainDestroyer : NetworkBehaviour
{

    public Tilemap terrain;

   
    public static TerrainDestroyer instance;

    //public static TerrainDestroyer instance;

   // [Command(requiresAuthority = false)]
    void Awake()
    {
        instance = this;
        //instance = TerrainDestroyer.FindObjectOfType<TerrainDestroyer>();
    }


    public void DestroyTerrain(Vector3 explosionLocation, float radius)
    {
        /*
        Collider2D[] cols = Physics2D.OverlapCircleAll(explosionLocation, radius);
        foreach (Collider2D col in cols)
        {        
            Vector3Int tilePos = terrain.WorldToCell(Vector3Int.FloorToInt(col.attachedRigidbody.transform.position));
            if (terrain.GetTile(tilePos) != null)
            {
                DestroyTile(tilePos);
            }

        }
        */
        
        for (int x = -(int)radius; x < radius; x++)
        {
            for (int y = -(int)radius; y < radius; y++)
            {
                Vector3Int tilePos = terrain.WorldToCell(explosionLocation + new Vector3(x, y, 0));
                if(terrain.GetTile(tilePos) != null)
                {
                    DestroyTile(tilePos);
                    RpcDestroyTile(tilePos);
                }
            }
        }
        
    }

    [Command(requiresAuthority = false)]
    void DestroyTile(Vector3Int tilePos)
    {
        terrain.SetTile(tilePos, null);
    }

    [ClientRpc]
    void RpcDestroyTile(Vector3Int tilePos)
    {
        terrain.SetTile(tilePos, null);
    }

    public void RestartMap()
    {
        terrain.RefreshAllTiles();
    }



}
