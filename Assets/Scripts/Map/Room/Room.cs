using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static IEnemy;

public enum RoomType
{
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
public enum Direction
{
    Up, Down, Left, Right
}

public class Room : MonoBehaviour
{
    [SerializeField] private SpriteRenderer floorRenderer;
    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;
    [SerializeField] private Vector2 _margin = new Vector2(2f, 2f);
    [SerializeField] private GameObject _itemPivot;
    [SerializeField] private GameObject[] _items;

    private bool _isEnemiesSpawn = false; // 중복을 막자
    public int _totalEnemyCount = 0; // 전체 몹 갯수
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
            int idx = UnityEngine.Random.Range(0, enemyLists.Count);
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
        foreach (Door d in GetComponentsInChildren<Door>(true))
        {
            doors[d.direction] = d;
            doors[d.direction].currentRoomPos = this.position;
        }
        SpawnEnemies();
    }

    public void SetMapSprite()
    {
        if (this.type == RoomType.Boss)
        {
            if (Resources.Load<Sprite>("ArtWork/Boss") == null)
                Debug.Log("boss map 찾을 수 없음");
            floorRenderer.sprite = Resources.Load<Sprite>("Map/Boss");

            Vector3 scale = floorRenderer.transform.localScale;
            scale.x = 1.21f;
            floorRenderer.transform.localScale = scale;
        }
        else
        {
            if (Resources.Load<Sprite>("ArtWork/NormalMap2") == null)
                Debug.Log("map 찾을 수 없음");
            floorRenderer.sprite = Resources.Load<Sprite>("Map/NormalMap2");
        }

    }

    public void AddDoor(Direction dir)
    {
        if (doors.TryGetValue(dir, out Door door))
        {
            door.SetDoor();
            door.isNeighbor = true;
        }

        SetCollision(dir);
    }

    public void DeleteDoor(Direction dir)
    {
        doors[dir].DelteDoor();
    }

    public void CheckedDoor()
    {
        Room room = gameObject.GetComponent<Room>();
        if (_totalEnemyCount == 0)
        {
            foreach (var d in room.GetComponentsInChildren<Door>())
            {
                d.DoorOpen();
            }
        }
        else
        {
            foreach (var d in room.GetComponentsInChildren<Door>())
            {
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
        _totalEnemyCount = 0;
        Vector3 center = (_minBounds + _maxBounds) / 2f;
        // 그룹별 분리
        var eliteEnemies = _enemyList.Where(e => e.type == EnemyType.Elite1 || e.type == EnemyType.Elite2 || e.type == EnemyType.Boss).ToList();
        var normalEnemies = _enemyList.Except(eliteEnemies).ToList();

        int eliteCount = eliteEnemies.Sum(e => e.count);
        int normalCount = normalEnemies.Sum(e => e.count);
        Debug.Log($"{eliteCount} {normalCount}");
        // 각 그룹별 오프셋 생성
        List<Vector2> eliteOffsets = GetElitePatternOffsets(eliteCount);
        List<Vector2> normalOffsets = GetPatternOffsets(normalCount);

        System.Random random = new System.Random();
        List<Vector2> randomizedNormalOffsets = normalOffsets.OrderBy(x => random.Next()).ToList();

        // 엘리트 몹 배치
        int eliteOffsetIndex = 0;
        foreach (var enemy in eliteEnemies)
        {
            _totalEnemyCount += enemy.count;
            for (int i = 0; i < enemy.count; i++)
            {
                Vector2 offset = eliteOffsets[eliteOffsetIndex++];
                Vector3 spawnPos = center + (Vector3)(offset * 1.5f);
                SpawnEnemyByType(enemy.type, enemy.type.ToString(), spawnPos);
            }
        }

        // 일반 몹 배치
        int normalOffsetIndex = 0;
        foreach (var enemy in normalEnemies)
        {
            _totalEnemyCount += enemy.count;
            for (int i = 0; i < enemy.count; i++)
            {
                //Debug.Log($"<color=red>{enemy.count}</color>");
                Vector2 offset = randomizedNormalOffsets[normalOffsetIndex++];
                Vector3 spawnPos = center + (Vector3)(offset * 1.5f);
                SpawnEnemyByType(enemy.type, enemy.type.ToString(), spawnPos);
            }
        }
    }

    private void SpawnEnemyByType(EnemyType type, string poolKey, Vector3 spawnPos)
    {
        switch (type)
        {
            case EnemyType.Flee:
                SpawnEnemy(PoolManager.Instance.Get<FleeEnemy>(poolKey), spawnPos);
                break;
            case EnemyType.Ranged:
                SpawnEnemy(PoolManager.Instance.Get<RangedEnemy>(poolKey), spawnPos);
                break;
            case EnemyType.Boss:
                SpawnEnemy(PoolManager.Instance.Get<Boss>(poolKey), spawnPos);
                break;
            case EnemyType.Normal:
                SpawnEnemy(PoolManager.Instance.Get<MoveEnemy>(poolKey), spawnPos);
                break;
            case EnemyType.Teleport:
                SpawnEnemy(PoolManager.Instance.Get<TeleportEnemy>(poolKey), spawnPos);
                break;
            case EnemyType.Rush1:
            case EnemyType.Rush2:
                SpawnEnemy(PoolManager.Instance.Get<RushEnemy>(poolKey), spawnPos);
                break;
            case EnemyType.Explode:
                SpawnEnemy(PoolManager.Instance.Get<ExplodeEnemy>(poolKey), spawnPos);
                break;
            case EnemyType.Elite1:
            case EnemyType.Elite2:
                SpawnEnemy(PoolManager.Instance.Get<ElitEnemy>(poolKey), spawnPos);
                break;
            case EnemyType.Minion:
                SpawnEnemy(PoolManager.Instance.Get<MinionEnemy>(poolKey), spawnPos);
                break;
        }
    }

    // spawn grid패턴으로 가져오기 엘리트용
    public List<Vector2> GetElitePatternOffsets(int count)
    {
        List<Vector2> offsets = new();
        switch (count)
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
                    float x = (i - (count - 1) / 2f) * 1.5f;
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
        if (enemy == null || spawnPoint == null) return;
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
        ChallengeManager.Instance.IncreaseProgress("kill_monster", 1);

        Debug.Log($"{_totalEnemyCount}");
        if (type == RoomType.Boss && _totalEnemyCount == 0)
        {
            ChallengeManager.Instance.IncreaseProgress("1_stage_boss_clear", 1);

            Invoke(nameof(GoToMainSence), 3f);
        }
        else if(_totalEnemyCount == 0) 
        {
            // 다 없어진 경우
            //문 열리게끔
            foreach (var d in room.GetComponentsInChildren<Door>())
            {
                d.DoorOpen();
            }

            // null이 아닌 아이템만 필터링
            var availableItems = new List<GameObject>();
            foreach (var item in _items)
            {
                if (item != null)
                    availableItems.Add(item);
            }

            if (availableItems.Count > 0 && _itemPivot != null)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableItems.Count);
                GameObject selectedItem = availableItems[randomIndex];
                Instantiate(selectedItem, _itemPivot.transform.position, Quaternion.identity);

                // 원본 배열에서 제거하려면:
                for (int i = 0; i < _items.Length; i++)
                {
                    if (_items[i] == selectedItem)
                    {
                        _items[i] = null;
                        break;
                    }
                }
            }

        }
    }

    private void GoToMainSence()
    {
        SceneManager.LoadScene("Save");
    }
}
