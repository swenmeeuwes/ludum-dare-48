using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour {
    public static WeaponsManager Instance { get; set; }

    [SerializeField] private Dagger _daggerPrefab;
    [SerializeField] private BroadSword _broadSwordPrefab;

    private void Awake() {
        Instance = this;
    }

    public Dagger GetDagger() {
        return Instantiate(_daggerPrefab);
    }

    public BroadSword GetBroadSword() {
        return Instantiate(_broadSwordPrefab);
    }
}
