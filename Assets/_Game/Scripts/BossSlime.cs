using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlime : Slime {
    [SerializeField] private float _waitAfterSpawningMoreSlimes = 5f;
    [SerializeField] private int _minSpawnSlimeAmount = 2;
    [SerializeField] private int _maxSpawnSlimeAmount = 4;
    [SerializeField] private int _stunnedTime = 3;
    
    [SerializeField] private int _health = 20;

    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _hurtMaterial;

    public SlimeBossRoom BossRoom { get; set; }

    private bool _isStunned;
    public bool IsStunned {
        get {
            return _isStunned;
        }
        private set {
            _isStunned = value;
            _animator.SetBool("Stunned", value);

            if (!value) {
                _spriteRenderer.material = _defaultMaterial;
            }
        }
    }

    private bool _isSpawning;
    public bool IsSpawning {
        get {
            return _isSpawning;
        }
        private set {
            _isSpawning = value;
            _animator.SetBool("Spawning", value);
        }
    }

    protected override void Start() {
        base.Start();
        
        Health = _health;
        MinCoins = 0;
        MaxCoins = 0;
    }

    private void OnDestroy() {
        BossRoom.BossDefeated();
    }

    public void OnAppearAnimationFinished() {
        StartCoroutine(ActionCoroutine());
    }

    public override void OnPlayerHit(Player player) {
        if (IsTakingDamage || !IsJumping) {
            return;
        }

        player.Damage(2);
    }

    public void Stun() {
        IsStunned = true;

        StopAllCoroutines();
        StartCoroutine(StunnedCoroutine());
    }

    public override void Damage(int amount) {
        if (!IsStunned) {
            return;
        }
        
        base.Damage(amount);

        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect() {
        _spriteRenderer.material = _hurtMaterial;

        yield return new WaitForSeconds(.4f);

        _spriteRenderer.material = _defaultMaterial;
        StoppedTakingDamage();
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        base.OnCollisionEnter2D(collision);

        if (!IsJumping) {
            return;
        }

        var rock = collision.gameObject.GetComponent<Rock>();
        if (rock != null) {
            SoundManager.Instance.PlayExplosion();
            Stun();
            rock.Break();
        }
    }

    protected override IEnumerator ActionCoroutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(0.4f, .8f));

            var r = Random.value;
            if (r < .65f) {
                QueueJump();

                yield return new WaitForSeconds(_jumpInterval);
            } else {
                StartCoroutine(SpawnSlimes());
                yield return new WaitForSeconds(_waitAfterSpawningMoreSlimes);
            }
        }
    }

    private IEnumerator SpawnSlimes() {
        IsSpawning = true;

        var amount = 1 + Random.Range(_minSpawnSlimeAmount - 1, _maxSpawnSlimeAmount);
        for (var i = 0; i < amount; i++) {
            BossRoom.SpawnSlime();

            yield return new WaitForSeconds(Random.Range(.8f, 1.5f));
        }

        BossRoom.SpawnRock();

        IsSpawning = false;
    }

    private IEnumerator StunnedCoroutine() {
        yield return new WaitForSeconds(_stunnedTime);

        IsStunned = false;
        StartCoroutine(ActionCoroutine());
    }
}
