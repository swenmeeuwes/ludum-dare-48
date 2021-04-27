using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {
    [SerializeField] private ShopTable[] _shopTables;
    [SerializeField] private Transform _playerSpawn;
    [SerializeField] private ShopKeeper _shopKeeper;
    [SerializeField] private CanvasGroup _bossIndicator;

    [SerializeField] private Sprite _broadSwordSprite;
    [SerializeField] private int _broadSwordCost;

    [SerializeField] private Sprite _daggerSprite;
    [SerializeField] private int _daggerCost;

    [SerializeField] private Sprite _heartSprite;
    [SerializeField] private int _heartCost;

    [SerializeField] private Sprite _twoHeartsSprite;
    [SerializeField] private int _twoHeartsCost;

    [SerializeField] private Sprite _threeHeartsSprite;
    [SerializeField] private int _threeHeartsCost;

    [SerializeField] private Sprite _extraHeartSprite;
    [SerializeField] private int _extraHeartCost;

    [SerializeField] private Sprite _helmetSprite;
    [SerializeField] private int _helmetCost;

    private Sale[] _sales;

    public Vector3 PlayerSpawnPos => _playerSpawn.position;

    private void Awake() {
        _bossIndicator.alpha = 0;
    }

    private void Start() {
        _sales = new Sale[] {
            new Sale {
                sprite = _broadSwordSprite,
                cost = _broadSwordCost,
                onBuy = () => {
                    var broadSword = WeaponsManager.Instance.GetBroadSword();
                    Player.Instance.Equip(broadSword);
                }
            },
            new Sale {
                sprite = _daggerSprite,
                cost = _daggerCost,
                onBuy = () => {
                    var dagger = WeaponsManager.Instance.GetDagger();
                    Player.Instance.Equip(dagger);
                }
            },
            new Sale {
                sprite = _heartSprite,
                cost = _heartCost,
                onBuy = () => {
                    Player.Instance.Heal(1);
                }
            },
            new Sale {
                sprite = _twoHeartsSprite,
                cost = _twoHeartsCost,
                onBuy = () => {
                    Player.Instance.Heal(2);
                }
            },
            //new Sale {
            //    sprite = _threeHeartsSprite,
            //    cost = _threeHeartsCost,
            //    onBuy = () => {
            //        Player.Instance.Heal(3);
            //    }
            //},
            new Sale {
                sprite = _extraHeartSprite,
                cost = _extraHeartCost,
                onBuy = () => {
                    Player.Instance.AddMaxHealth(1, true);
                }
            },
            new Sale {
                sprite = _helmetSprite,
                cost = _helmetCost,
                onBuy = () => {
                    Player.Instance.PutOnHelmet();
                }
            },
        };

        SetupItems();

        _shopKeeper.Speak();
    }

    private void SetupItems() {
        for (var i = 0; i < _shopTables.Length; i++) {
            var shopTable = _shopTables[i];
            shopTable.ShopKeeper = _shopKeeper;

            var randomSale = _sales[UnityEngine.Random.Range(0, _sales.Length)];

            shopTable.DisplayItem(randomSale.sprite, randomSale.cost, randomSale.onBuy);
        }
    }

    public void NextIsBoss() {
        _bossIndicator.alpha = 1;
    }

    struct Sale {
        public Sprite sprite;
        public int cost;
        public Action onBuy;
    }
}
