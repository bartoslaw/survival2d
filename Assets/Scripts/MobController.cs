using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour {
	private const string IS_WALKING = "isWalking";
	private const string IS_ATTACKING = "isAttacking";

	private const float idleTime = 1.0f;
	private const float walkingTime = 0.5f;

	private Animator animator;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;

	private float speed = 2.3f;
	private Vector3 lastVelocity;
	private bool wasPlayerSeen = false;
	private Direction directionToPlayer;
	private Direction currentDirection;

	private bool startedWalking = false;
	private bool isIdle = false;
	private bool shouldBeIdle = true;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> (); 	
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		if (animator.GetBool (IS_ATTACKING)) {
			Attack ();
			return;
		}

		if (wasPlayerSeen) {
			WalkToPlayer();
		} else {
			if (shouldBeIdle) {
				StayIdle ();
			} else {
				Walk ();
			}
		}
	}

	void Attack() {

	}

	void Walk() {
		if (!startedWalking) {
			isIdle = false;
			startedWalking = true;
			animator.SetBool (IS_WALKING, true);

			int randomValue = Random.Range (0, 10);
			currentDirection = randomValue < 5 ? Direction.LEFT : Direction.RIGHT;

			spriteRenderer.flipX = currentDirection != Direction.LEFT;
			lastVelocity = new Vector3 (currentDirection == Direction.LEFT ? -speed : speed, 0.0f, 0.0f);

			Invoke ("ToggleIdleState", walkingTime);
		}

		rigidBody.velocity = lastVelocity;
	}

	void StayIdle() {
		if (!isIdle) {
			isIdle = true;
			startedWalking = false;
			animator.SetBool (IS_WALKING, false);

			Invoke ("ToggleIdleState", idleTime);
		}

		rigidBody.velocity = Vector3.zero;
	}

	void WalkToPlayer() {
	}

	void ToggleIdleState() {
		shouldBeIdle = !shouldBeIdle;
	}
}
