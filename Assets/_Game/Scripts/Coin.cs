using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _pickUpRange = 0.5f;

    private float _initialMoveSpeed = 5f;
    private float _moveSpeed = 5f;
    private bool _canBePickedUp = false;
    private bool _isBeingPickedUp = false;

    private void Start() {
        var velX = Random.Range(.4f, 1.2f);
        if (Random.Range(0f, 1f) >= .5f) {
            velX = -velX;
        }

        var velY = Random.Range(.4f, 1.2f);
        if (Random.Range(0f, 1f) >= .5f) {
            velY = -velY;
        }

        _rigidbody.velocity = new Vector2(velX, velY);

        Invoke("AllowPickUp", .8f);
    }

    private void Update() {
        if (_canBePickedUp && Player.Instance != null) {
            var playerPos = Player.Instance.transform.position;
            var toPlayer = playerPos - transform.position;
            var dir = toPlayer.normalized;
            var distanceToPlayer = toPlayer.magnitude;

            _rigidbody.velocity = dir * _moveSpeed;

            _moveSpeed += 5f * Time.deltaTime;
            if (_moveSpeed > 15f) {
                _moveSpeed = 20f;
            }

            if (distanceToPlayer < _pickUpRange) {
                if (!_isBeingPickedUp) {
                    _isBeingPickedUp = true;

                    SoundManager.Instance.PlayPickUp();

                    _spriteRenderer.DOFade(0, .15f)
                    .OnComplete(() => {
                        GameManager.Instance.CoinPickup();
                        Destroy(gameObject);
                    });
                }
            }
        }
    }

    private void AllowPickUp() {
        _canBePickedUp = true;
    }
}