using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChujCiWdupeUnityScript : MonoBehaviour {
	private const string IS_WALKING = "isWalking";
	private const string IS_SHOOTING = "isShooting";


	private Animator animator;
	private float speed = 2.3f;
	private float moveEpsilon = 0.1f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> (); 	
	}

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");

		if (Input.GetKeyDown (KeyCode.Space)) {
			animator.SetBool (IS_SHOOTING, true);
			Invoke("stopShooting", 0.5f);
			return;
		}

		transform.position += new Vector3(horizontal * speed * Time.deltaTime, 0.0f, 0.0f);

		if (horizontal - moveEpsilon > 0.0f) {
			animator.SetBool (IS_WALKING, true);
			GetComponent<SpriteRenderer> ().flipX = true;
		} else if (horizontal + moveEpsilon < 0.0f) {
			animator.SetBool (IS_WALKING, true);
			GetComponent<SpriteRenderer> ().flipX = false;
		} else {
			animator.SetBool (IS_WALKING, false);
		}
	}

	private void stopShooting() {
		animator.SetBool (IS_SHOOTING, false);
	}
}
