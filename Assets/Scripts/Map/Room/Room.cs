using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public enum RoomType {
    Start,
    Normal,
    Treasure,
    Boss
}

public enum Direction {
    Up, Down, Left, Right
}

public class Room : MonoBehaviour
{
    // Map 상의 위치 좌표
    public Vector2Int position;

    public RoomType type;

    // 문 방향 정보
    public Dictionary<Direction, Door> doors = new();

    public GameObject instance;

    public void Init(Vector2Int pos, RoomType type = RoomType.Normal) {
        this.position = pos;
        this.type = type;
    }

    public void RegisterDoors()
    {
        doors.Clear();
        foreach (Door d in GetComponentsInChildren<Door>(true)) {
            doors[d.direction] = d;
        }
    }


    public void AddDoor(Direction dir) {
        if (doors.TryGetValue(dir, out Door door)) {
            door.SetDoor();
            door.isNeighbor = true;
        }

        SetCollision(dir);
    }

    public void DeleteDoor(Direction dir) {
        doors[dir].DoorClose();
    }

    public bool CheckedNeighbor(Direction dir) {
        return doors[dir].isNeighbor;
    }

    void SetCollision(Direction dir) {
        Transform collision = transform;

        if (dir == Direction.Up) collision = transform.Find("Grid/CollisionU");
        else if (dir == Direction.Down) collision = transform.Find("Grid/CollisionD");
        else if (dir == Direction.Left) collision = transform.Find("Grid/CollisionL");
        else if (dir == Direction.Right) collision = transform.Find("Grid/CollisionR");

        collision.gameObject.SetActive(false);       
    }
}
