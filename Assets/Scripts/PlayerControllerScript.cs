using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour {

	private enum State
	{
		WALKING_LEFT,
		WALKING_RIGHT,
		IDLE,
		SHOOTING
	}

	private const string IS_WALKING = "isWalking";
	private const string IS_SHOOTING = "isShooting";

	private const float bulletSpawnXPosition = 0.32f;
	private const float bulletSpawnYPosition = 0.09f;

	private Animator animator;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;
	private State currentState = State.IDLE;

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
		print (currentState);
		if (currentState == State.SHOOTING) {
			return;
		}
			
		if (Input.GetKeyDown (KeyCode.Space) && currentState != State.SHOOTING && currentState != State.WALKING_LEFT && currentState != State.WALKING_RIGHT) {
			currentState = State.SHOOTING;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			currentState = State.WALKING_LEFT;
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			currentState = State.WALKING_RIGHT;
		} else if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.Space)) {
			currentState = State.IDLE;
		}

		EvaluateState ();
		rigidBody.velocity = lastVelocity;
	}

	private void EvaluateState() {
		if (currentState == State.WALKING_LEFT) {
			Walk (KeyCode.LeftArrow);
		} else if (currentState == State.WALKING_RIGHT) {
			Walk (KeyCode.RightArrow);
		} else if (currentState == State.SHOOTING) {
			Shoot ();
		} else { //idle
			StayIdle();
		}
	}

	private void StayIdle() {
		animator.SetBool (IS_WALKING, false);
		lastVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
	}

	private void Shoot() {
		animator.SetBool (IS_SHOOTING, true);
		Invoke ("stopShooting", 0.5f);
		Instantiate (
			bulletPrefab,
			bulletSpawn
		)
		.GetComponent<BulletScript> ()
		.Shoot (!spriteRenderer.flipX ? Direction.LEFT : Direction.RIGHT);
	}

	private void Walk(KeyCode keyCode) {
		bool previousFlip = spriteRenderer.flipX;

		animator.SetBool (IS_WALKING, true);
		spriteRenderer.flipX = keyCode != KeyCode.LeftArrow;
		lastVelocity = new Vector3 (keyCode == KeyCode.LeftArrow ? -speed : speed, 0.0f, 0.0f);

		if (keyCode == KeyCode.LeftArrow) {
			bulletSpawn.localPosition = new Vector3 (-bulletSpawnXPosition, bulletSpawnYPosition, 0.0f);
		} else {
			bulletSpawn.localPosition = new Vector3 (bulletSpawnXPosition, bulletSpawnYPosition, 0.0f);
		}
	}

	private void stopShooting() {
		animator.SetBool (IS_SHOOTING, false);
		currentState = State.IDLE;
	}
}