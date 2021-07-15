using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntryHandler : MonoBehaviour
{
    
    public GameObject playerChalk;
    public GameObject playerPoof;
    
    private GameObject _player;

    public void ExecuteSequence(GameObject player)
    {
        _player = player;
        _player.SetActive(false);

        StartCoroutine(PlayAnimations());
    }

    private IEnumerator PlayAnimations()
    {
        GameObject i = Instantiate(playerChalk, _player.transform.position, Quaternion.identity);
        i.GetComponent<SpriteRenderer>().sprite = _player.GetComponent<SpriteRenderer>().sprite;

        yield return new WaitForSeconds(1f);

        Instantiate(playerPoof, _player.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.3f);
        _player.SetActive(true);
        Destroy(i);
    }




}
