using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {
    public static CoinSpawner Instance { get; set; }

    [SerializeField] private Coin _coinPrefab;

    private void Awake() {
        Instance = this;
    }

    public void SpawnCoins(Vector3 at, int amount) {
        for (var i = 0; i < amount; i++) {
            var coin = Instantiate(_coinPrefab);
            coin.transform.position = at;
        }
    }
}
