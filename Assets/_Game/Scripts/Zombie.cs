using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Monster {
    [SerializeField] private float _movementSpeed = 2f;

    private bool _isWalking;
    public bool IsWalking {
        get {
            return _isWalking;
        }
        set {
            _isWalking = value;
            _animator.SetBool("IsWalking", value);
        }
    }

    public bool IsOnTheMove { get; set; }

    private void Start() {
        Health = 5;
        MinCoins = 4;
        MaxCoins = 7;
        KnockbackResistance = 4;
    }

    private void Update() {
        if (!IsOnTheMove || Player.Instance == null || IsDieing || IsTakingDamage) {
            IsWalking = false;
            return;
        }

        IsWalking = true;

        var playerPos = Player.Instance.transform.position;
        var toPlayer = playerPos - transform.position;
        var dir = toPlayer.normalized;

        _rigidbody.velocity = dir * _movementSpeed;
    }

    public override void Sleep() {
        base.Sleep();

        IsOnTheMove = false;
    }

    public override void OnPlayerHit(Player player) {
        if (IsTakingDamage || IsDieing) {
            return;
        }

        player.Damage(1);
    }

    protected override IEnumerator ActionCoroutine() {
        yield return new WaitForSeconds(Random.Range(0.4f, .8f));

        IsOnTheMove = true;
    }
}
