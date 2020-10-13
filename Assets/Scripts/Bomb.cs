using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    void OnMouseDown()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        GetComponentInParent<BlockManager>().OnBlockClear(colliders.Length);

        foreach (Collider2D collider in colliders)
        {
            if (Block.IsBlock(collider.gameObject))
            {
                Destroy(collider.gameObject);
            }
        }
        Destroy(gameObject);
    }
}