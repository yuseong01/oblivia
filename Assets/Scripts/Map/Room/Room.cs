using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using static IEnemy;

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
    [SerializeField] private Transform[] _spawnPoints;
    private List<EnemySpawnInfo> _enemyList; // 룸이 가진 적 정보
    public void Init(Vector2Int pos, RoomType type = RoomType.Normal) 
    {
        this.position = pos;
        this.type = type;

    }

    public void TypeInit(RoomType type = RoomType.Normal)
    {
        if (PoolManager.Instance.TryGetSpawnData(type, out var enemyLists) && enemyLists.Count > 0)
        {
            int idx = Random.Range(0, enemyLists.Count);
            _enemyList = enemyLists[idx];
        }
        else
        {
            _enemyList = new List<EnemySpawnInfo>();
        }
    }
    public void RegisterDoors()
    {
        doors.Clear();
        foreach (Door d in GetComponentsInChildren<Door>(true)) {
            doors[d.direction] = d;
            doors[d.direction].currentRoomPos = this.position;
        }
        SpawnEnemies();
    }


    public void AddDoor(Direction dir) 
    {
        if (doors.TryGetValue(dir, out Door door)) {
            door.SetDoor();
            door.isNeighbor = true;
        }

        SetCollision(dir);
    }

    public void DeleteDoor(Direction dir) 
    {
        doors[dir].DelteDoor();
    }

    public bool CheckedNeighbor(Direction dir) 
    {
        return doors[dir].isNeighbor;
    }

    void SetCollision(Direction dir) 
    {
        Transform collision = transform;

        if (dir == Direction.Up) collision = transform.Find("Grid/CollisionU");
        else if (dir == Direction.Down) collision = transform.Find("Grid/CollisionD");
        else if (dir == Direction.Left) collision = transform.Find("Grid/CollisionL");
        else if (dir == Direction.Right) collision = transform.Find("Grid/CollisionR");

        collision.gameObject.SetActive(false);       
    }

    public void SpawnEnemies()
    {
        if (_enemyList == null || _enemyList.Count == 0)
        {
            // 없을 경우 return
            Debug.Log("enemyList 없음");
            return;
        }
        foreach (var enemy in _enemyList)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                // 풀에서 꺼내서 위치 설정
                string poolKey = enemy.poolKey;
              
                switch (enemy.type)
                {
                    case EnemyType.Flee:
                        var flee = PoolManager.Instance.Get<FleeEnemy>(poolKey);
                        SpawnEnemy(flee, _spawnPoints);
                        break;

                    case EnemyType.Ranged:
                        var ranged = PoolManager.Instance.Get<RangedEnemy>(poolKey);
                        SpawnEnemy(ranged, _spawnPoints);
                        break;

                    case EnemyType.Boss:
                        var boss = PoolManager.Instance.Get<Boss>(poolKey);
                        SpawnEnemy(boss, _spawnPoints);
                        break;
                }
            }
        }

    }
    private void SpawnEnemy<T>(BaseEnemy<T> enemy, Transform[] spawnPoints) where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>, IPoolable
    {
        if (enemy == null || spawnPoints == null || spawnPoints.Length == 0) return;

        // 스폰 위치 랜덤 선택
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 적 위치 설정
        GameObject go = enemy.gameObject;
        go.transform.position = spawnPoint.position;
        go.transform.rotation = Quaternion.identity;

        // 활성화
        go.SetActive(true);
    }
}
