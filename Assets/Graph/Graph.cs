﻿using System;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    private Transform pointPrefab;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    [SerializeField]
    FunctionLibrary.FunctionName function = default;

    private Transform[] points;

    public int Resolution
    {
        get => resolution;
        set => Mathf.Clamp(resolution, 10, 100);
    }

    void Awake()
    {
        float step = 2f / resolution;
        Vector3 position = Vector3.zero;
        points = new Transform[resolution * resolution];
        Vector3 scale = Vector3.one * step;

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    private void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if(x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.25f) * step - 1f;
            }

            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, time);
        }
    }
}
