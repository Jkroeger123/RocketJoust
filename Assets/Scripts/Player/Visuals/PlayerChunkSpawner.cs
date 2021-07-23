using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Serialization;
using UnityEngine;

public class PlayerChunkSpawner : MonoBehaviour
{
    public float explosionForce = 30f;


    public List<ListWrapper> chunks =new List<ListWrapper>();
    
    public GameObject chunkPrefab;

    public void SpawnChunks(int i)
    {
        foreach (Sprite sprite in chunks[i-1].colorOption)
        {
            GameObject o = Instantiate(chunkPrefab, transform.position, transform.rotation);
            o.GetComponent<SpriteRenderer>().sprite = sprite;

            Vector2 randDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            o.GetComponent<Rigidbody2D>().velocity = randDir * explosionForce;

            SpriteRenderer r = o.GetComponent<SpriteRenderer>();
            r.DOColor(Color.clear, 15f).SetEase(Ease.InQuint).OnComplete(() => Destroy(o));

        }
    }

}

[System.Serializable]
public class ListWrapper
{
    public List<Sprite> colorOption;
}
