using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Enemy.Spawner {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private float spawnInterval;

        
        // when the script gets enabled, start spawning
        private void OnEnable() {
            StartCoroutine(nameof(SpawnEnemy));
        }

        
        // when the script gets disabled, stop spawning
        private void OnDisable() {
            StopCoroutine(nameof(SpawnEnemy));
        }

        
        // spawn enemies every given seconds
        private IEnumerator SpawnEnemy() {
            while (true) {
                yield return new WaitForSeconds(spawnInterval);
                var randChoice = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[randChoice], transform);
            }
        }
    }
}