using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewController : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D collider) {
		print ("OnTriggerEnter2d");
		if (collider.gameObject.tag == "Player") {
			transform.parent.SendMessage ("PlayerSeen");
		}
	}
}
