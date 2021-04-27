using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {
    public static MonsterManager Instance { get; private set; }

    [SerializeField] private Slime _slimePrefab;
    [SerializeField] private Zombie _zombiePrefab;

    private void Awake() {
        Instance = this;
    }

    public Slime GetSlimePrefab() {
        return _slimePrefab;
    }

    public Monster GetMonsterPrefab() {
        if (GameManager.Instance.CurrentFloor == 1) {
            return _slimePrefab;
        }

        var zombieChance = Mathf.Lerp(0f, 1f, GameManager.Instance.CurrentFloor / 20f);
        if (Random.value < zombieChance) {
            return _zombiePrefab;
        } else {
            return _slimePrefab;
        }
    }
}
