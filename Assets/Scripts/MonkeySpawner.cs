using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeySpawner : MonoBehaviour
{
    [Header("Settings")]
    public float monkeySpawnCooldownDuration = 1;

    [Header("Components")]
    public GameObject monkeyPrefab;
    public List<Transform> spawnPoints;

    private float lastMonkeySpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastMonkeySpawnTime + monkeySpawnCooldownDuration)
        {
            int rand = Random.Range(0, spawnPoints.Count);
            spawnMonkey(spawnPoints[rand].position);
        }
    }

    private void spawnMonkey(Vector2 pos)
    {
        GridAI gridAI = Instantiate(monkeyPrefab).GetComponent<GridAI>();
        GameObject monkey = gridAI.objectToMove.gameObject;
        monkey.transform.position = pos;
        lastMonkeySpawnTime = Time.time;
        onMonkeySpawned?.Invoke(gridAI);
    }
    public delegate void OnMonkeySpawned(GridAI monkey);
    public event OnMonkeySpawned onMonkeySpawned;

}
