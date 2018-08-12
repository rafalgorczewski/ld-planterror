using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour {
	[SerializeField]
	public int id = 0;
	[SerializeField]
	public float maxGrabLength = 1f;
	[SerializeField]
	public GameObject childPrefab;
	public GameObject branchPrefab;
	public Transform gameOverTransform;

	private GameObject childObject;
	private GameObject branchObject;


	static float length = 0;

	bool isAlive = true;
	bool isChildCreated = false;
	bool gameOver = false;
	float lerpedGameOverAlpha = 0f;
	static bool isStartingNode = true;

	void Start() {
		if (isAlive && !gameOver && isStartingNode) {
			CreateChildNode();
			CreateBranch();
			isStartingNode = false;
		}
	}

	void Update() {
		if (Input.GetMouseButtonUp(0) && !gameOver && !isChildCreated) {
			CreateChildNode();
			CreateBranch();
		}
		if (isChildCreated) {
			if (isAlive && !gameOver) {
				ApplyPositionChangeToChild();
			}
			RotateBranchTowardsChild();
			ScaleBranch();
			if (Input.GetMouseButtonDown(0) && !gameOver) {
				if (!gameOver) {
					isAlive = false;
					branchObject.GetComponent<Branch>().isAlive = false;
					length += DistanceTo(transform.position, childObject.transform.position);
				}
			}
		}
		if (gameOver) {
			lerpedGameOverAlpha += 0.01f;
			GameOver();
		}
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
			if (tag == "Player" && !gameOver) {
				GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
				GetComponent<AudioSource>().Play();
				gameOver = true;
			}
		}
	}

	private void CreateChildNode() {
		childObject = Instantiate(childPrefab, transform.position, transform.rotation);
		childObject.GetComponent<Node>().id = id + 1;
		childObject.transform.parent = transform;
		childObject.GetComponent<Node>().gameOverTransform = gameOverTransform;
		childObject.tag = "Player";
		tag = "Node";
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().target = gameObject;
		GameObject.FindGameObjectWithTag("BlackHoleSpawner").GetComponent<BlackHoleSpawner>().player = gameObject;
		isChildCreated = true;
	}

	private void CreateBranch() {
		branchObject = Instantiate(branchPrefab, transform.position, transform.rotation);
		branchObject.GetComponent<Branch>().id = id + 1;
		branchObject.transform.parent = transform;
		branchObject.transform.position += new Vector3(0, 0, 1);
	}

	private void ApplyPositionChangeToChild() {
		var clickedPosition = Camera.allCameras[0].ScreenToWorldPoint(Input.mousePosition);
		if (DistanceTo(clickedPosition, transform.position) <= maxGrabLength) {
			childObject.GetComponent<Rigidbody2D>().MovePosition(new Vector3(clickedPosition.x, clickedPosition.y, transform.position.z));
		}
	}

	private void RotateBranchTowardsChild() {
		var dir = childObject.transform.position - transform.position;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		var newRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
		branchObject.transform.rotation = newRotation;
	}

	private void ScaleBranch() {
		var distance = DistanceTo(branchObject.transform.position, childObject.transform.position);
		branchObject.transform.localScale = new Vector3(1, distance * 4f, 1);
	}

	private float DistanceTo(Vector3 lhs, Vector3 rhs) {
		return Mathf.Sqrt(Mathf.Pow(lhs.x - rhs.x, 2) + Mathf.Pow(lhs.y - rhs.y, 2));
	}

	private void GameOver() {
		var colour = gameOverTransform.GetComponent<SpriteRenderer>().color;
		gameOverTransform.GetComponent<SpriteRenderer>().color = new Color(colour.r, colour.g, colour.b, lerpedGameOverAlpha);
		GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().color = new Color(colour.r, colour.g, colour.b, lerpedGameOverAlpha);
		GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text = length.ToString("0.0");
		if (Input.GetButtonDown("Submit")) {
			length = 0f;
			isStartingNode = true;
			UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
		}
	}
}
