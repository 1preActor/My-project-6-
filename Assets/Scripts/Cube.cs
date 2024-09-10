using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Renderer), typeof(Collider), typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private Coroutine _releaseWaiter;
    private Renderer _renderer;

    private float _timeBeforeRelease;
    private float _maxReleaseTime;
    private float _minReleaseTime;
    private bool _isDeactivated;

    public event Action<Cube> Fallen;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _isDeactivated = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Platform>() != null && _isDeactivated == false)
        {
            _isDeactivated = true;
            _renderer.material.color = UnityEngine.Random.ColorHSV();
            _releaseWaiter = StartCoroutine(CountUp());
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
            StopCoroutine(_releaseWaiter);
    }

    private IEnumerator CountUp()
    {
        _minReleaseTime = 2f;
        _maxReleaseTime = 5f;

        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(_minReleaseTime, _maxReleaseTime));

        Fallen?.Invoke(this);
    }
}