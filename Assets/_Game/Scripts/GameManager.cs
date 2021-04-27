using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [SerializeField] private Shop _shopPrefab;
    [SerializeField] private TutorialRoom _tutorialPrefab;
    [SerializeField] private SlimeBossRoom _slimeBossRoom;

    public int CurrentFloor { get; set; } = 0;
    public int Coins { get; set; } = 0;

    private Shop _currentShop;
    private TutorialRoom _currentTutorial;
    private SlimeBossRoom _currentSlimeBossRoom;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        // Start weapon
        Player.Instance.Equip(WeaponsManager.Instance.GetDagger());

        ShowTutorial();

        // DEBUG:
        //EnterSlimeBossRoom();

        //Player.Instance.Equip(WeaponsManager.Instance.GetBroadSword());
        //Player.Instance.PutOnHelmet();
        //Player.Instance.AddMaxHealth(2, true);
    }

    public void EnterSlimeBossRoom() {
        _currentSlimeBossRoom = Instantiate(_slimeBossRoom);
        Player.Instance.transform.position = _slimeBossRoom.PlayerSpawnPos;
        UIManager.Instance.FadeIn();

        SoundManager.Instance.PlayBossBGM();
        SoundManager.Instance.FadeInBGM(.25f);
    }

    public void ShowTutorial() {
        _currentTutorial = Instantiate(_tutorialPrefab);
        Player.Instance.transform.position = _currentTutorial.PlayerSpawnPos;
        UIManager.Instance.FadeIn();

        SoundManager.Instance.PlayDungeonBGM();
        SoundManager.Instance.FadeInBGM(.25f);
    }

    public void CompleteTutorial() {
        NextFloor();
    }

    public void CompleteFloor() {
        SoundManager.Instance.FadeOutBGM(.25f);

        UIManager.Instance.FadeOut()
            .OnComplete(() => {
                SoundManager.Instance.PlayShopBGM();
                SoundManager.Instance.FadeInBGM(.25f);

                DungeonGenerator.Instance.CleanUp();
                _currentShop = Instantiate(_shopPrefab);

                if (CurrentFloor == 10) {
                    _currentShop.NextIsBoss();
                }

                Player.Instance.transform.position = _currentShop.PlayerSpawnPos;
                Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);

                UIManager.Instance.SetFloor(CurrentFloor, true);

                UIManager.Instance.FadeIn();
            });
    }

    public void CompleteShop() {
        NextFloor();
    }

    public void NextFloor() {
        CurrentFloor++;

        SoundManager.Instance.FadeOutBGM(.25f);

        UIManager.Instance.FadeOut()
            .OnComplete(() => {
                if (_currentShop != null) {
                    Destroy(_currentShop.gameObject);
                    _currentShop = null;
                }

                if (_currentTutorial != null) {
                    Destroy(_currentTutorial.gameObject);
                    _currentTutorial = null;
                }

                if (CurrentFloor <= 10) {
                    SoundManager.Instance.PlayDungeonBGM();
                    SoundManager.Instance.FadeInBGM(.25f);

                    DungeonGenerator.Instance.CleanUp();
                    DungeonGenerator.Instance.Generate(5 + CurrentFloor);
                    Player.Instance.transform.position = Vector3.zero;
                    Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);

                    UIManager.Instance.SetFloor(CurrentFloor);
                } else {
                    EnterSlimeBossRoom();
                    UIManager.Instance.SetBossFloor();
                }

                UIManager.Instance.FadeIn();
            });
    }

    public void CoinPickup() {
        Coins++;
        UIManager.Instance.SetCoins(Coins);
    }

    public void PlayerDead() {
        var playerPos = Player.Instance.transform.position;

        Camera.main.transform.DOMove(new Vector3(playerPos.x, playerPos.y, Camera.main.transform.position.z), .65f);
        Camera.main.GetComponent<Camera>().DOOrthoSize(1f, .65f);

        DeathOverlay.Instance.Show(.65f);
    }
}
