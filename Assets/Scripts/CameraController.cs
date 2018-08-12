using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[SerializeField]
	public GameObject target;
	[SerializeField]
	public float speed = 2f;

	Vector2 offset;

	void Start() {
		SearchForPlayer();
	}


	void FixedUpdate() {
		MoveToTarget();
	}

	public void SearchForPlayer() {
		target = GameObject.FindGameObjectWithTag("Player");
		offset = new Vector3(transform.position.x - target.transform.position.x, transform.position.y - target.transform.position.y, -100);
	}

	public void MoveToTarget() {
		var targetCamPos = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, -100);
		transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -100), targetCamPos, speed * Time.deltaTime);
	}
}
