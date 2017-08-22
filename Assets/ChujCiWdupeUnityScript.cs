using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChujCiWdupeUnityScript : MonoBehaviour {
	private const string IS_WALKING = "isWalking";
	private const string IS_SHOOTING = "isShooting";


	private Animator animator;
	private float speed = 2.3f;
	private float moveEpsilon = 0.1f;
	private Rigidbody2D rigidBody;
	private Vector3 lastVelocity;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> (); 	
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			animator.SetBool (IS_SHOOTING, true);
			Invoke("stopShooting", 0.5f);
			return;
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			animator.SetBool (IS_WALKING, true);
			GetComponent<SpriteRenderer> ().flipX = false;
			lastVelocity = new Vector3 (-speed, 0.0f, 0.0f);
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			animator.SetBool (IS_WALKING, true);
			GetComponent<SpriteRenderer> ().flipX = true;
			lastVelocity = new Vector3 (speed, 0.0f, 0.0f);
		} else if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
			animator.SetBool (IS_WALKING, false);
			lastVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
		}

		print (lastVelocity);
		rigidBody.velocity = lastVelocity;
	}

	private void stopShooting() {
		animator.SetBool (IS_SHOOTING, false);
	}
}
