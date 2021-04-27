using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVisibilityManager : MonoBehaviour {
    [SerializeField] private Camera _camera;

    public static RoomVisibilityManager Instance { get; private set; }

    private Room _previousRoom;
    private Room _currentRoom;

    private void Awake() {
        Instance = this;
    }

    public void PlayerEntersNewRoom(Room room) {
        _previousRoom = _currentRoom;
        _currentRoom = room;

        UpdateRoomVisibility();
        MoveCameraToCurrentRoom();
    }

    public void PlayerExitsRoom(Room room) {
        //if (room == _currentRoom) {
        //    _currentRoom = _previousRoom;

        //    MoveCameraToCurrentRoom();
        //    UpdateRoomVisibility();
        //}
    }

    private void UpdateRoomVisibility() {
        if (_previousRoom != null) {
            _previousRoom.Hide();
            _previousRoom.PutMonstersToSleep();
        }

        if (_currentRoom != null) {
            _currentRoom.Show();
            _currentRoom.WakeMonsters();
        }
    }

    private void MoveCameraToCurrentRoom() {
        _camera.transform.DOMove(new Vector3(_currentRoom.transform.position.x, _currentRoom.transform.position.y, _camera.transform.position.z), .25f, true);
    }

    private void OnDrawGizmos() {
        if (_currentRoom != null) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(_currentRoom.transform.position, Vector3.one);
        }

        if (_previousRoom != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_previousRoom.transform.position, Vector3.one);
        }
    }
}
