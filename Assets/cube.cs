using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    public static event Action<GameObject> CubeFalled;

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
        Color cubeStartColor = new Color(1, 1, 1);

        StartCoroutine(CountUp(delay, cubeStartColor));
    }

    private IEnumerator CountUp(float delay, Color cubeStartColor)
    {
        var wait = new WaitForSeconds(delay);

        yield return wait;

        GetComponent<Renderer>().material.color = cubeStartColor;
        CubeFalled.Invoke(gameObject);
    }
}