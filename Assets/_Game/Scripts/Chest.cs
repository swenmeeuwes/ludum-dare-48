using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Room Room { get; set; }

    public int MinCoins { get; set; } = 3; // set in room.cs
    public int MaxCoins { get; set; } = 6;

    public void Open() {
        SoundManager.Instance.PlayOpenChest();
        _animator.SetTrigger("Open");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var pointyEnd = collision.GetComponent<PointyEnd>();
        if (pointyEnd != null) {
            _collider.enabled = false;

            Open();
        }
    }

    public void SpawnCoins() {
        CoinSpawner.Instance.SpawnCoins(transform.position, 1 + Random.Range(MinCoins - 1, MaxCoins));
        _spriteRenderer.DOFade(0, .45f)
            .OnComplete(() => {
                Room.RemoveChest(this);
                Destroy(gameObject);
            });
    }
}
