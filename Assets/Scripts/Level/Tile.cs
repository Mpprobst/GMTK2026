using System.Linq;
using UnityEngine;

[System.Serializable]
public class PickupData
{
    public GameObject prefab;
    [Range(1,10)] public int rarity;
}

public class Tile : MonoBehaviour
{
    // needs to slow the player down
    // needs a chance to spawn water and other rewards

    [Range(0,1)] public float itemChance = 0.5f;
    [SerializeField] private PickupData[] items;
    [SerializeField] private Transform itemSpawnLoc;

    public void Initialize(float spawnScale)
    {
        if (Random.Range(0f, 1f) < itemChance * spawnScale)
        {
            int sum = items.Select(x => x.rarity).Sum();
            int rand = Random.Range(0, sum);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].rarity >= rand)
                {
                    // spawn
                    GameObject spawned = Instantiate(items[i].prefab, itemSpawnLoc);
                    spawned.transform.localPosition = Vector3.zero;
                    spawned.transform.localScale = new Vector3(1 / transform.localScale.x, 1/transform.localScale.y, 1/transform.localScale.z);
                    break;
                }
                rand -= items[i].rarity;
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
}
