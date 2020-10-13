using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {

  public GameObject blockPrefab;
  public GameObject bombPrefab;

  GameObject firstBlock;
  GameObject lastBlock;
  List<GameObject> removeBlockList = new List<GameObject> ();

  ScoreManager scoreManager;
  FeverManager feverManager;
  TimeManager timeManager;

  void Start () {
    StartCoroutine (GenerateBlocks (45));
    scoreManager = gameObject.AddComponent<ScoreManager> ();
    feverManager = gameObject.AddComponent<FeverManager> ();
    timeManager = gameObject.AddComponent<TimeManager> ();

    feverManager.RegisterOnFeverCallBack (() => timeManager.AddTime (5));
  }

  void Update () {
    if (Input.GetMouseButton (0) && firstBlock == null) {
      OnDragStart ();
    } else if (Input.GetMouseButton (0) && firstBlock) {
      OnDragging ();
    } else if (Input.GetMouseButtonUp (0) && firstBlock) {
      OnDragEnd ();
    }
  }

  IEnumerator GenerateBlocks (int n) {
    for (int i = 0; i < n; i++) {
      // Generate a new block every 0.02 seconds
      yield return new WaitForSeconds (0.02f);
      GameObject block = GameObject.Instantiate (blockPrefab);
      block.transform.parent = transform;
    }
  }

  void GenerateBomb (Vector3 position) {
    GameObject bomb = GameObject.Instantiate (bombPrefab);
    bomb.transform.position = position;
    bomb.transform.parent = transform;
  }

  void OnDragStart () {
    GameObject newBlock = MousedOverBlock ();
    if (newBlock != null) {
      firstBlock = newBlock;
      lastBlock = newBlock;
      AddToRemoveBlockList (newBlock);
    }
  }

  void OnDragging () {
    GameObject newBlock = MousedOverBlock ();
    if (newBlock != null && newBlock != lastBlock) {
      if (IsNewBlockRemovable (newBlock)) {
        lastBlock = newBlock;
        AddToRemoveBlockList (newBlock);
      }
    }
  }

  void OnDragEnd () {
    int count = removeBlockList.Count;
    if (count >= 3) {
      OnBlockClear (count);
      if (count >= 7) {
        // Since lastBlock is deleted in ClearRemoveBlockList, this has to go before that
        GenerateBomb (lastBlock.transform.position);
      }
      ClearRemoveBlockList ();
    } else {
      ResetRemoveBlockList ();
    }
    firstBlock = null;
    lastBlock = null;
  }

  GameObject MousedOverBlock () {
    RaycastHit2D hit = Physics2D.Raycast (
      Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

    if (hit.collider != null) {
      GameObject hitObject = hit.collider.gameObject;
      if (Block.IsBlock (hitObject)) {
        return hitObject;
      }
    }
    return null;
  }

  bool IsNewBlockRemovable (GameObject newBlock) {
    if (!Block.IsSameType (newBlock, firstBlock)) {
      return false;
    } else if (newBlock.GetComponent<Block> ().IsOnChain ()) {
      return false;
    }

    RaycastHit2D[] hits = Physics2D.RaycastAll (
      (Vector2) lastBlock.transform.position,
      (Vector2) (newBlock.transform.position - lastBlock.transform.position),
      1);

    // The first hit is ignored because it returns the origin itself
    if (hits.Length > 1) {
      if (hits[1].collider != null) {
        if (hits[1].collider.gameObject == newBlock.gameObject) {
          return true;
        }
      }
    }
    return false;
  }

  void AddToRemoveBlockList (GameObject block) {
    removeBlockList.Add (block);
    block.GetComponent<Block> ().SetIsOnChain (true);
    block.GetComponent<Block> ().SetTransparency (0.5f);
  }

  // Distroy and move everyting out of removeBlockList
  void ClearRemoveBlockList () {
    foreach (GameObject block in removeBlockList) {
      Destroy (block);
    }
    removeBlockList.Clear ();
  }

  // Move everything out of removeBlockList without distroying
  void ResetRemoveBlockList () {
    foreach (GameObject block in removeBlockList) {
      block.GetComponent<Block> ().SetIsOnChain (false);
      block.GetComponent<Block> ().SetTransparency (1f);
    }
    removeBlockList.Clear ();
  }

  public void OnBlockClear (int chain) {
    scoreManager.AddScore (ScoreManager.CalculateScore (chain, 1, feverManager.IsFever ()));
    feverManager.AddFeverValue (chain);
    StartCoroutine (GenerateBlocks (chain));
  }
}