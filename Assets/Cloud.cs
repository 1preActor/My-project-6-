using UnityEngine;
using UnityEngine.Pool;

public class Cloud : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _poolCapaciti = 40;
    [SerializeField] private int _poolMaxSize = 100;
    [SerializeField] private int  _cloudHeight = 50;
    [SerializeField] private float _cloudSize = 10;

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => obj.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapaciti,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        float rainStartTime = 0f;
        float cubeRepeatTime = 0.05f;

        InvokeRepeating(nameof(GetCube), rainStartTime, cubeRepeatTime);
    }

    private void OnEnable()
    {
        Cube.CubeFalled += ReturnCubeInCloud;
    }

    private void OnDisable()
    {
        Cube.CubeFalled -= ReturnCubeInCloud;
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = new Vector3(Random.Range(-_cloudSize, _cloudSize), _cloudHeight, Random.Range(-_cloudSize, _cloudSize));
        obj.SetActive(true);
    }

    private void GetCube() => _pool.Get();

    private void ReturnCubeInCloud(GameObject gameObject) => _pool.Release(gameObject);
}