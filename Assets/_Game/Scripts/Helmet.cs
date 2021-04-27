using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRender;

    public void SetFlipX(bool value) {
        _spriteRender.flipX = value;
    }

    public void SetIsWalking(bool value) {
        _animator.SetBool("IsWalking", value);
    }

    public void PopOff() {
        transform.SetParent(null);
        transform.position = Player.Instance.transform.position;

        SoundManager.Instance.PlayHelmetOff();

        _animator.SetTrigger("PopOff");
    }

    public void PopOffAnimationFinished() {
        Destroy(gameObject);
    }
}
