using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningSlime : MonoBehaviour {
    public Room Room { get; set; }

    public void PlaySound() {
        SoundManager.Instance.PlaySpawn();
    }

    public void SpawnSlime() {
        var slimePrefab = MonsterManager.Instance.GetSlimePrefab();
        var slime = Instantiate(slimePrefab, Room.transform);
        slime.transform.position = transform.position;
        slime.Room = Room;

        slime.Wake();

        Room.RegisterExternalMonster(slime);

        Destroy(gameObject);
    }
}
