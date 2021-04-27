using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private Transform _heartContainer;
    [SerializeField] private UIHeart _heartPrefab;
    [SerializeField] private CanvasGroup _fadeOverlay;
    [SerializeField] private TMP_Text _floorTextField;
    [SerializeField] private TMP_Text _coinsTextField;

    public static UIManager Instance { get; private set; }

    private List<UIHeart> _uiHearts = new List<UIHeart>();

    private int _hearts;

    private void Awake() {
        Instance = this;

        _floorTextField.text = "Tutorial";
    }

    public Tween FadeOut(float duration = .25f) {
        return _fadeOverlay.DOFade(1, duration);
    }

    public Tween FadeIn(float duration = .25f) {
        return _fadeOverlay.DOFade(0, duration);
    }

    public void SetMaxHearts(int value) {
        for (var i = 0; i < _uiHearts.Count; i++) {
            Destroy(_uiHearts[i].gameObject);
        }

        _uiHearts.Clear();

        for (var i = 0; i < value; i++) {
            var heart = Instantiate(_heartPrefab, _heartContainer);
            heart.SetFilled(i < _hearts);

            _uiHearts.Add(heart);
        }
    }

    public void SetHearts(int value) {
        _hearts = value;

        for (var i = 0; i < _uiHearts.Count; i++) {
            var heart = _uiHearts[i];
            heart.SetFilled(i < _hearts);
        }
    }

    public void SetFloor(int value, bool isShop = false) {
        if (isShop) {
            _floorTextField.text = $"Floor {value} - Shop";
        } else {
            _floorTextField.text = $"Floor {value}";
        }
    }
    
    public void SetBossFloor() {
        _floorTextField.text = $"Boss";
    }

    public void SetCoins (int value) {
        _coinsTextField.text = $"x{value}";
    }
}
