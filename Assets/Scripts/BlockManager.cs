using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager: MonoBehaviour {

	public GameObject blockPrefab;
	public Sprite[] blockSprites;

	GameObject firstBlock;
	GameObject lastBlock;
	String removeBlockName;
	List<GameObject> removeBlockList = new List<GameObject>();

	ScoreManager scoreManager;

	void Start () {
		StartCoroutine(GenerateBlocks(45));
		scoreManager = new ScoreManager ();
	}

	void Update () {
		if (Input.GetMouseButton (0) && firstBlock == null) {
			OnDragStart ();
		} else if (Input.GetMouseButton (0) && firstBlock) {
			OnDragging ();
		} else if (Input.GetMouseButtonUp(0) && firstBlock) {
			OnDragEnd ();
		}
	}

	IEnumerator GenerateBlocks(int n){
		for (int i = 0; i < n; i++) {
			// Generate a block every 0.02 seconds
			yield return new WaitForSeconds (0.02f);

			GameObject block = GameObject.Instantiate (blockPrefab);

			int blockType = UnityEngine.Random.Range (0, 5);
			block.name = "Block_" + blockType;
			SpriteRenderer blockRenderer = block.GetComponent<SpriteRenderer> ();
			blockRenderer.sprite = blockSprites[blockType];

			block.transform.position = new Vector3(
				UnityEngine.Random.Range (-2.0f, 2.0f),
				10,
				0);
			block.transform.eulerAngles = new Vector3(
				0,
				0,
				UnityEngine.Random.Range(-40f, 40f));
		}
	}

	void OnDragStart() {
		RaycastHit2D hit = Physics2D.Raycast(
			Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);

		if (hit.collider != null) {
			GameObject hitObject = hit.collider.gameObject;

			if (hitObject.name.StartsWith("Block_")) {
				firstBlock  = hitObject;
				lastBlock   = hitObject;
				removeBlockName = hitObject.name;
				AddToRemoveBlockList (hitObject);
			}
		}
	}

	void OnDragging() {
		RaycastHit2D hit = Physics2D.Raycast(
			Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		
		if (hit.collider != null) {
			GameObject hitObject = hit.collider.gameObject;

			if (hitObject.name == removeBlockName && lastBlock != hitObject) {
				if (IsNewBlockRemovable(hitObject)) {
					lastBlock = hitObject;
					AddToRemoveBlockList (hitObject);
				}
			}
		}
	}

	void OnDragEnd() {
		int count = removeBlockList.Count;
		if (count >= 3) {
			scoreManager.AddScore (ScoreManager.CalculateScore(count, 1, false));
			ClearRemoveBlockList ();
			StartCoroutine(GenerateBlocks(count));
		} else {
			ResetRemoveBlockList ();
		}
		firstBlock = null;
		lastBlock  = null;
	}

	bool IsNewBlockRemovable(GameObject newBlock) {
		float distance = Vector2.Distance(
			newBlock.transform.position, lastBlock.transform.position);
		print (distance);
		return (distance < 1.0f);
	}

	void AddToRemoveBlockList(GameObject block) {
		removeBlockList.Add (block);
		block.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.5f);
	}

	// Distroy and move everyting out of removeBlockList
	void ClearRemoveBlockList() {
		for (int i = 0; i < removeBlockList.Count; i++) {
			Destroy (removeBlockList[i]);
		}
		removeBlockList.Clear ();
	}

	// Move everything out of removeBlockList without distroying
	void ResetRemoveBlockList() {
		for (int i = 0; i < removeBlockList.Count; i++) {
			removeBlockList[i].GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
		}
		removeBlockList.Clear ();
	}
}