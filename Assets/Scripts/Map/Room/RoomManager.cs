using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // room prefab
    public GameObject room;
    // 방 총 개수
    public int roomCount;
    // 만든 방 count
    public int createRoomCount = 0;
    // 생성된 방 좌표를 키 값으로 방 저장
    private Dictionary<Vector2Int, GameObject> roomInstances = new();


    void Start()
    {
        // 방 생성
        GenerateRooms();

        foreach (var r in roomInstances) {
            Room room = r.Value.GetComponent<Room>();
            room.RegisterDoors();
        }

        // 문 생성
        ConnectDoor(roomInstances);
    }

    void GenerateRooms()
    {
        // 시작 지점 (0,0)
        Vector2Int currentPos = Vector2Int.zero; 
        // 방 개수 랜덤 지정
        roomCount = UnityEngine.Random.Range(8, 12);

        while (createRoomCount < roomCount)
        {
            // 방이 없는 좌표에만 방 생성
            if (!roomInstances.ContainsKey(currentPos))
            {
                GameObject newRoom = Instantiate(room, GridToWorld(currentPos), Quaternion.identity, transform);
                Debug.Log(createRoomCount+ " : " + currentPos );
                roomInstances[currentPos] = newRoom;
                createRoomCount++;
            }
            currentPos += GetRandomDirection();
        }
    }

    // Vector2Int는 논리 좌표로 Unity 월드 좌표로 변환하는 과정 필요
    Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * 20f, gridPos.y * 12f, 0f); 
    }

    Vector2Int GetRandomDirection()
    {
        // 위, 아래, 왼, 오 중에 랜덤으로 좌표 get 
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        return dirs[UnityEngine.Random.Range(0, dirs.Length)];
    }

    // 각 방 순회하며 문 연결 수행
    void ConnectDoor(Dictionary<Vector2Int, GameObject> roomInstances) {
        foreach (var r in roomInstances) {
            Vector2Int pos = r.Key;
            Room room = r.Value.GetComponent<Room>();

            foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
                Vector2Int neighborPos = pos + DirectionToVector(dir);

                if (roomInstances.ContainsKey(neighborPos)) {
                    Room neighborRoom = roomInstances[neighborPos].GetComponent<Room>();
                    room.AddDoor(dir);
                    neighborRoom.AddDoor(Opposite(dir));
                }                
            }

            foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
                if (!room.CheckedNeighbor(dir)) {
                    room.DeleteDoor(dir);
                }
            }
        }
    }

    // 방향을 벡터로
    Vector2Int DirectionToVector(Direction dir)
    {
        return dir switch {
            Direction.Up => Vector2Int.up,
            Direction.Down => Vector2Int.down,
            Direction.Left => Vector2Int.left,
            Direction.Right => Vector2Int.right,
            _ => Vector2Int.zero
        };
    }

    // 반대쪽
    Direction Opposite(Direction dir)
    {
        return dir switch {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => dir
        };
    }

}
