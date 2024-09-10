using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Renderer),typeof(Collider))]

public class Cube : MonoBehaviour
{
    private Coroutine _releaseWaiter;
    private Renderer _renderer;
    private WaitForSecondsRealtime _wait;

    private float _timeBeforeRelease;
    private float _maxReleaseTime;
    private float _minReleaseTime;
    private bool _isDeactivated;

    public event Action<Cube> Fallen;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _maxReleaseTime = 5f;
        _minReleaseTime = 2f;
        _isDeactivated = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Platform>() != null && _isDeactivated == false)
        {
            _isDeactivated = true;

            _renderer.material.color = UnityEngine.Random.ColorHSV();

            _timeBeforeRelease = UnityEngine.Random.Range(_minReleaseTime, _maxReleaseTime);
            _wait = new WaitForSecondsRealtime(_timeBeforeRelease);

            RestartWaiting();
        }
    }

    private void OnEnable()
    {
        _isDeactivated = false;

        _renderer.material.color = Color.white;
    }

    private void OnDisable()
    {
        if (_releaseWaiter != null)
        {
            StopCoroutine(_releaseWaiter);
        }
    }

    private void RestartWaiting()
    {
        _releaseWaiter = StartCoroutine(CountUp());
    }

    private IEnumerator CountUp()
    {
        yield return _wait;

        Fallen?.Invoke(this);
    }
}