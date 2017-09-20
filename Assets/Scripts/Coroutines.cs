using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Coroutines {
	public static IEnumerator WaitAndChange(SpriteRenderer renderer) {
		while (true) {
			yield return new WaitForSeconds(0.1f);
			renderer.enabled = !renderer.enabled;
		}
	}
}


