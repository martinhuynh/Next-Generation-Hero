using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBehavior : MonoBehaviour
{
    private static List<GameObject> points = new();
    private int startHP = 4;
    private int currentHP;
    private float[] opacity;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = startHP;
        calculateOpactiy();
        startPosition = transform.position;
        points.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for egg collision.
        // if currentHP 0 spawn relocate randomly and reset hp.
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collide");
        if (collision.gameObject.name.Contains("Egg")) {
            Debug.Log("Egg");
            decHP();
            // Destroy egg
            Destroy(collision.gameObject);
        }
    }

    private void decHP()
    {
        currentHP--;
        if (currentHP <= 0) respawn();
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity[currentHP - 1]);
    }

    private void calculateOpactiy()
    {
        float increments = 1f / (float)startHP;
        opacity = new float[startHP];
        for (int i = 0; i < startHP; i++)
        {
            opacity[i] = increments * (i + 1);
        }
    }

    private void respawn()
    {
        currentHP = startHP;
        // relocate 15 to 15 randomly x and y.
        int xMag = 15;
        int yMag = 15;
        Vector3 objPosition = new Vector3(startPosition.x + Random.Range(xMag * -1, xMag), startPosition.y + Random.Range(yMag * -1, yMag), 0);
        transform.position = objPosition;
    }

}
