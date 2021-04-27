using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeBossRoom : Room {
    [SerializeField] private BossSlime _bossSlimePrefab;
    [SerializeField] private Transform _bossSlimeSpawn;
    [SerializeField] private SpawningSlime _spawningSlimePrefab;
    [SerializeField] private Rock _rockPrefab;
    [SerializeField] private Rect _spawningSlimeArea;
    [SerializeField] private CanvasGroup _winCanvasGroup;

    [SerializeField] private Transform _playerSpawn;

    public Vector3 PlayerSpawnPos => _playerSpawn.position;

    private void Start() {
        _winCanvasGroup.alpha = 0;

        Invoke("SpawnBossSlime", 1f);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && _winCanvasGroup.alpha > .9f) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {

    }

    protected override void OnTriggerExit2D(Collider2D collision) {

    }

    public void SpawnBossSlime() {
        var bossSlime = Instantiate(_bossSlimePrefab, transform);
        bossSlime.transform.position = _bossSlimeSpawn.position;
        bossSlime.BossRoom = this;
    }

    public void SpawnSlime() {
        var at = new Vector2(
            Random.Range(_spawningSlimeArea.x, _spawningSlimeArea.x + _spawningSlimeArea.width),
            Random.Range(_spawningSlimeArea.y, _spawningSlimeArea.y + _spawningSlimeArea.height)
        );

        var spawningSlime = Instantiate(_spawningSlimePrefab, transform);
        spawningSlime.transform.position = at;
        spawningSlime.Room = this;
    }

    public void SpawnRock() {
        var spawningRock = Instantiate(_rockPrefab, transform);
        spawningRock.transform.position = Player.Instance.transform.position;
        spawningRock.Room = this;
    }

    public void BossDefeated() {
        SoundManager.Instance.PlayVictoryBGM();
        PutMonstersToSleep();
        KillAllMonsters();

        _winCanvasGroup.DOFade(1, .65f)
            .SetDelay(.45f);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_spawningSlimeArea.center, _spawningSlimeArea.size);
    }
}
