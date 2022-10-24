using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    private static List<GameObject> points = new List<GameObject>();
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
    public GameObject E;
    public GameObject F;
    private bool isVisible = true;

    // Start is called before the first frame update
    void Start()
    {
        points.Add(A);
        points.Add(B);
        points.Add(C);
        points.Add(D);
        points.Add(E);
        points.Add(F);
        Debug.Log("Points size: " + points.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            isVisible = !isVisible;
            GlobalBehavior.sTheGlobalBehavior.UpdateShowWayPoints(isVisible);
            foreach (GameObject i in points)
            {
                i.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (isVisible) ? 1f : 0f);
            }
        }
    }

    public static GameObject getNextPoint(GameObject current)
    {
        int index = -1;
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].gameObject.name.Equals(current.gameObject.name))
            {
                index = i;
                break;
            }
        }
        //Debug.Log("index " + index);
        // Randomly chooses a different point that is not current.
        if (EnemySpawner.random)
        {
            int randomVal ;
            while ((randomVal = Random.Range(0, points.Count)) == index) ;
            Debug.Log("randomval " + randomVal);
            return points[randomVal];
        }
        
        GameObject next = points[0];
        if (index < points.Count - 1) next = points[index + 1];
        return next;
    }

    public static GameObject getFirstPoint()
    {
        return (points.Count != 0) ? points[0] : null;  // Null if points empty.
    }
}
