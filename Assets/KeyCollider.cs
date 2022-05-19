using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyCollider : MonoBehaviour
{
    private PlayerMovement playerStats;
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if(other.gameObject.name == "Locks" && playerStats.hasKey == true)
        {
            other.gameObject.GetComponent<Tilemap>().SetTile(other.gameObject.GetComponent<Tilemap>().WorldToCell(playerStats.movePoint.position + new Vector3(0, 1, 0)), null);
            other.gameObject.GetComponent<Tilemap>().SetTile(other.gameObject.GetComponent<Tilemap>().WorldToCell(playerStats.movePoint.position + new Vector3(1, 0, 0)), null);
            other.gameObject.GetComponent<Tilemap>().SetTile(other.gameObject.GetComponent<Tilemap>().WorldToCell(playerStats.movePoint.position + new Vector3(0, -1, 0)), null);
            other.gameObject.GetComponent<Tilemap>().SetTile(other.gameObject.GetComponent<Tilemap>().WorldToCell(playerStats.movePoint.position + new Vector3(-1, 0, 0)), null);
        }
    }
    
}
