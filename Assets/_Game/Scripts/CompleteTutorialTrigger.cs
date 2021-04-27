using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteTutorialTrigger : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GetComponent<Collider2D>().enabled = false;
            GameManager.Instance.CompleteTutorial();
        }
    }
}
