using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] tilePrefabs;
    [SerializeField] private int rows, cols;
    [Range(0, 1)] [SerializeField] private float tilePct = 0.1f;
    [SerializeField] private float tileSize = 1;
    [SerializeField] private Transform tileContainer;

    private List<GameObject> tiles = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnLevel();
    }

    public void SpawnLevel()
    {
        foreach (var t in tiles)
            Destroy(t);

        tiles.Clear();

        Vector3 corner = tileContainer.transform.position + new Vector3(-(rows) / 2f * tileSize, 0, -(cols) / 2f * tileSize);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (Random.Range(0f,1f) < tilePct)
                {
                    // spawn a tile at random
                    Vector3 pos = corner + new Vector3(i, 0, j) * tileSize;
                    GameObject spawnedTile = (GameObject)Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)], pos, Quaternion.identity, tileContainer);
                    tiles.Add(spawnedTile);
                }
            }
        }
    }

    // Let's try wave function collapse
    // need to identify rules for tiles for what can spawn next to it (i want all 8 directions)
    // then we pick a point at random and collapse it
    // collapsing a tile places restrictions on neighboring tiles
    // then get a random tile with the least entropy (ties pick random)
    // how do we get that? surely we don't go through all and pick the lowest? Ive heard of shannon entropy
    // somehow it uses a sample to determine shape and whatnot. That's the part I don't understand

}
