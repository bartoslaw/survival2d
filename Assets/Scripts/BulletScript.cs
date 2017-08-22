using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public enum Direction {
		LEFT,
		RIGHT
	}

	private float speed = 7.0f;
	private Rigidbody2D rigidBody;
	private bool isAlive = false;
	private Vector3 lastVelocity;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();	
	}
	
	// Update is called once per frame
	void Update () {
		if (isAlive) {
			rigidBody.velocity = lastVelocity;
		} 
	}

	public void Shoot(Direction direction) {
		isAlive = true;
		lastVelocity = new Vector3 (direction == Direction.LEFT ? -speed : speed, 0.0f, 0.0f);
		Invoke ("killDaBullet", 2.0f);
	}

	public void killDaBullet() {
		isAlive = false;
		Destroy (transform.gameObject);
	}
}
