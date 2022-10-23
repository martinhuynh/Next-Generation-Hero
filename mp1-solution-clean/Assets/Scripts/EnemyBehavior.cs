using UnityEngine;
using System.Collections;

/* Should go to A-F points in order or randomly.
 * Randomly spawn on the screen and go in random direction.
 */
public class EnemyBehavior : MonoBehaviour {

	private int startHP = 4;
	private int currentHP;
	
	public float mSpeed = 20f;
	private GameObject nextPoint = null;
	
	// Use this for initialization
	void Start () {
		NewDirection();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
		GlobalBehavior globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior>();
		
		GlobalBehavior.WorldBoundStatus status =
			globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
			
		if (status != GlobalBehavior.WorldBoundStatus.Inside) {
			Debug.Log("collided position: " + this.transform.position);
			NewDirection();
		}	
	}

	private void respawn()
	{
		currentHP = startHP;
		// relocate 15 to 15 randomly x and y.
		int xMag = 15;
		int yMag = 15;
		Vector3 objPosition = new Vector3(Random.Range(xMag * -1, xMag), + Random.Range(yMag * -1, yMag), 0);
		transform.position = objPosition;
	}

	// New direction will be something completely random!
	private void NewDirection() {
		Vector2 v = Random.insideUnitCircle;
		transform.up = new Vector3(v.x, v.y, 0.0f);
	}
}
