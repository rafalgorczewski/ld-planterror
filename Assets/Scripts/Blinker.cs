using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour {
	[SerializeField]
	public float maxTime = 3f;

	bool blink = false;
		
	float timeLeft;

	void Start() {
		timeLeft = maxTime;
	}

	void Update() {
		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0f) {
			var colour = GetComponent<SpriteRenderer>().color;
			GetComponent<SpriteRenderer>().color = (blink ? new Color(colour.r, colour.g, colour.b, 0) : new Color(colour.r, colour.g, colour.b, 255));
			blink = !blink;
			timeLeft = maxTime;
		}
	}
}
