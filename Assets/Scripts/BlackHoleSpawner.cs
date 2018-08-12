using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSpawner : MonoBehaviour {
	[SerializeField]
	public GameObject blackHolePrefab;
	[SerializeField]
	public GameObject player;
	[SerializeField]
	public float timeout;
	[SerializeField]
	public float radiusStart;
	[SerializeField]
	public float radiusEnd;

	float timeLeft;

	void Start () {
		timeLeft = timeout;
	}

	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0f ) {
			SpawnBlackHole();
			timeLeft = timeout;
		}
	}

	private void SpawnBlackHole() {
		float deg = Random.Range(0f, 359f);
		float r = Random.Range(radiusStart, radiusEnd);
		float x =  (Mathf.Sin(deg) * r);
		float y = (Mathf.Cos(deg) * r);
		Instantiate(blackHolePrefab, player.transform.position + new Vector3(x,-y,-4), player.transform.rotation);
	}

	private float DistanceTo(Vector3 lhs, Vector3 rhs) {
		return Mathf.Sqrt(Mathf.Pow(lhs.x - rhs.x, 2) + Mathf.Pow(lhs.y - rhs.y, 2));
	}
}
