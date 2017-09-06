using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour {

	private enum State {
		IDLE,
		RANDOM_WALKING,
		WALK_TOWARDS_PLAYER,
		ATTACK
	}

	private const string IS_WALKING = "isWalking";
	private const string IS_ATTACKING = "isAttacking";

	private const float idleTime = 1.0f;
	private const float walkingTime = 0.5f;
	private const float attackTime = 0.5f;

	private State currentState = State.IDLE;
	public GameObject player;

	private Animator animator;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;

	private float speed = 1.3f;
	private Vector3 lastVelocity;

	private Direction directionToPlayer;
	private Direction currentDirection;

	void Start () {
		animator = GetComponent<Animator> (); 	
		rigidBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
		
	void Update () {
		float distanceBetweenPlayerAndMob = Math.Abs (transform.position.x - player.transform.position.x);

		if (distanceBetweenPlayerAndMob < 0.5f && currentState == State.WALK_TOWARDS_PLAYER) {
			currentState = State.ATTACK;
		} else if (distanceBetweenPlayerAndMob > 0.5f && currentState == State.ATTACK) {
			currentState = State.WALK_TOWARDS_PLAYER;
		}
			
		EvaluateState ();
		rigidBody.velocity = lastVelocity;
	}

	void EvaluateState() {
		print (currentState);
		if (currentState == State.IDLE) {
			StayIdle ();
		} else if (currentState == State.RANDOM_WALKING) {
			Walk ();
		} else if (currentState == State.WALK_TOWARDS_PLAYER) {
			WalkToPlayer ();
		} else if (currentState == State.ATTACK) {
			Attack ();
		}
	}

	void Attack() {
		animator.SetBool (IS_WALKING, false);
		animator.SetBool (IS_ATTACKING, true);
		lastVelocity = Vector3.zero;

		if (!IsInvoking()) {
			Invoke ("ToggleAttackingState", attackTime);
		}
	}
		
	void Walk() {
		animator.SetBool (IS_WALKING, true);
		spriteRenderer.flipX = currentDirection != Direction.LEFT;
		lastVelocity = new Vector3 (currentDirection == Direction.LEFT ? -speed : speed, 0.0f, 0.0f);

		if (!IsInvoking()) {
			Invoke ("ToggleIdleState", walkingTime);
		}
	}

	void StayIdle() {
		animator.SetBool (IS_WALKING, false);
		lastVelocity = Vector3.zero;

		if (!IsInvoking()) {
			Invoke ("ToggleIdleState", idleTime);
		}
	}

	void WalkToPlayer() {
		animator.SetBool (IS_ATTACKING, false);
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
	}

	void ToggleAttackingState() {
		animator.SetBool(IS_ATTACKING, false);
		currentState = State.WALK_TOWARDS_PLAYER;
	}

	void ToggleIdleState() {
		if (currentState == State.IDLE) {
			int randomValue = UnityEngine.Random.Range (0, 10);
			currentDirection = randomValue < 5 ? Direction.LEFT : Direction.RIGHT;
			currentState = State.RANDOM_WALKING;
		} else {
			currentState = State.IDLE;
		}
 	}

	public void PlayerSeen() {
		print ("PlayerSeen");
		CancelInvoke ();
		currentState = State.WALK_TOWARDS_PLAYER;
	}
}
