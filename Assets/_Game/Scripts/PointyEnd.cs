using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointyEnd : MonoBehaviour {
    public int DamageAmount { get; set; }
    public int KnockbackForce { get; set; }

    private Collider2D _collider;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var monster = other.GetComponent<Monster>();

        if (monster == null) {
            return;
        }

        monster.Damage(DamageAmount);

        var dir = (other.transform.position - transform.position).normalized;
        monster.Knockback(dir * KnockbackForce);
    }

    public void CanDamage(bool value) {
        _collider.enabled = value;
    }
}
