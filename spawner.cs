using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {

	public Wave[] waves;
	public LivingEntity enemy;


	int enemyRemainingToSpawn;
	int enemyRemainingAlive;
	float nextSpawnTime;

	Wave CurrentWave;
	int currentWaveNumber;

	void Start(){
		NextWave ();

	}

	void Update(){
		if (enemyRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
			enemyRemainingToSpawn--;
			nextSpawnTime = Time.time + CurrentWave.timeBetweenSpawns;
			LivingEntity spawnedEnemy = Instantiate (enemy, transform.position, Quaternion.identity) as LivingEntity;
			spawnedEnemy.OnDeath += OnEnemyDeath;
		}
	}
	void OnEnemyDeath(){
		enemyRemainingAlive--;
		if (enemyRemainingAlive == 0) {
			NextWave ();
		}
		
	}
	void NextWave(){
		currentWaveNumber++;
		if (currentWaveNumber - 1 < waves.Length) {
			CurrentWave = waves [currentWaveNumber - 1];

			enemyRemainingToSpawn = CurrentWave.enemyCount;
			enemyRemainingAlive = enemyRemainingToSpawn;
		}


	}
	[System.Serializable]
	public class Wave{
		public int enemyCount;
		public float timeBetweenSpawns;
	}
}
