using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CloneState<T> : IState<T> where T : MonoBehaviour, IEnemy, IStateMachineOwner<T>
{
    private int _numberOfClones = 4;             // ������ �н� ��
    private float _spacing = 2f;               // �н� �� ����
    private float _cloneDuration = 3f;           // ��ü ���� �ð� (���ϸ� ���� ����)
    private float _timer;

    private List<GameObject> spawnedClones = new List<GameObject>();

    public void Enter(T obj)
    {
        _timer = 0f;
        obj.ChangeState(new IdleState<T>());
        GameObject clonePrefab = (obj as Boss)?.GetClonePrefab();
        if (clonePrefab == null)
        { 
            return;
        }

        Vector3 center = obj.transform.position;
        List<Vector3> positions = GetLinearPositions(center, _numberOfClones, _spacing);

        foreach (var pos in positions)
        {
            GameObject cloneObj = GameObject.Instantiate(clonePrefab, pos, Quaternion.identity);
            var cloneScript = cloneObj.GetComponent<CloneEnemy>();
            spawnedClones.Add(cloneObj);
        }
    }

    public void Update(T obj)
    {
        // Ŭ�е� ��� IdleState�� ����
        if (_timer >= _cloneDuration)
        {
            obj.SetSpeed(Random.Range(2f, _numberOfClones));
            obj.ChangeState(new IdleState<T>());
        }
    }
    public void Exit(T obj)
    {
        spawnedClones.Clear();
    }

    private List<Vector3> GetLinearPositions(Vector3 center, int count, float spacing)
    {
        List<Vector3> positions = new List<Vector3>();
        float totalWidth = (count - 1) * spacing;
        Vector3 origin = center - new Vector3(totalWidth / 2f, 0f, 0f); // X�� ���� ����

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = origin + new Vector3(i * spacing, 0f, 0f);
            positions.Add(pos);
        }

        return positions;
    }
}
