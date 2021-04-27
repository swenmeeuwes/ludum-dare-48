using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : MonoBehaviour {
    [SerializeField] private Transform _playerSpawn;

    public Vector3 PlayerSpawnPos => _playerSpawn.position;
}
