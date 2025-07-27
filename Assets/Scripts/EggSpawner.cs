using System.Collections;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    public GameObject redEggPrefab, whiteEggPrefab;
    public Transform[] spawnPoints;

    public float spawnInterval = 1.0f;
    public static EggSpawner Instance;
    void Awake() => Instance = this;

    public void UpdateSpawnInterval(float newInterval)
    {
        spawnInterval = newInterval;
    }

    void Start()
    {
        StartCoroutine(SpawnEggs());
    }

    IEnumerator SpawnEggs()
    {
        while (true)
        {
            SpawnRandomEgg();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomEgg()
    {
        int randIndex = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPos = spawnPoints[randIndex].position;

        GameObject prefabToSpawn = Random.value > 0.3f ? redEggPrefab : whiteEggPrefab;

        Vector3 topOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        spawnPos.y = topOfScreen.y + 1f; // Move above screen

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }

}
