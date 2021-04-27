using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour {
    [SerializeField] private Tilemap _collidingTilemap;
    [SerializeField] private Tilemap _floorTilemap;

    [SerializeField] private TileBase _wallTile;
    [SerializeField] private TileBase _rockTile;
    [SerializeField] private TileBase _floorTile;
    [SerializeField] private TileBase _floorDecorTile;
    [SerializeField] private TileBase _pitTile;

    [SerializeField] private Stairs _stairsPrefab;
    [SerializeField] private Chest _chestPrefab;
    [SerializeField] private Spikes _spikesPrefab;

    [SerializeField] private CanvasGroup _overlay;

    public int MaxChests { get; set; }
    public int MaxMonsters { get; set; }
    public RoomData RoomData { get; set; }
    public bool IsBeingCleanedUp { get; set; }

    private List<Monster> _monsters = new List<Monster>();
    private List<Chest> _chests = new List<Chest>();
    private List<Spikes> _spikes = new List<Spikes>();

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag != "Player") {
            return;
        }

        RoomVisibilityManager.Instance.PlayerEntersNewRoom(this);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag != "Player") {
            return;
        }

        RoomVisibilityManager.Instance.PlayerExitsRoom(this);
    }

    public void CleanUp() {
        IsBeingCleanedUp = true;

        for (var i = 0; i < _monsters.Count; i++) {
            var monster = _monsters[i];
            monster.Sleep();
            Destroy(monster.gameObject);
        }

        _overlay.DOKill();
    }

    public void PutMonstersToSleep() {
        for (var i = 0; i < _monsters.Count; i++) {
            var monster = _monsters[i];
            monster.Sleep();
        }

        for (var i = 0; i < _spikes.Count; i++) {
            var spikes = _spikes[i];
            spikes.Sleep();
        }
    }

    public void KillAllMonsters() {
        for (var i = 0; i < _monsters.Count; i++) {
            var monster = _monsters[i];
            monster.Kill();
        }
    }

    public void WakeMonsters() {
        for (var i = 0; i < _monsters.Count; i++) {
            var monster = _monsters[i];
            monster.Wake();
        }

        for (var i = 0; i < _spikes.Count; i++) {
            var spikes = _spikes[i];
            spikes.Wake();
        }
    }

    public void Show() {
        if (IsBeingCleanedUp) {
            return;
        }

        _overlay.DOKill();
        _overlay.DOFade(0, .25f);
    }

    public void Hide() {
        if (IsBeingCleanedUp) {
            return;
        }

        _overlay.DOKill();
        _overlay.DOFade(1, .25f);
    }

    public void RemoveMonster(Monster monster) {
        _monsters.Remove(monster);
    }

    public void RemoveChest(Chest chest) {
        _chests.Remove(chest);
    }

    public void Render() {
        var roomTemplate = RoomData.Template;

        var width = roomTemplate.width;
        var halvedWidth = Mathf.RoundToInt(width * .5f);

        var height = roomTemplate.height;
        var halvedHeight = Mathf.RoundToInt(height * .5f);

        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                RenderPixel(roomTemplate, new Vector3Int(x, y, 0));
            }
        }

        FillDoorsThatLeadNowhere();

        _collidingTilemap.transform.position += new Vector3(-halvedWidth, -halvedHeight, 0);
        _floorTilemap.transform.position += new Vector3(-halvedWidth, -halvedHeight, 0);
    }

    private void FillDoorsThatLeadNowhere() {
        if (RoomData.Up == null) {
            CreateWall(new Vector3Int(6, RoomData.Template.height - 1, 0));
            CreateWall(new Vector3Int(7, RoomData.Template.height - 1, 0));
        }

        if (RoomData.Down == null) {
            CreateWall(new Vector3Int(6, 0, 0));
            CreateWall(new Vector3Int(7, 0, 0));
        }

        if (RoomData.Left == null) {
            CreateWall(new Vector3Int(0, 3, 0));
            CreateWall(new Vector3Int(0, 4, 0));
        }

        if (RoomData.Right == null) {
            CreateWall(new Vector3Int(RoomData.Template.width - 1, 3, 0));
            CreateWall(new Vector3Int(RoomData.Template.width - 1, 4, 0));
        }
    }

    private void RenderPixel(Texture2D roomTemplate, Vector3Int at) {
        var color = roomTemplate.GetPixel(at.x, at.y);

        if (color == Color.black) {
            if (at.x > 0 && at.x < RoomData.Template.width - 1 &&
                at.y > 0 && at.y < RoomData.Template.height - 1) {
                CreateRock(at);
            } else {
                CreateWall(at);
            }
            return;
        }

        if (color == Color.white) {
            CreateFloor(at);
            return;
        }

        if (color == Color.red) {
            CreateMonster(at);
            return;
        }

        if (color == Color.cyan) {
            CreatePit(at);
            return;
        }

        if (color == Color.blue) {
            CreateStairs(at);
            return;
        }

        if (color == Color.magenta) {
            CreateSpikes(at);
            return;
        }

        // Yellow
        if (color == new Color(1, 1, 0, 1)) {
            CreateChest(at);
            return;
        }

        if (color.a == 0) {
            // Empty
            return;
        }

        Debug.LogWarning($"RoomRenderer was unable to render pixel of color {color}");
    }

    public void CreateWall(Vector3Int at) {
        _collidingTilemap.SetTile(at, _wallTile);
        _floorTilemap.SetTile(at, null);

        CreateFloor(at); // Put floor under wall
    }

    public void CreateRock(Vector3Int at) {
        _collidingTilemap.SetTile(at, _rockTile);
        CreateFloor(at); // Put floor under rock
    }

    public void CreateFloor(Vector3Int at) {
        //var r = Random.Range(0f, 1f);
        //if (r > .8f) {
        //    _floorTilemap.SetTile(at, _floorDecorTile);
        //} else {
        _floorTilemap.SetTile(at, _floorTile);
        //}
    }

    public void CreatePit(Vector3Int at) {
        _collidingTilemap.SetTile(at, _pitTile);
    }

    public void CreateStairs(Vector3Int at) {
        var stairs = Instantiate(_stairsPrefab, transform);
        stairs.transform.position = _collidingTilemap.CellToWorld(at)
            + new Vector3(-Mathf.RoundToInt(RoomData.Template.width * .5f), -Mathf.RoundToInt(RoomData.Template.height * .5f), 0)
            + new Vector3(.5f, .5f);
    }

    public void CreateSpikes(Vector3Int at) {
        CreateFloor(at); // Put floor under spikes

        var spikes = Instantiate(_spikesPrefab, transform);
        spikes.transform.position = _collidingTilemap.CellToWorld(at)
            + new Vector3(-Mathf.RoundToInt(RoomData.Template.width * .5f), -Mathf.RoundToInt(RoomData.Template.height * .5f), 0)
            + new Vector3(.5f, .5f);

        if (_spikes.Count > 0) {
            spikes.NoSound = true; // prevent sound stacking
        }

        _spikes.Add(spikes);
    }

    public void CreateChest(Vector3Int at) {
        CreateFloor(at); // Put floor under chest

        if (_chests.Count >= MaxChests) {
            return;
        }

        var chest = Instantiate(_chestPrefab, transform);
        chest.transform.position = _collidingTilemap.CellToWorld(at)
            + new Vector3(-Mathf.RoundToInt(RoomData.Template.width * .5f), -Mathf.RoundToInt(RoomData.Template.height * .5f), 0)
            + new Vector3(.5f, .5f);

        chest.Room = this;
        chest.MinCoins = 15;
        chest.MaxCoins = 25;

        _chests.Add(chest);
    }

    public void CreateMonster(Vector3Int at) {
        if (MonsterManager.Instance == null) {
            return;
        }

        CreateFloor(at); // Put floor under monster

        if (_monsters.Count >= MaxMonsters) {
            return;
        }

        var monsterPrefab = MonsterManager.Instance.GetMonsterPrefab();
        var monster = Instantiate(monsterPrefab, transform);
        monster.transform.position = _collidingTilemap.CellToWorld(at)
            + new Vector3(-Mathf.RoundToInt(RoomData.Template.width * .5f), -Mathf.RoundToInt(RoomData.Template.height * .5f), 0)
            + new Vector3(.5f, .5f);

        monster.Room = this;

        _monsters.Add(monster);
    }

    public void RegisterExternalMonster(Monster monster) {
        _monsters.Add(monster);
    }
}
