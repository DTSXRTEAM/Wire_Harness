using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class DynamicSplineUpdater : MonoBehaviour
{
    public SplineContainer splineContainer;
    public List<Transform> spheres = new List<Transform>(); // List of sphere instances

    void Update()
    {
        UpdateSpline(); // Continuously update spline
    }

    void UpdateSpline()
    {
        if (splineContainer == null || spheres.Count == 0) return;

        Spline spline = splineContainer.Spline;
        spline.Clear();

        foreach (Transform sphere in spheres)
        {
            spline.Add(new BezierKnot(sphere.position)); // Add sphere positions to spline
        }

        ApplySmoothTangents(spline);


    }

    void ApplySmoothTangents(Spline spline)
    {
        for (int i = 0; i < spline.Count; i++)
        {
            spline.SetTangentMode(i, TangentMode.AutoSmooth);
        }
    }

    public void AddSphere(Transform sphere)
    {
        if (!spheres.Contains(sphere))
        {
            spheres.Add(sphere);
        }
    }

    public void RemoveSphere(Transform sphere)
    {
        if (spheres.Contains(sphere))
        {
            spheres.Remove(sphere);
        }
    }
}