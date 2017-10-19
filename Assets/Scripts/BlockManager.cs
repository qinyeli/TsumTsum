using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager: MonoBehaviour {

	public GameObject blockPrefab;
	public Sprite[] blockSprites;

	void Start () {
		StartCoroutine(GenerateBlocks(55));
	}

	IEnumerator GenerateBlocks(int n){
		for (int i = 0; i < n; i++) {
			// Generate a block every 0.02 seconds
			yield return new WaitForSeconds (0.02f);

			GameObject block = GameObject.Instantiate (blockPrefab);
			SpriteRenderer blockRenderer = block.GetComponent<SpriteRenderer> ();
			block.transform.position = new Vector3(
				UnityEngine.Random.Range (-2.0f, 2.0f),
				10,
				0);
			block.transform.eulerAngles = new Vector3(
				0,
				0,
				UnityEngine.Random.Range(-40f, 40f));
			blockRenderer.sprite = blockSprites[UnityEngine.Random.Range(0, 5)];
		}
	}
}
