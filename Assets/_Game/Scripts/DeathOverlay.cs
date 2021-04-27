using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathOverlay : MonoBehaviour {
    public static DeathOverlay Instance { get; private set; }

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _floorTextField;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _canvasGroup.alpha = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && _canvasGroup.alpha > .9f && Player.Instance.Died) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Show(float delay = 0) {
        _floorTextField.text = $"Floor  {GameManager.Instance.CurrentFloor}";

        _canvasGroup.DOFade(1, .45f)
            .SetDelay(delay);
    }
}
