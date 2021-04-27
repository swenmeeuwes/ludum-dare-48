using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData {
    public Room Room { get; set; }
    public Vector2Int Location { get; set; }
    public Texture2D Template { get; set; }

    public RoomData Up { get; set; }
    public RoomData Down { get; set; }
    public RoomData Left { get; set; }
    public RoomData Right { get; set; }
}
