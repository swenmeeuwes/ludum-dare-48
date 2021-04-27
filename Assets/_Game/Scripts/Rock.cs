using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
    [SerializeField] private SpriteRenderer _rockSpriteRenderer;
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;
    [SerializeField] private Collider2D _collider;

    public SlimeBossRoom Room { get; set; }

    public bool IsBroken { get; set; }

    private void OnTriggerEnter2D(Collider2D collision) {
        var bossSlime = collision.GetComponent<BossSlime>();
        var monster = collision.GetComponent<Monster>();
        if (monster != null && bossSlime == null) {
            monster.Damage(1);
        }

        if (bossSlime != null) {
            bossSlime.Stun();
            Break();
        }

        var player = collision.GetComponent<Player>();
        if (player != null) {
            player.Damage(1);
            Break();
        }
    }

    public void OnFallen() {
        var prevPos = Camera.main.transform.position;
        Camera.main.DOShakePosition(.1f, strength: 1f).OnComplete(() => Camera.main.transform.position = prevPos);

        SoundManager.Instance.PlayRockFall();
    }

    public void Break() {
        if (IsBroken) {
            return;
        }

        IsBroken = true;

        _collider.enabled = false;

        _shadowSpriteRenderer.DOFade(0, .3f);
        _rockSpriteRenderer.DOFade(0, .35f)
            .OnComplete(() => {
                Destroy(gameObject);
            });
    }
}
