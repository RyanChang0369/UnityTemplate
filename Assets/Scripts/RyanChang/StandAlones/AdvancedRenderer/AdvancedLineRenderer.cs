﻿using UnityEngine;

public class AdvancedLineRenderer : MonoBehaviour
{
    [Header("User settings")]
    [Tooltip("Transform that make up the renderer")]
    public Transform[] transforms;

    [Tooltip("If checked, then this advanced line renderer relies upon a ALR creator.")]
    public bool usedByCreator;

    [Header("Autogenerated values")]
    [Tooltip("Line renderer")]
    public LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private Vector3[] positions;

    private void Update()
    {
        positions = new Vector3[transforms.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transforms[i].position;
        }

        if (lineRenderer.positionCount != transforms.Length)
        {
            lineRenderer.positionCount = transforms.Length;
        }

        lineRenderer.SetPositions(positions);
    }
}