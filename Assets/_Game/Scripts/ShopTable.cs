using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopTable : MonoBehaviour {
    [SerializeField] private CanvasGroup _instructions;
    [SerializeField] private CanvasGroup _price;
    [SerializeField] private SpriteRenderer _slot;
    [SerializeField] private TMP_Text _costTextField;
    [SerializeField] private CanvasGroup _cannotBuyOverlay;

    private bool _playerIsNear = false;
    private Action _onBuy;

    public ShopKeeper ShopKeeper { get; set; }

    public int Cost { get; private set; }
    public bool Sold { get; private set; }

    private void Start() {
        _instructions.alpha = 0;
        _cannotBuyOverlay.alpha = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && !Sold) {
            _playerIsNear = true;
            _instructions.DOFade(1, .25f);

            if (Cost > GameManager.Instance.Coins) {
                _cannotBuyOverlay.alpha = 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            _playerIsNear = false;
            _instructions.DOFade(0, .25f);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && _playerIsNear) {
            PlayerBuysItem();
        }
    }

    public void DisplayItem(Sprite sprite, int cost, Action onBuy) {
        _slot.sprite = sprite;
        _costTextField.text = cost.ToString();
        _onBuy = onBuy;

        Cost = cost;
    }

    private void PlayerBuysItem() {
        if (Sold) {
            return;
        }

        if (Cost > GameManager.Instance.Coins) {
            _costTextField.transform.DOKill();
            _costTextField.DOKill();
            _costTextField.GetComponent<RectTransform>().localScale = Vector3.one;

            _costTextField.transform.DOShakeScale(.25f);

            _costTextField.DOColor(Color.red, .25f)
                .OnComplete(() => {
                    _costTextField.DOColor(Color.black, .45f);
                });

            return;
        }

        if (ShopKeeper != null) {
            ShopKeeper.Speak();
        }

        _onBuy.Invoke();
        Sold = true;

        _slot.DOFade(0, .25f);
        _price.DOFade(0, .25f);
        _instructions.DOFade(0, .25f);

        GameManager.Instance.Coins -= Cost;
        UIManager.Instance.SetCoins(GameManager.Instance.Coins);

        SoundManager.Instance.PlayBuy();
    }
}
