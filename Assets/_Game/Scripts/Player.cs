using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player Instance { get; private set; }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;

    [SerializeField] private Helmet _helmetPrefab;

    [SerializeField] private Material _default;
    [SerializeField] private Material _hitEffect;

    [SerializeField] private float _speed = 4;
    [SerializeField] private float _damageCooldown = 1f;

    public Helmet Helmet { get; set; }

    public int MaxHealth { get; private set; } = 3;
    public int Health { get; private set; } = 3;
    public bool Died { get; set; }
    public Weapon Weapon { get; set; }

    public float LastDamageTime { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        UIManager.Instance.SetMaxHearts(MaxHealth);
        UIManager.Instance.SetHearts(Health);
    }

    private void Update() {
        if (Died) {
            return;
        }

        HandleMovement();
        HandleAttack();
    }

    public void PutOnHelmet() {
        if (Helmet != null) {
            return;
        }

        Helmet = Instantiate(_helmetPrefab, transform);
        Helmet.transform.localPosition = Vector3.zero;
    }

    public void LoseHelmet() {
        Helmet.PopOff();

        Helmet = null;
    }

    public void Equip(Weapon weapon) {
        if (Weapon != null) {
            Destroy(Weapon.gameObject);
            Weapon = null;
        }

        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = Vector3.zero;
        Weapon = weapon;
    }

    public void AddMaxHealth(int amount, bool fillNewHearts) {
        MaxHealth += amount;
        if (fillNewHearts) {
            Health += amount;
        }

        UIManager.Instance.SetMaxHearts(MaxHealth);
        UIManager.Instance.SetHearts(Health);
    }

    public void Heal(int amount) {
        Health += amount;

        if (Health > MaxHealth) {
            Health = MaxHealth;
        }

        UIManager.Instance.SetHearts(Health);
    }

    public void Damage(int amount) {
        if (LastDamageTime + _damageCooldown > Time.time || Died) {
            return;
        }

        _spriteRenderer.material = _hitEffect;

        if (Helmet == null) {
            Health -= amount;
            UIManager.Instance.SetHearts(Health);
        } else {
            LoseHelmet();
        }

        StartCoroutine(HitEffectCoroutine());

        LastDamageTime = Time.time;

        SoundManager.Instance.PlayHit();

        if (Health <= 0) {
            Died = true;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.Sleep();
            Destroy(_rigidbody);

            _animator.SetTrigger("Die");
            GameManager.Instance.PlayerDead();
        }
    }

    private IEnumerator HitEffectCoroutine() {
        _spriteRenderer.material = _hitEffect;

        yield return new WaitForSeconds(_damageCooldown);

        _spriteRenderer.material = _default;
    }

    private void HandleMovement() {
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");

        var move = new Vector3(moveX, moveY, 0);
        var moveNormalized = move.normalized;

        _rigidbody.velocity = moveNormalized * _speed;

        if (move.magnitude > .1f) {
            var flipX = move.x < 0;
            _spriteRenderer.flipX = flipX;
            _animator.SetBool("IsMoving", true);

            if (Helmet != null) {
                Helmet.SetIsWalking(true);
                Helmet.SetFlipX(flipX);
            }
        } else {
            _animator.SetBool("IsMoving", false);

            if (Helmet != null) {
                Helmet.SetIsWalking(false);
            }
        }
    }

    private void HandleAttack() {
        if (Input.GetMouseButton(0) && Weapon.CanAttack) {
            Weapon.Attack();
        }
    }
}