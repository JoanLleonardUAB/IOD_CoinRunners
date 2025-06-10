using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTileManager : MonoBehaviour
{
    [SerializeField]
    private List<PowerTile> tiles;

    [SerializeField]
    private float interval = 5f;

    void Start()
    {
        StartCoroutine(ActivateRandomTiles());
    }

    IEnumerator ActivateRandomTiles()
    {
        while (true)
        {
            int idx = Random.Range(0, tiles.Count);
            var tile = tiles[idx];
            tile.Activate();
            yield return new WaitForSeconds(tile.effectDuration);
            tile.Deactivate();
            yield return new WaitForSeconds(interval);
        }
    }
}
