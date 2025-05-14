using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    [SerializeField] private GameObject _orbitPrefab;
    [SerializeField] private GameObject _orbitPivot;
     private int _objectCount;
    [SerializeField] private float _radius = 1f;
    [SerializeField] private float _rotationSpeed = 50f;

    private List<Transform> _orbitingObjects = new List<Transform>();
    private List<float> _angles = new List<float>(); // 각도 정보 저장

    [SerializeField] private PlayerStatHandler _playerStatHandler;
    [SerializeField] private ItemEffectManager _itemEffects;

    private void Awake()
    {
        _playerStatHandler = GetComponent<PlayerStatHandler>();
        _itemEffects = GetComponent<ItemEffectManager>();
    }

    void OnEnable()
    {
        _playerStatHandler.OnOrbitCountChanged += CreateOrbitingObjects;
    }

    void OnDisable()
    {
        _playerStatHandler.OnOrbitCountChanged -= CreateOrbitingObjects;
    }

    public void CreateOrbitingObjects()
    {
        _objectCount = _playerStatHandler.OrbitCount;

        foreach (var obj in _orbitingObjects)
        {
            Destroy(obj.gameObject);
        }
        _orbitingObjects.Clear();
        _angles.Clear();

        for (int i = 0; i < _objectCount; i++)
        {
            float angle = 360f / _objectCount * i;
            _angles.Add(angle); // 초기 각도 저장

            Vector3 pos = _orbitPivot.transform.position + Quaternion.Euler(0, 0, angle) * Vector3.right * _radius;
            GameObject orbitObj = Instantiate(_orbitPrefab, pos, Quaternion.identity);
            var controller = orbitObj.GetComponent<AttackController>();
            controller.ItemEffectManager = _itemEffects;
            controller.StatHandler = _playerStatHandler;
            _orbitingObjects.Add(orbitObj.transform);
        }
    }

    void Update()
    {
        UpdateOrbitPositions();
    }

    void UpdateOrbitPositions()
    {
        for (int i = 0; i < _orbitingObjects.Count; i++)
        {
            _angles[i] += _rotationSpeed * Time.deltaTime; // 각도 갱신
            float angleRad = _angles[i] * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * _radius;
            _orbitingObjects[i].position = _orbitPivot.transform.position + offset;
        }
    }
}
