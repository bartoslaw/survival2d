using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider) {
		print ("Collision babe");
		if (collider.gameObject.tag == "Player") {
			print ("Collision with player");
			transform.parent.SendMessage ("PlayerSeen");
		}
	}
}
