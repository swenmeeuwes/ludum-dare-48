using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    [SerializeField] private Animator _animator;

    [SerializeField] private float _firstStabDelay = .5f;
    [SerializeField] private float _stabInterval = 5f;

    public int DamageAmount { get; set; } = 1;

    public Room Room { get; set; }

    private Coroutine _actionCoroutine;
    public bool NoSound { get; set; }
    public bool PreventSounds { get; set; }

    public void Wake() {
        if (_actionCoroutine == null) {
            _actionCoroutine = StartCoroutine(ActionCoroutine());
        }

        PreventSounds = false;
    }

    public void Sleep() {
        if (_actionCoroutine != null) {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;
        }

        PreventSounds = true;
    }

    public void QueueStab() {
        _animator.SetTrigger("Stab");
    }

    public void PlayStabSound() {
        if (PreventSounds || NoSound) {
            return;
        }

        SoundManager.Instance.PlayStab();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var monster = collision.GetComponent<Monster>();
        if (monster != null) {
            monster.Damage(DamageAmount);
        }

        var player = collision.GetComponent<Player>();
        if (player != null) {
            player.Damage(DamageAmount);
        }
    }

    private IEnumerator ActionCoroutine() {
        while (true) {
            yield return new WaitForSeconds(_firstStabDelay);

            QueueStab();

            yield return new WaitForSeconds(_stabInterval);
        }
    }
}
