using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
public enum SpawnPattern
{
    Line,
    Triangle,
    Diamond,
    Circle,
    Random // 그냥 그리드에서 뽑는 경우
}
public enum Direction {
    Up, Down, Left, Right
}

public class Room : MonoBehaviour
{
    [SerializeField] private SpriteRenderer floorRenderer;
    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;
    [SerializeField] private Vector2 _margin = new Vector2(2f, 2f);
    private bool _isEnemiesSpawn = false; // 중복을 막자
    private int _totalEnemyCount = 0; // 전체 몹 갯수

    public int TotalEnemyCount => _totalEnemyCount;
    int spawnCount = 0;
    public Vector2 GetMinBounds() => _minBounds;
    public Vector2 GetMaxBounds() => _maxBounds;
    // Map 상의 위치 좌표
    public Vector2Int position;

    public RoomType type;
    // margin 설정
    public void SetMargin(Vector2 margin)
    {
        _margin = margin;
    }
    // 문 방향 정보
    public Dictionary<Direction, Door> doors = new();

    public GameObject instance;
    
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
            Debug.Log($"[Room: {position}] enemyList.Count = {_enemyList.Count}");
        }
        else
        {
            Debug.LogWarning($"[Room: {position}] enemyList 없음! RoomType: {type}");
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

    public void SetMapSprite() {
        if (this.type == RoomType.Boss) {
            if (Resources.Load<Sprite>("ArtWork/Boss") == null)
                Debug.Log("boss map 찾을 수 없음");
            floorRenderer.sprite = Resources.Load<Sprite>("Map/Boss");

            Vector3 scale = floorRenderer.transform.localScale;
            scale.x = 1.21f;
            floorRenderer.transform.localScale = scale;
        }
        else {
            if (Resources.Load<Sprite>("ArtWork/NormalMap2") == null)
                Debug.Log("map 찾을 수 없음");
            floorRenderer.sprite = Resources.Load<Sprite>("Map/NormalMap2");
        }

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

    public void CheckedDoor() {
        Room room = gameObject.GetComponent<Room>();
        if (_totalEnemyCount == 0) {
            foreach(var d in room.GetComponentsInChildren<Door>()) {
                d.DoorOpen();
            }
        }
        else {
            foreach(var d in room.GetComponentsInChildren<Door>()) {
                d.DoorClose();
            }
        }
            
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
        if (_isEnemiesSpawn) return;
        _isEnemiesSpawn = true;
        if (_enemyList == null || _enemyList.Count == 0)
        {
            // 없을 경우 return
            Debug.Log("enemyList 없음");
            return;
        }
        Vector3 center = (_minBounds + _maxBounds) / 2f;
        foreach (var enemy in _enemyList)
        {

            spawnCount = enemy.count;
            _totalEnemyCount += spawnCount; // 몹 갯수 누적 저장
            List<Vector2> offsets;
            if (enemy.type == EnemyType.Elite || enemy.type == EnemyType.Boss)
                offsets = GetElitePatternOffsets(spawnCount);
            else
                offsets = GetPatternOffsets(spawnCount);
                for (int i = 0; i < spawnCount; i++)
                {
                    // 풀에서 꺼내서 위치 설정
                    string poolKey = enemy.poolKey;
                    Vector2 offset = offsets.Count > i ? offsets[i] : Random.insideUnitCircle;
                    Vector3 spawnPos = center + (Vector3)(offset * 1.5f); // 간격 1.5f


                    switch (enemy.type)
                    {
                        case EnemyType.Flee:
                            var flee = PoolManager.Instance.Get<FleeEnemy>(poolKey);
                            SpawnEnemy(flee, spawnPos);
                            break;

                        case EnemyType.Ranged:
                            var ranged = PoolManager.Instance.Get<RangedEnemy>(poolKey);
                            SpawnEnemy(ranged, spawnPos);
                            break;

                        case EnemyType.Boss:
                            var boss = PoolManager.Instance.Get<Boss>(poolKey);
                            SpawnEnemy(boss, spawnPos);
                            break;
                        case EnemyType.Normal:
                            var normal = PoolManager.Instance.Get<MoveEnemy>(poolKey);
                            SpawnEnemy(normal, spawnPos);
                            break;
                        case EnemyType.Teleport:
                            var teleport = PoolManager.Instance.Get<TeleportEnemy>(poolKey);
                            SpawnEnemy(teleport, spawnPos);
                            break;
                    }
                }
        }

    }

    // spawn grid패턴으로 가져오기 엘리트용
    public List<Vector2> GetElitePatternOffsets(int count)
    {
        List<Vector2> offsets = new();
        switch(count)
        {
            case 1:
                offsets.Add(new Vector2(0, 0)); 
                break;
            case 2:
                offsets.Add(new Vector2(-0.7f, 0));
                offsets.Add(new Vector2(0.7f, 0));
                break;
        }
        return offsets;
    }
    // spawn grid패턴으로 가져오기 잡몬용
    public List<Vector2> GetPatternOffsets(int count)
    {
        List<Vector2> offsets = new();

        switch (count)
        {
            case 2: // 일렬로
                for (int i = 0; i < count; i++)
                {
                    float x = (i - (count - 1)/2f) * 1.5f;
                    offsets.Add(new Vector2(x, 0));
                }
                break;

            case 3:// 삼각형
                offsets.Add(new Vector2(0, 2));
                offsets.Add(new Vector2(-2, 0));
                offsets.Add(new Vector2(2, 0));
                break;

            case 4: // 다이아몬드
                offsets.Add(new Vector2(0, 2));
                offsets.Add(new Vector2(-2, 0));
                offsets.Add(new Vector2(2, 0));
                offsets.Add(new Vector2(0, -2));
                break;

            default: // 둥근원
                for (int i = 0; i < count; i++)
                {
                    float angle = (360f / count) * i * Mathf.Deg2Rad;
                    offsets.Add(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 1.2f);
                }
                break;
        }

        return offsets;
    }
    private void SpawnEnemy<T>(BaseEnemy<T> enemy, Vector3 spawnPoint) where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>, IPoolable
    {
        if (enemy == null || spawnPoint == null ) return;

        // 적 위치
        GameObject go = enemy.gameObject;
        go.transform.position = spawnPoint;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);

        // 현재 방 연결
        enemy.SetCurrentRoom(this);
    }

    public void CalculateRoomBounds()
    {
        if (floorRenderer != null)
        {
            _margin = new Vector2(_margin.x, _margin.y);
            Bounds bounds = floorRenderer.bounds;
            _minBounds = new Vector2(bounds.min.x + _margin.x, bounds.min.y + _margin.y);
            _maxBounds = new Vector2(bounds.max.x - _margin.x, bounds.max.y - _margin.y);
        }
    }

    public void EnemyDied()
    {
        Room room = gameObject.GetComponent<Room>();

        _totalEnemyCount--;
        Debug.Log("죽었네...");
        if(type == RoomType.Boss &&  _totalEnemyCount == 0 )
        {
            Debug.Log("<color=red>메인화면으로</color>");
        }
        if(_totalEnemyCount > 0)
        {
            // 아직 있는 경우
        }
        else
        {
            // 다 없어진 경우
            //문 열리게끔
            foreach(var d in room.GetComponentsInChildren<Door>()) {
                d.DoorOpen();
            }

            Debug.Log("<color=yellow><b>이 방은 끝</b></color>");
        }
    }
}
