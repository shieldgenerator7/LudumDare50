using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeySpawner : MonoBehaviour
{
    [Header("Settings")]
    public float SpawnCooldownDuration 				= 0;
	public float spawnCooldownDurationMin 			= 1;
	public float spawnCooldownDurationMax 			= 8;
	public float changePerSecond 					= -.15f;
	
    [Header("Components")]
    public GameObject monkeyPrefab;
    public List<Transform> spawnPoints;

    private float lastMonkeySpawnTime = 0;

	
    // Update is called once per frame
    void Update()
    {
		SpawnCooldownDuration += changePerSecond * Time.deltaTime;
		
		if (SpawnCooldownDuration <= spawnCooldownDurationMin){
		SpawnCooldownDuration = spawnCooldownDurationMin;
		}
        if (Time.time >= lastMonkeySpawnTime + SpawnCooldownDuration)
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
	
	void OnEnable()
    {
        SpawnCooldownDuration = spawnCooldownDurationMax; 
    }
}
