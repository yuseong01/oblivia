using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    // void Start() {
    //     // 문 등록
    //     foreach (Door d in GetComponentsInChildren<Door>()) {
    //         doors[d.direction] = d;
    //     }
    // }

    public void RegisterDoors()
    {
        doors.Clear();
        foreach (Door d in GetComponentsInChildren<Door>(true)) {
            doors[d.direction] = d;
            Debug.Log($"[Room] {d.direction} 방향 문 등록됨");
        }
    }


    public void AddDoor(Direction dir) {
        if (doors.TryGetValue(dir, out Door door)) {
            door.SetDoor();
            door.isNeighbor = true;
        } else {
            Debug.LogWarning($"[Room] {dir} 방향 문 없음 - AddDoor 실패");
        }
    }

    public void DeleteDoor(Direction dir) {
        doors[dir].DoorClose();
    }

    public bool CheckedNeighbor(Direction dir) {
        return doors[dir].isNeighbor;
    }
    // public bool HasDoor(Direction dir)
    // {
    //     return doors.Contains(dir);
    // }
}
