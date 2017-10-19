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

	void Start () {
		StartCoroutine(GenerateBlocks(55));
	}

	void Update () {
		if (Input.GetMouseButton (0) && firstBlock == null) {
			OnClickStart ();
		} else if (Input.GetMouseButton (0) && firstBlock) {
			OnClicking ();
		} else if (Input.GetMouseButtonUp(0) && firstBlock) {
			OnClickEnd ();
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

	void OnClickStart() {
		RaycastHit2D hit = Physics2D.Raycast(
			Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);

		if (hit.collider != null) {
			GameObject hitObject = hit.collider.gameObject;

			if (hitObject.name.StartsWith("Block_")) {
				firstBlock  = hitObject;
				lastBlock   = hitObject;
				removeBlockName = hitObject.name;
				removeBlockList.Add (hitObject);
			}
		}
	}

	void OnClicking() {
		RaycastHit2D hit = Physics2D.Raycast(
			Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		
		if (hit.collider != null) {
			GameObject hitObject = hit.collider.gameObject;

			if (hitObject.name == removeBlockName && lastBlock != hitObject) {
				float distance = Vector2.Distance(
					hitObject.transform.position, lastBlock.transform.position);

				print (distance);
				if (distance < 1.0f) {
					lastBlock = hitObject;
					removeBlockList.Add (hitObject);
				}
			}
		}
	}

	void OnClickEnd() {
		int count = removeBlockList.Count;
		if (count >= 3) {
			for (int i = 0; i < count; i++) {
				Destroy (removeBlockList[i]);
			}
			removeBlockList.Clear ();
		}
		firstBlock = null;
		lastBlock  = null;
	}
}
