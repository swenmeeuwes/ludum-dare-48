using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopKeeper : MonoBehaviour {
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CanvasGroup _speechBalloon;
    [SerializeField] private TMP_Text _speechTextField;
    [SerializeField] private Sprite[] _shopKeeperSprites;
    [SerializeField] private string[] _buyTexts;

    private void Start() {
        RandomizeSprite();

        _speechBalloon.alpha = 0;
    }

    private void OnDestroy() {
        _speechBalloon.DOKill();
    }

    public void RandomizeSprite() {
        _spriteRenderer.sprite = _shopKeeperSprites[Random.Range(0, _shopKeeperSprites.Length)];
    }

    public void Speak() {
        _speechBalloon.alpha = 0;
        _speechTextField.text = _buyTexts[Random.Range(0, _buyTexts.Length)];

        _speechBalloon.DOKill();
        _speechBalloon.DOFade(1, .45f)
            .OnComplete(() => {
                _speechBalloon.DOFade(0, .45f)
                    .SetDelay(5f);
            });
    }
}
