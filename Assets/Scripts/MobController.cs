using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour {
	private const string IS_WALKING = "isWalking";
	private const string IS_ATTACKING = "isAttacking";

	private const float idleTime = 1.0f;
	private const float walkingTime = 0.5f;
	private const float attackTime = 0.5f;

	public GameObject player;

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
	private bool isAttacking = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> (); 	
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		float distanceBetweenPlayerAndMob = Math.Abs (transform.position.x - player.transform.position.x);
		print (distanceBetweenPlayerAndMob);

		if (animator.GetBool (IS_ATTACKING)) {
			Attack ();
			return;
		}
			
		if (distanceBetweenPlayerAndMob < 0.5f && wasPlayerSeen) {
			print ("For some reason not here");
			animator.SetBool (IS_WALKING, false);
			animator.SetBool (IS_ATTACKING, true);
			return;
		} else {
			animator.SetBool (IS_ATTACKING, false);
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
		if (!isAttacking) {
			isAttacking = true;
			lastVelocity = Vector3.zero;

			//todo do not invoke this one after another, keep him idle for a bit if he's in an attacking state
			Invoke ("ToggleAttackingState", attackTime);
		}

		rigidBody.velocity = lastVelocity;
	}

	void ToggleAttackingState() {
		animator.SetBool (IS_ATTACKING, false);
	}

	void Walk() {
		if (!startedWalking) {
			isIdle = false;
			startedWalking = true;
			animator.SetBool (IS_WALKING, true);

			int randomValue = UnityEngine.Random.Range (0, 10);
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
		animator.SetBool (IS_WALKING, true);
		bool isPlayerToTheLeft = transform.position.x > player.transform.position.x;

		lastVelocity = new Vector3 (isPlayerToTheLeft ? -speed : speed, 0.0f, 0.0f);

		bool shouldFlipLeft = isPlayerToTheLeft && currentDirection != Direction.LEFT;
		bool shouldFlipRight = !isPlayerToTheLeft && currentDirection != Direction.RIGHT;

		if (shouldFlipLeft) {
			currentDirection = Direction.LEFT;
			spriteRenderer.flipX = false;
		} else if (shouldFlipRight) {
			currentDirection = Direction.RIGHT;
			spriteRenderer.flipX = true;
		}

		rigidBody.velocity = lastVelocity;
	}

	void ToggleIdleState() {
		shouldBeIdle = !shouldBeIdle;
	}

	public void PlayerSeen() {
		print ("Message received");
		wasPlayerSeen = true;
	}
}
