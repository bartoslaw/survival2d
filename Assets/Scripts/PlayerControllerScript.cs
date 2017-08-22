using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour {
	private const string IS_WALKING = "isWalking";
	private const string IS_SHOOTING = "isShooting";

	private Animator animator;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;

	private float speed = 2.3f;
	private Vector3 lastVelocity;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	void Start () {
		animator = GetComponent<Animator> (); 	
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
		
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			animator.SetBool (IS_SHOOTING, true);
			Invoke("stopShooting", 0.5f);
			Instantiate (
				bulletPrefab,
				bulletSpawn
			)
			.GetComponent<BulletScript> ()
			.Shoot (!spriteRenderer.flipX ? BulletScript.Direction.LEFT : BulletScript.Direction.RIGHT);

			return;
		}
			
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Walk (KeyCode.LeftArrow);
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Walk (KeyCode.RightArrow);
		} else if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
			animator.SetBool (IS_WALKING, false);
			lastVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
		}

		rigidBody.velocity = lastVelocity;
	}

	private void Walk(KeyCode keyCode) {
		bool previousFlip = spriteRenderer.flipX;

		animator.SetBool (IS_WALKING, true);
		spriteRenderer.flipX = keyCode != KeyCode.LeftArrow;
		lastVelocity = new Vector3 (keyCode == KeyCode.LeftArrow ? -speed : speed, 0.0f, 0.0f);
	
		if (keyCode == KeyCode.LeftArrow) {
			bulletSpawn.position = new Vector3 (-0.3f, 0.9f, 0.0f);
		} else {
			bulletSpawn.position = new Vector3 (0.3f, 0.9f, 0.0f);
		}
	}

	private void stopShooting() {
		animator.SetBool (IS_SHOOTING, false);
	}
}