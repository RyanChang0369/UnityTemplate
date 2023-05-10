﻿using System.Collections;
using UnityEngine;

public abstract class AdvancedLineRendererCreator : MonoBehaviour
{
    [Header("Autogenerated values")]
    [Tooltip("The ALR.")]
    public AdvancedLineRenderer alr;

    protected virtual void Start()
    {
        gameObject.RequireComponentAuto(out alr, "Advanced line renderer.");
    }
}