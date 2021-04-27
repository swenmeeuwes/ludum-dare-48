using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
    public static DungeonGenerator Instance { get; private set; }

    [SerializeField] private Room _roomPrefab;
    [SerializeField] private Texture2D[] _roomTemplates;
    [SerializeField] private Texture2D[] _treasureRoomsTemplates;
    [SerializeField] private Texture2D _startRoomTemplate;
    [SerializeField] private Texture2D[] _endRoomTemplates;

    //[SerializeField] [Range(0, 1)] private float _branchingWeight;

    public static int RoomWidth { get; } = 14;
    public static int RoomHeight { get; } = 8;

    private List<Room> _rooms = new List<Room>();

    private void Awake() {
        Instance = this;
    }

    //private void Start() {
    //    Generate(15);

    //    _rooms[0].Show();
    //}

    public void CleanUp() {
        for (var i = 0; i < _rooms.Count; i++) {
            var room = _rooms[i];
            room.CleanUp();
            Destroy(room.gameObject);
        }

        _rooms.Clear();
    }

    public void Generate(int amountOfRooms) {
        var availableCells = new HashSet<Vector2Int>() {
            Vector2Int.zero
        };

        // Generate dungeon structure
        for (var i = 0; i < amountOfRooms; i++) {
            var cellIndex = UnityEngine.Random.Range(0, availableCells.Count);
            var cell = availableCells.ToArray()[cellIndex];
            availableCells.Remove(cell);

            var roomTemplate = _startRoomTemplate;
            if (i == amountOfRooms - 1) {
                roomTemplate = _endRoomTemplates[UnityEngine.Random.Range(0, _endRoomTemplates.Length)];
            }
            else if (i > 0) {
                var treasureRoomChance = Mathf.Lerp(.05f, .3f, Mathf.Min(1, GameManager.Instance.CurrentFloor / 10));
                if (UnityEngine.Random.value < treasureRoomChance) {
                    roomTemplate = _treasureRoomsTemplates[UnityEngine.Random.Range(0, _treasureRoomsTemplates.Length)];
                } else {
                    roomTemplate = _roomTemplates[UnityEngine.Random.Range(0, _roomTemplates.Length)];
                }
            }

            var room = GenerateRoom(cell, roomTemplate);
            room.MaxMonsters = 1 + Mathf.FloorToInt(GameManager.Instance.CurrentFloor / 3f);
            room.MaxChests = 1 + Mathf.FloorToInt(GameManager.Instance.CurrentFloor / 3f);

            if (!_rooms.Any(r => r.RoomData.Location == cell + Vector2Int.up)) {
                availableCells.Add(cell + Vector2Int.up);
            }
            if (!_rooms.Any(r => r.RoomData.Location == cell + Vector2Int.down)) {
                availableCells.Add(cell + Vector2Int.down);
            }
            if (!_rooms.Any(r => r.RoomData.Location == cell + Vector2Int.left)) {
                availableCells.Add(cell + Vector2Int.left);
            }
            if (!_rooms.Any(r => r.RoomData.Location == cell + Vector2Int.right)) {
                availableCells.Add(cell + Vector2Int.right);
            }

            var pos = cell * new Vector2Int(RoomWidth, RoomHeight);
            room.transform.position = new Vector3(pos.x, pos.y, 0);

            _rooms.Add(room);
        }

        // Render rooms
        for (var i = 0; i < _rooms.Count; i++) {
            var room = _rooms[i];

            var roomUp = _rooms.FirstOrDefault(r => r.RoomData.Location == room.RoomData.Location + Vector2Int.up);
            if (roomUp != null) {
                room.RoomData.Up = roomUp.RoomData;
            }

            var roomDown = _rooms.FirstOrDefault(r => r.RoomData.Location == room.RoomData.Location + Vector2Int.down);
            if (roomDown != null) {
                room.RoomData.Down = roomDown.RoomData;
            }

            var roomLeft = _rooms.FirstOrDefault(r => r.RoomData.Location == room.RoomData.Location + Vector2Int.left);
            if (roomLeft != null) {
                room.RoomData.Left = roomLeft.RoomData;
            }

            var roomRight = _rooms.FirstOrDefault(r => r.RoomData.Location == room.RoomData.Location + Vector2Int.right);
            if (roomRight != null) {
                room.RoomData.Right = roomRight.RoomData;
            }

            room.Render();
        }
    }

    private Room GenerateRoom(Vector2Int at, Texture2D roomTemplate) {
        var room = Instantiate(_roomPrefab);
        var roomData = new RoomData {
            Room = room,
            Location = at,
            Template = roomTemplate
        };
        room.RoomData = roomData;

        return room;
    }

    //private RoomData GenerateRoom(int roomsLeft, RoomData previousRoom, Vector2Int location, Texture2D forceRoomTemplate = null) {
    //    if (roomsLeft <= 0) {
    //        return null;
    //    }

    //    var room = Instantiate(_roomPrefab);
    //    var roomTemplate = forceRoomTemplate;
    //    if (roomTemplate == null) {
    //        roomTemplate = _roomTemplates[UnityEngine.Random.Range(0, _roomTemplates.Length)];
    //    }

    //    var branchingRandomizer = UnityEngine.Random.Range(0f, 1f);
    //    var branches = 1;
    //    if (branchingRandomizer > .9) {
    //        branches = 4;
    //    } else if (branchingRandomizer > .7) {
    //        branches = 3;
    //    } else if (branchingRandomizer > .4) {
    //        branches = 2;
    //    }

    //    var roomData = new RoomData {
    //        Location = location,
    //        Room = room,
    //    };

    //    room.RoomData = roomData;
    //    room.Render(roomTemplate);

    //    // Bind to parent
    //    if (previousRoom != null) {
    //        if (previousRoom.Location - location == Vector2Int.up) {
    //        roomData.Up = previousRoom;
    //    }
    //    if (previousRoom.Location - location == Vector2Int.down) {
    //        roomData.Down = previousRoom;
    //    }
    //    if (previousRoom.Location - location == Vector2Int.left) {
    //        roomData.Left = previousRoom;
    //    }
    //    if (previousRoom.Location - location == Vector2Int.right) {
    //        roomData.Right = previousRoom;
    //    }
    //    }

    //    // Generate branches
    //    for (var i = 0; i < branches; i++) {
    //        var nextRoomLocation = GetRandomDirectionForRoom(roomData);
    //        if (!nextRoomLocation.HasValue) {
    //            break;
    //        }

    //        var childRoom = GenerateRoom(roomsLeft - 1, roomData, nextRoomLocation.Value);

    //        if (nextRoomLocation == Vector2Int.up) {
    //            roomData.Up = childRoom;
    //        }
    //        if (nextRoomLocation == Vector2Int.down) {
    //            roomData.Down = childRoom;
    //        }
    //        if (nextRoomLocation == Vector2Int.left) {
    //            roomData.Left = childRoom;
    //        }
    //        if (nextRoomLocation == Vector2Int.right) {
    //            roomData.Right = childRoom;
    //        }
    //    }

    //    _rooms.Add(roomData);

    //    return roomData;
    //}

    //private Vector2Int? GetRandomDirectionForRoom(RoomData room) {
    //    var directions = GetShuffledDirections();
    //    for (var i = 0; i < directions.Length; i++) {
    //        var direction = directions[i];

    //        if (direction == Vector2Int.up && room.Up == null) {
    //            return direction;
    //        }
    //        if (direction == Vector2Int.down && room.Down == null) {
    //            return direction;
    //        }
    //        if (direction == Vector2Int.left && room.Left == null) {
    //            return direction;
    //        }
    //        if (direction == Vector2Int.right && room.Right == null) {
    //            return direction;
    //        }
    //    }

    //    return null;
    //}

    //private Vector2Int[] GetShuffledDirections() {
    //    var directions = new Vector2Int[] {
    //        Vector2Int.up,
    //        Vector2Int.down,
    //        Vector2Int.left,
    //        Vector2Int.right,
    //    };

    //    return directions.OrderBy(a => Guid.NewGuid()).ToArray();
    //}
}
