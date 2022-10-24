using UnityEngine;
using System.Collections;

/* Should go to A-F points in order or randomly.
 * Randomly spawn on the screen and go in random direction.
 */
public class EnemyBehavior : MonoBehaviour {

	private int startHP = 4;
	private int currentHP;
	private float[] opacity;
	private static int destroyed = 0;
	private static int touched = 0;
	public float mSpeed = 20f;
	public GameObject nextPoint;
	
	// Use this for initialization
	void Start () {
		currentHP = startHP;
		NewDirection();
		calculateOpactiy();
		//Debug.Log(nextPoint.transform.position.x + " " + nextPoint.transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.up = new Vector3(nextPoint.transform.position.x, nextPoint.transform.position.y, 0.0f);
		//Vector3 targ = nextPoint.transform.position;
		//targ.z = 0f;

		//Vector3 objectPos = transform.position;
		//targ.x = targ.x - objectPos.x;
		//targ.y = targ.y - objectPos.y;

		//float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg * 90f;
		//transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		
		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
		Vector2 v = nextPoint.transform.position - transform.position;
		transform.up = new Vector3(v.x, v.y, 0.0f);
		GlobalBehavior globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior>();
		
		GlobalBehavior.WorldBoundStatus status =
			globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
			
		if (status != GlobalBehavior.WorldBoundStatus.Inside) {
			//Debug.Log("collided position: " + this.transform.position);
			NewDirection();
		}

		
		
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
		// relocate 15 to 15 randomly x and y.
		int xMag = 45;
		int yMag = 25;
		Vector3 objPosition = new Vector3(Random.Range(xMag * -1, xMag), + Random.Range(yMag * -1, yMag), 0);
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity[currentHP - 1]);
		transform.position = objPosition;
	}

	// New direction will be something completely random!
	private void NewDirection() {
		Vector2 v = Random.insideUnitCircle;
		transform.up = new Vector3(v.x, v.y, 0.0f);
	}
}
