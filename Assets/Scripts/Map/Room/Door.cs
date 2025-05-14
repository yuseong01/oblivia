using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Door : MonoBehaviour
{
    public Direction direction;
    public bool isNeighbor = false;
    public Animator animator;

    public Vector2Int currentRoomPos;
    public Vector2Int nextRoomPos;

    // 몬스터 spwan 됐는지 체크
    private bool hasSpawned = false;
    public List<RoomSpawn> spawnPoints;
    void Awake() 
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        SetDir();
        currentRoomPos = GetComponentInParent<Room>().position;
        nextRoomPos = RoomManager.Instance.GetNextRoomPos(direction);
    }

    public void SetDoor() 
    {
        gameObject.SetActive(true);
    }

    public void DelteDoor() 
    {
        gameObject.SetActive(false);
    }

    public void DoorOpen() 
    {
        // 몬스터 모두 처치 시 문 오픈
        // 아마 추후 애니메이션 처리할 듯
        
        // 몬스터 모두 처치한 뒤 문 열리는 애니메이션 (IsOpen) 실행
        animator.CrossFade("Open", 0f);
        // 문 trigger 모두 설정
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void DoorClose() 
    {
        // 방에 들어갔을 때, 문 클로즈
        // 아마 추후 애니메이션 처리할 듯
        
        // Player가 Spawn된 뒤 문 trigger 모두 해제
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    // 현재 문이 어느 방향 문인지
    void SetDir() 
    {
        UnityEngine.Vector2 pos = transform.localPosition;

        if (Mathf.Abs(pos.x) > Mathf.Abs(pos.y)) {
            direction = (pos.x > 0) ? Direction.Right : Direction.Left;
        }
        else {
            direction = (pos.y > 0) ? Direction.Up : Direction.Down;
        }
    }

    UnityEngine.Vector3 GetNextRoomCenter() 
    {
        return new UnityEngine.Vector3(nextRoomPos.x * 20f, nextRoomPos.y * 12f, 0f); 
    }

    UnityEngine.Vector3 GetPlayerSpawnPosition(Direction enterDir, UnityEngine.Vector3 nextRoomCenter) 
    {
        UnityEngine.Vector3 spawnPos = nextRoomCenter;

        // Player가 나올 문은 현재 문의 반대
        Direction nextDoorDir = RoomManager.Instance.Opposite(direction);

        if(nextDoorDir == Direction.Up) 
            spawnPos = nextRoomCenter + new UnityEngine.Vector3(0f, 3.5f, 0f);
        else if (nextDoorDir == Direction.Down)
            spawnPos = nextRoomCenter + new UnityEngine.Vector3(0f, -3f, 0f);
        else if (nextDoorDir == Direction.Left) 
            spawnPos = nextRoomCenter + new UnityEngine.Vector3(-7f, 0f, 0f);
        else if (nextDoorDir == Direction.Right)
            spawnPos = nextRoomCenter + new UnityEngine.Vector3(6.5f, 0f, 0f);

        return spawnPos;
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) {
            // 다음 방의 센터 좌표 계산
            nextRoomPos = RoomManager.Instance.GetNextRoomPos(this.direction);
            UnityEngine.Vector3 nextRoomCenter = GetNextRoomCenter();

            // 플레이어가 다음 방에서 나와야 하는 방향 계산
            Direction entryDirInNextRoom = RoomManager.Instance.Opposite(this.direction);
            UnityEngine.Vector3 spawnPos = GetPlayerSpawnPosition(entryDirInNextRoom, nextRoomCenter);

            // 플레이어 위치 이동
            other.transform.position = spawnPos;

            // 카메라 이동
            CameraController.Instance.MoveRoom(nextRoomCenter);

            // currentRoomPos 갱신
            RoomManager.Instance.SetCurrentRoom(nextRoomPos); 
            Debug.Log(RoomManager.Instance.GetRoomType());
        }
    }

}
