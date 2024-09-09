using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    public event Action<Cube> Fallen;

    //private void Awake()
    //{
    //    GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
    //}

    private void OnTriggerEnter(Collider other)
    {
        Init();
    }

    private void Init()
    {
        GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();

        float lifetimeMinCube = 2f;
        float lifetimeMaxCube = 5f;
        float delay = UnityEngine.Random.Range(lifetimeMinCube, lifetimeMaxCube);

        StartCoroutine(CountUp(delay));
    }

    private IEnumerator CountUp(float delay)
    {
        var wait = new WaitForSeconds(delay);

        yield return wait;

        Fallen?.Invoke(this);
    }
}