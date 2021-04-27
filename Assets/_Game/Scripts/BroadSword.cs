using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadSword : Weapon {
    [SerializeField] private Animator _animator;
    [SerializeField] private PointyEnd _pointyEnd;
    [SerializeField] private Transform _offset;

    public bool IsAttacking { get; set; }

    private void Start() {
        _pointyEnd.DamageAmount = 2;
        _pointyEnd.KnockbackForce = 10;
        _pointyEnd.CanDamage(false);

        CanAttack = true;
    }

    public override void Attack() {
        _pointyEnd.CanDamage(true);
        _animator.SetTrigger("Attack");
        CanAttack = false;

        IsAttacking = true;

        SoundManager.Instance.PlayAttack();
    }

    public void Recharge() {
        CanAttack = true;
    }

    public void HandleAttackAnimationFinished() {
        _pointyEnd.CanDamage(false);
        Recharge();

        IsAttacking = false;
    }

    protected override void LookAtMouse() {
        base.LookAtMouse();

        if (!IsAttacking) {
            var flip = transform.rotation.z < -.7f || transform.rotation.z > .7f;
            _offset.localScale = new Vector3(1, flip ? -1 : 1, 1);
        }
    }
}