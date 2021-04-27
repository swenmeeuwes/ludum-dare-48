using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour {
    [SerializeField] protected Rigidbody2D _rigidbody;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    public Room Room { get; set; }
    public int Health { get; set; } = 1;
    public int KnockbackResistance { get; set; } = 1;
    public int MinCoins { get; set; }
    public int MaxCoins { get; set; }
    public bool IsTakingDamage { get; set; }
    public bool IsDieing { get; set; }

    protected Coroutine _actionCoroutine;
    protected Vector3 _initialPos;

    public abstract void OnPlayerHit(Player player);
    protected abstract IEnumerator ActionCoroutine();

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        var player = collision.gameObject.GetComponent<Player>();
        if (player == null) {
            return;
        }

        OnPlayerHit(player);
    }

    public void Knockback(Vector2 force) {
        _rigidbody.AddForce(force / KnockbackResistance, ForceMode2D.Impulse);
    }

    public virtual void Wake() {
        if (_actionCoroutine == null) {
            _initialPos = transform.position;
            _actionCoroutine = StartCoroutine(ActionCoroutine());
        }
    }

    public virtual void Sleep() {
        if (_actionCoroutine != null) {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;
        }

        transform.position = _initialPos;
    }

    public virtual void Damage(int amount) {
        IsTakingDamage = true;

        Health -= amount;

        if (Health <= 0) {
            Die();
        } else {
            _animator.SetTrigger("Hit");
        }

        SoundManager.Instance.PlayHit();
    }

    public void StoppedTakingDamage() {
        IsTakingDamage = false;
    }

    public void Kill() {
        Die();
    }

    protected void Die() {
        if (IsDieing) {
            return;
        }

        IsDieing = true;
        _animator.SetTrigger("Die"); // Will trigger `DieFadeOut` via animation signal

        if (Room != null) {
            Room.RemoveMonster(this);
        }
    }

    protected virtual void DieFadeOut() {
        if (CoinSpawner.Instance != null) {
            CoinSpawner.Instance.SpawnCoins(transform.position, Random.Range(MinCoins, MaxCoins));
        }

        _rigidbody.Sleep();
        _collider.enabled = false;

        _spriteRenderer.DOFade(0, .45f)
            .OnComplete(() => Destroy(gameObject));
    }
}
