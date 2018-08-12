using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {
	[SerializeField]
	public float lifespan = 10f;
	float time;

	void Start () {
		time = 0;
	}
	
	void Update () {
		time += Time.deltaTime;
		transform.Rotate(new Vector3(0, 0, -1f));
		if (time >= lifespan) {
			GetComponent<Animator>().Play("BlackHoleAnimationReversed") ;
			Object.Destroy(gameObject, 1.2f);
		}
	}
}
