using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask collidersLayer;
    public LayerMask locksLayer;
    public GameObject nextLevelSpawn;
    public int currentLevel = 1;
    public GameObject water;
    public bool hasKey = false;
    private Vector3 oldPoint;
    private bool moved = false;
    List<GameObject> generatedWater = new List<GameObject>();
    private int moveInterval = 0;
    List<int> ignoredMoves= new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameObject.Find("Level" + currentLevel + "Spawn").transform.position;
        movePoint.parent = null;
        oldPoint = movePoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (!moved && Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, collidersLayer))
                {
                    oldPoint = movePoint.position;
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    moveInterval++;
                    moved = true;
                }
            } else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, collidersLayer))
                {
                    oldPoint = movePoint.position;
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    moveInterval++;
                    moved = true;
                }
            }
        }


        if(moved && !ignoredMoves.Contains(moveInterval) && Vector3.Distance(transform.position, movePoint.position) == 0f)
        {
            generatedWater.Add(Instantiate(water, oldPoint, Quaternion.identity));
            moved = false;
        } else if (ignoredMoves.Contains(moveInterval)) {
            moved = false;
        }

        if(Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f, 0f), .2f, collidersLayer) && Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f, 0f), .2f, collidersLayer) && Physics2D.OverlapCircle(movePoint.position + new Vector3(0, 1f, 0f), .2f, collidersLayer) && Physics2D.OverlapCircle(movePoint.position + new Vector3(0, -1f, 0f), .2f, collidersLayer)){
            Debug.Log("you lost the game");
            Respawn(currentLevel);
        }
        
    }

    void FindNextLevel(int level)
    {
        transform.position = movePoint.position = GameObject.Find("Level" + level + "Spawn").transform.position;
    }

    void Respawn(int level, string respawnCase = "finishedLevel")
    {
        foreach(var obj in generatedWater)
        {
            Destroy(obj);
        }
        FindNextLevel(currentLevel);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Bridges")
        {
            moved = false;
            ignoredMoves.Add(moveInterval + 1);
            other.gameObject.GetComponent<Tilemap>().SetTile(other.gameObject.GetComponent<Tilemap>().WorldToCell(movePoint.position), null);
        }

        if(other.gameObject.name == "Goals")
        {
            currentLevel++;
            FindNextLevel(currentLevel);
        }
        if(other.gameObject.name == "Keys")
        {
            other.gameObject.GetComponent<Tilemap>().SetTile(other.gameObject.GetComponent<Tilemap>().WorldToCell(movePoint.position), null);
            hasKey = true;
        }
    }   

}
