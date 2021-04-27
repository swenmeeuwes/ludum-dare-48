using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {
    [SerializeField] private CanvasGroup _instructions;

    private bool _playerIsOnStairs = false;

    public bool Interacted { get; set; }

    private void Start() {
        _instructions.alpha = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            _playerIsOnStairs = true;
            _instructions.alpha = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            _playerIsOnStairs = false;
            _instructions.alpha = 0;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && _playerIsOnStairs) {
            if (Interacted) {
                return;
            }

            SoundManager.Instance.PlayStairs();

            Interacted = true;
            GameManager.Instance.CompleteFloor();
        }
    }
}
