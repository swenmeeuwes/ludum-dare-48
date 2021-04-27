using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeart : MonoBehaviour {
    [SerializeField] private Image _image;

    [SerializeField] private Sprite _full;
    [SerializeField] private Sprite _empty;

    private bool _isFilled = true;

    public void SetFilled(bool value) {
        var tookHit = _isFilled && !value;

        _image.sprite = value ? _full : _empty;

        if (tookHit) {
            _image.rectTransform.DOShakeScale(.45f);
        }

        _isFilled = value;
    }
}
