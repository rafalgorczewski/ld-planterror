using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour {

	[SerializeField]
	public int id = 0;
	public bool isAlive = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D collision) {
		if (collision.tag == "BlackHole") {
			if (!isAlive) {
				var force = (collision.transform.position - transform.position).normalized;
				GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x, force.y));
			}
		}
		else if (collision.tag == "BlackHoleCenter") {
			GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.5f, 0f);
		}
	}
}
