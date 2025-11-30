using System.Collections;
using System.Collections.Generic;
using ARDrums.Effects;
using UnityEngine;

public class MultiHighwayLight : MonoBehaviour
{
    [SerializeField] private RapidCurveEffect[] _lights;

    public void Initialize()
    {
        foreach (RapidCurveEffect light in _lights)
        {
            light.Initialize();
        }
    }

    public void Disable()
    {
        foreach (var light in _lights)
        {
            light.Disable();
        }
    }
    
    public void Curve(float value = 1.0f)
    {
        foreach (var light in _lights)
        {
            light.Curve(value);
        }
    }
    
    public void Launch()
    {
        foreach (var light in _lights)
        {
            light.Launch();
        }
    }

    public void Update()
    {
        // space key to test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
            Curve(1.0f);       
        }
    }
}
