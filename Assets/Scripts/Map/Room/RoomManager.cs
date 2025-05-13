using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    // room prefab
    public GameObject room;
    // 생성된 방 좌표를 키 값으로 방 저장
    public Vector2Int currentRoomPos;
    // 방 총 개수
    int roomCount;
    // 만든 방 count
    int createRoomCount = 0;

    private Dictionary<Vector2Int, GameObject> roomInstances = new();

    // 방향 값을 좌표 값으로 
    private static readonly List<Vector2Int> directions = new()
    {
        new Vector2Int(0, 1),   // U
        new Vector2Int(0, -1),  // D
        new Vector2Int(-1, 0),  // L
        new Vector2Int(1, 0)    // R
    };


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        // 3. 방 생성
        GenerateRooms();
        SetRoomType();

        foreach (var r in roomInstances) 
        {
            Room room = r.Value.GetComponent<Room>();
            room.RegisterDoors();

            // 적 스폰
           
        }

        // 문 생성
        ConnectDoor(roomInstances);
    }

    public void SetCurrentRoom(Vector2Int pos) 
    {
        currentRoomPos = pos;
    }

    public Vector2Int GetCurrentRoom() {
        return currentRoomPos;
    }

    public Vector2Int GetNextRoomPos(Direction dir) 
    {
        return currentRoomPos + DirectionToVector2Int(dir);
    }

    public RoomType GetRoomType() {
        RoomType currentRoomType = roomInstances[currentRoomPos].GetComponent<Room>().type;
        return currentRoomType;
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
                Room roomComponent = newRoom.GetComponent<Room>();
                Debug.Log(createRoomCount + " : " + currentPos);
                // 1. 룸 타입 설정
                RoomType randomType = RoomType.Normal;
                // 2. 초기화
                roomInstances[currentPos] = newRoom;
                createRoomCount++;
            }
            currentPos += GetRandomDirection();
        }
    }

    Vector2Int GetRandomDirection()
    {
        // 위, 아래, 왼, 오 중에 랜덤으로 좌표 get 
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        return dirs[UnityEngine.Random.Range(0, dirs.Length)];
    }

    // 각 방 순회하며 문 연결 수행
    void ConnectDoor(Dictionary<Vector2Int, GameObject> roomInstances) 
    {
        foreach (var r in roomInstances) {
            Vector2Int pos = r.Key;
            Room room = r.Value.GetComponent<Room>();
            room.position = pos;

            foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
                Vector2Int neighborPos = pos + DirectionToVector2Int(dir);

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

    void SetRoomType() {
        // 우선 모든 방을 Normal로
        foreach (var r in roomInstances) {
            Vector2Int pos = r.Key;
            Room room = r.Value.GetComponent<Room>();

            room.type = RoomType.Normal;
        }
        
        // Start Room 지정
        roomInstances[new Vector2Int(0, 0)].GetComponent<Room>().type = RoomType.Start;

        // 가장 먼 방 Boss Room
        Vector2Int farthestPos = FindFarthestRoom(new Vector2Int(0,0), roomInstances);
        roomInstances[farthestPos].GetComponent<Room>().type = RoomType.Boss;

        foreach (var r in roomInstances)
        {
            Room room = r.Value.GetComponent<Room>();
            room.TypeInit(room.type);
        }
    }

    Vector2Int FindFarthestRoom(Vector2Int start, Dictionary<Vector2Int, GameObject> roomInstances)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        Vector2Int farthest = start;

        queue.Enqueue(start);
        visited.Add(start);

        // BFS
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            farthest = current; // 마지막에 방문한 방

            // 4방향 모두 확인
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = current + dir;
                // 현재 방의 이웃이면서 방문한 적 없으면
                if (roomInstances.ContainsKey(neighbor) && !visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return farthest;
    }


    // Vector2Int는 논리 좌표로 Unity 월드 좌표로 변환하는 과정 필요
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * 20f, gridPos.y * 12f, 0f); 
    }

    // 방향을 좌표로
    public Vector2Int DirectionToVector2Int(Direction dir)
    {
        return dir switch {
            Direction.Up => Vector2Int.up,
            Direction.Down => Vector2Int.down,
            Direction.Left => Vector2Int.left,
            Direction.Right => Vector2Int.right,
            _ => Vector2Int.zero
        };
    }

    // 방향을 벡터로
    public Vector3 DirectionToVector(Direction dir)
    {
        return dir switch
        {
            Direction.Up => Vector3.up,
            Direction.Down => Vector3.down,
            Direction.Left => Vector3.left,
            Direction.Right => Vector3.right,
            _ => Vector3.zero
        };
    }

    // 반대쪽
    public Direction Opposite(Direction dir)
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
