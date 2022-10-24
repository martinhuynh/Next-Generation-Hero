using UnityEngine;
using System.Collections;

/* Should go to A-F points in order or randomly.
 * Randomly spawn on the screen and go in random direction.
 */
public class EnemyBehavior : MonoBehaviour {

	public EnemySpawner enemySpawner;
	private int startHP = 4;
	private int currentHP;
	private float[] opacity;
	private static int destroyed = 0;
	private static int touched = 0;
	public float mSpeed = 20f;
	private float rotationSpeed = 150f;
	public GameObject nextPoint;
	private Vector2 mousePos;
	
	// Use this for initialization
	void Start () {
		currentHP = startHP;
		NewDirection();
		calculateOpactiy();
		//Debug.Log(nextPoint.transform.position.x + " " + nextPoint.transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {

		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
        GlobalBehavior globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();

        GlobalBehavior.WorldBoundStatus status =
            globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);

        if (status != GlobalBehavior.WorldBoundStatus.Inside)
        {
            //Debug.Log("collided position: " + this.transform.position);
            NewDirection();
        }
        mousePos = nextPoint.transform.position;	
	}

	void FixedUpdate()
	{
		Vector2 lookDirection = mousePos - new Vector2(transform.position.x, transform.position.y);
		float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
		Quaternion qTo = Quaternion.Euler(new Vector3(0, 0, angle));
		transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, rotationSpeed * Time.deltaTime);
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

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Egg"))
        {
			Destroy(collision.gameObject);
			GlobalBehavior.sTheGlobalBehavior.DestroyAnEgg();
			decHP();
        } else if (collision.gameObject.name.Contains("Hero"))
        {
			GlobalBehavior.sTheGlobalBehavior.UpdateTouchedEnemies(++touched);
			GlobalBehavior.sTheGlobalBehavior.UpdateDestroyedEnemies(destroyed + touched);
			respawn();
        } else if (collision.gameObject.name.Equals(nextPoint.gameObject.name))
        {
			//Debug.Log(nextPoint.gameObject.name);
			nextPoint = PointSpawner.getNextPoint(nextPoint);
			//Debug.Log("next " + nextPoint.gameObject.name);
		}
    }

	private void decHP()
	{
		currentHP--;
		if (currentHP <= 0)
		{
			respawn();
			GlobalBehavior.sTheGlobalBehavior.UpdateDestroyedEnemies(++destroyed + touched);
		}
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity[currentHP - 1]);
	}

	private void respawn()
	{
		currentHP = startHP;
		//int xMag = 45;
		//int yMag = 25;
		//Vector3 objPosition = new Vector3(Random.Range(xMag * -1, xMag), + Random.Range(yMag * -1, yMag), 0);
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity[currentHP - 1]);
		transform.position = enemySpawner.getRandomLocation();
	}

	// New direction will be something completely random!
	private void NewDirection() {
		Vector2 v = Random.insideUnitCircle;
		transform.up = new Vector3(v.x, v.y, 0.0f);
	}
}
