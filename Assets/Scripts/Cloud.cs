using UnityEngine;
using UnityEngine.Pool;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _poolCapacity = 40;
    [SerializeField] private int _poolMaxSize = 100;
    [SerializeField] private int _cloudHeight = 60;
    [SerializeField] private float _cloudSize = 20;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () =>
        {
            var result = Instantiate(_prefab);

            result.Fallen += ReturnCubeInCloud;

            return result;
        },
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => obj.gameObject.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        float rainStartTime = 0f;
        float cubeRepeatTime = 0.05f;

        InvokeRepeating(nameof(GetCube), rainStartTime, cubeRepeatTime);
    }

    private void ActionOnGet(Cube obj)
    {
        obj.transform.position = new Vector3(Random.Range(-_cloudSize, _cloudSize), _cloudHeight, Random.Range(-_cloudSize, _cloudSize));
        obj.gameObject.SetActive(true);
        obj.GetComponent<Renderer>().material.color = Color.white;
    }

    private void GetCube() => _pool.Get();

    private void ReturnCubeInCloud(Cube cube)
    {
        cube.Fallen -= ReturnCubeInCloud;
        _pool.Release(cube);
    }
}