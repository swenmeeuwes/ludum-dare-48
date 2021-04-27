using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster {
    [SerializeField] protected float _jumpForce = 10f;
    [SerializeField] protected float _jumpInterval = 5f;

    private Tween _moveTween;

    public bool IsJumping { get; private set; }

    protected virtual void Start() {
        Health = 2;
        MinCoins = 2;
        MaxCoins = 4;
    }

    public override void Wake() {
        _animator.ResetTrigger("Reset");

        base.Wake();
    }

    public override void Sleep() {
        if (_actionCoroutine != null) {
            _animator.ResetTrigger("QueueJump");
            _animator.SetTrigger("Reset");
        }

        if (_moveTween != null) {
            DOTween.Kill(_moveTween);
            _moveTween = null;
        }

        base.Sleep();
    }

    public override void OnPlayerHit(Player player) {
        if (IsTakingDamage || !IsJumping) {
            return;
        }

        player.Damage(1);
    }

    public override void Damage(int amount) {
        base.Damage(amount);

        _animator.ResetTrigger("QueueJump");

        if (_moveTween != null) {
            DOTween.Kill(_moveTween);
            _moveTween = null;
        }
    }

    public void QueueJump() {
        _animator.SetTrigger("QueueJump"); // Will trigger the `Jump` method with a signal in the animation
    }

    public void Jump() {
        if (Player.Instance == null) {
            return;
        }

        // Small trick to retrigger collision messages
        _collider.enabled = false;
        _collider.enabled = true;

        IsJumping = true;

        var target = Player.Instance.transform.position;
        var toTarget = target - transform.position;
        var dir = toTarget.normalized;
        //var distance = Mathf.Min(toTarget.magnitude, _jumpForce);

        _rigidbody.velocity = dir * _jumpForce;

        //_moveTween = transform.DOMove(transform.position + dir * distance, 1.2f);

        SoundManager.Instance.PlayJump();
    }

    public void FinishJump() { // Called by animator
        IsJumping = false;
    }

    protected override IEnumerator ActionCoroutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(0.4f, .8f));

            QueueJump();

            yield return new WaitForSeconds(_jumpInterval);
        }
    }
}
