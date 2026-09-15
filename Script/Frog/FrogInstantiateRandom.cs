using System.Collections;
using UnityEngine;

// Ryan Joshua Smith Frog Prefab spawner script for Frog micro-game

public class FrogInstantiateRandom : MonoBehaviour
{
    [Header("Insect & Bird Prefabs")]
    [SerializeField] GameObject[] prefabArray;

    [Header("Chosen Index")]
    private int randomPrefab;

    private void Start()
    {
        StartCoroutine(InstantiateRandomPrefab());
    }

    private IEnumerator InstantiateRandomPrefab()
    {
        // Delays the next loop through as too many on the screen would make it difficult to move around 
        yield return new WaitForSeconds(Random.Range(0, 10));

        // Picks a random number using the length of the prefab array as the limit, said number is used for the index of what should be spawned
        randomPrefab = Random.Range(0, prefabArray.Length);

        // Spawns the prefab corresponding to the index number generated before and tells it where to spawn (whatever empty parent this script is attached to)
        Instantiate(prefabArray[randomPrefab], gameObject.transform.position, gameObject.transform.rotation);

        // Continues the cycle
        StartCoroutine(InstantiateRandomPrefab());
    }
}