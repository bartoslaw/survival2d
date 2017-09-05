using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	private float speed = 15.0f;
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
		Invoke ("killTheBullet", 2.0f);
	}

	public void killTheBullet() {
		isAlive = false;
		Destroy (transform.gameObject);
	}
}
