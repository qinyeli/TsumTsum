using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public Sprite[] blockSprites;

	int blockType;
	bool isOnChain;

	void Start () {
		blockType = UnityEngine.Random.Range (0, 5);
		name = "Block_" + blockType;
		GetComponent<SpriteRenderer> ().sprite = blockSprites[blockType];

		transform.position = new Vector3 (UnityEngine.Random.Range (-2.0f, 2.0f), 10, 0);
		transform.eulerAngles = new Vector3 (0, 0, UnityEngine.Random.Range (-40f, 40f));
	}

	public void SetIsOnChain(bool isOnChain) {
		this.isOnChain = isOnChain;
	}

	public bool IsOnChain() {
		return isOnChain;
	}

	public void SetTransparency(float transparency) {
		Color color = GetComponent<SpriteRenderer> ().color;
		color.a = transparency;
		GetComponent<SpriteRenderer> ().color = color;
	}

	public static bool IsSameType(GameObject block1, GameObject block2) {
		Block b1 = block1.GetComponent<Block> ();
		Block b2 = block2.GetComponent<Block> ();
		return (b1 != null && b2 != null && b1.blockType == b2.blockType);
	}

	public static bool IsBlock(GameObject block) {
		return (block.GetComponent<Block> () != null);
	}
}
