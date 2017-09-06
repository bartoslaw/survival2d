using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewController : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			transform.parent.SendMessage ("PlayerSeen");
		}
	}
}
