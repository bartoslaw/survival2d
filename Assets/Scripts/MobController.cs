using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour {
	private const string IS_WALKING = "isWalking";
	private const string IS_ATTACKING = "isAttacking";

	private Animator animator;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;

	private float speed = 2.3f;
	private Vector3 lastVelocity;

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

		Walk ();
	}

	void Attack() {
		
	}

	void Walk() {
		
	}
}
