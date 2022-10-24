using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private Vector2 spawnBounds;
    private float spawnBorderPercent = 0.9f;
    private int totalToSpawn = 1;
    public static bool random = false;
    public Sprite randomSprite, sequence;
    private static List<GameObject> enemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //spawnBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < 10; i++)
        {
            enemies.Add(Instantiate(enemy, getRandomLocation(), Quaternion.identity));
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            random = !random;
            foreach(GameObject i in enemies)
            {
                i.GetComponent<SpriteRenderer>().sprite = (random) ? randomSprite : sequence;
            }
            
            GlobalBehavior.sTheGlobalBehavior.UpdateNextWayPoint((random) ? "Random" : "Sequence");
        }
    }

    public Vector3 getRandomLocation()
    {
        spawnBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        bool objSpawned = false;
        Vector3 newLocation = new Vector3();
        while (!objSpawned)
        {
            float x = spawnBounds.x;
            float y = spawnBounds.y;
            Vector3 objPosition = new Vector3(Random.Range(x * -1, x), Random.Range(y * -1, y), 0);
            // Reduce range by spawnBorder (percent)
            objPosition.x *= spawnBorderPercent;
            objPosition.y *= spawnBorderPercent;
            // ensures new object is spawned far enough away
            if ((objPosition - transform.position).magnitude < 3)
            {
                continue;
            }
            else
            {
                objSpawned = true;
                newLocation = objPosition;
            }
            //Debug.Log("Loop: " + newLocation);
        }
        return newLocation;
    }
}
