using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour {

	private enum State {
		IDLE,
		IDLE_AFTER_ATTACK,
		RANDOM_WALKING,
		WALK_TOWARDS_PLAYER,
		ATTACK,
		HURT,
		DEAD
	}

	private const string IS_WALKING = "isWalking";
	private const string IS_ATTACKING = "isAttacking";

	private const float idleTime = 1.0f;
	private const float walkingTime = 0.5f;
	private const float attackTime = 0.5f;
	private const float coolOfTime = 0.4f;
	private const float hurtTime = 0.5f;

	public GameObject player;
	public int lifeCount = 3;

	private State currentState = State.IDLE;

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
		if (currentState == State.IDLE) {
			StayIdle ();
		} else if (currentState == State.RANDOM_WALKING) {
			Walk ();
		} else if (currentState == State.WALK_TOWARDS_PLAYER) {
			WalkToPlayer ();
		} else if (currentState == State.ATTACK) {
			Attack ();
		} else if (currentState == State.IDLE_AFTER_ATTACK) {
			StayIdleAfterAttack ();	
		} else if (currentState == State.HURT) {
			Suffer ();
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

	void StayIdleAfterAttack() {
		animator.SetBool (IS_WALKING, false);
		lastVelocity = Vector3.zero;

		if (!IsInvoking ()) {
			Invoke ("ToggleAttackingState", coolOfTime);
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
		
	IEnumerator coroutine;

	private IEnumerator WaitAndChange(float waitTime)
	{
		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			spriteRenderer.enabled = !spriteRenderer.enabled;
		}
	}

	void Suffer() {
		animator.SetBool (IS_ATTACKING, false);
		animator.SetBool (IS_WALKING, false);

		if (coroutine == null) {
			print ("Starting coroutine");
			coroutine = WaitAndChange (0.1f);
			StartCoroutine (coroutine);
		}

		lastVelocity = Vector3.zero;
	}

	void ToggleAttackingState() {
		if (currentState == State.ATTACK) {
			animator.SetBool (IS_ATTACKING, false);
			currentState = State.IDLE_AFTER_ATTACK;
		} else {
			currentState = State.ATTACK;
		}
	}

	void ToggleIdleState() {
		if (!spriteRenderer.enabled) {
			spriteRenderer.enabled = true;
		}

		if (coroutine != null) { 
			print ("Stopping coroutine");
			StopCoroutine (coroutine);
			coroutine = null;
		}

		if (currentState == State.IDLE) {
			int randomValue = UnityEngine.Random.Range (0, 10);
			currentDirection = randomValue < 5 ? Direction.LEFT : Direction.RIGHT;
			currentState = State.RANDOM_WALKING;
		} else {
			currentState = State.IDLE;
		}
 	}

	public void PlayerSeen() {
		CancelInvoke ();
		currentState = State.WALK_TOWARDS_PLAYER;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == Tags.BULLET_TAG) {
			Destroy (collision.gameObject);

			if (currentState != State.HURT) {
				currentState = State.HURT;

//				lifeCount--;
				if (lifeCount == 0) {
					Destroy (gameObject);
					return;
				}
					
				Invoke ("ToggleIdleState", hurtTime);
			}
		}
	}
}
