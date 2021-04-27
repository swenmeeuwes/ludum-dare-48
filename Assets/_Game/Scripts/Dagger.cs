using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon {
    [SerializeField] private Animator _animator;
    [SerializeField] private PointyEnd _pointyEnd;

    private void Start() {
        _pointyEnd.DamageAmount = 1;
        _pointyEnd.KnockbackForce = 5;
        _pointyEnd.CanDamage(false);

        CanAttack = true;
    }

    public override void Attack() {
        _pointyEnd.CanDamage(true);
        _animator.SetTrigger("Attack");
        CanAttack = false;

        SoundManager.Instance.PlayAttack();
    }

    public void Recharge() {
        CanAttack = true;
    }

    public void HandleAttackAnimationFinished() {
        _pointyEnd.CanDamage(false);
        Recharge();
    }
}