using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sensor
{
    private Source.Type type;
    private float slope;
    private static float minStimulus = 0.1f;

    public Sensor(Source.Type type, float slope)
    {
        this.type = type;
        this.slope = slope;
    }

    public float Sense(Vector2 pos)
    {
        float stimulus = 0f;
        foreach (Source source in SourcePool.instance.sources)
        {
            if (source.type != this.type)
            {
                continue;
            } 
            float distance = Vector2.Distance(pos, (Vector2)source.transform.position);
            stimulus += StimulusNorm(distance);
        }

        return Math.Min(1f, Math.Max(minStimulus, stimulus));
    }

    private float StimulusNorm(float distance)
    {
        float result = slope * distance / SourcePool.instance.scale;
        if (slope < 0)
        {
            result += 1f;
        }

        return Math.Max(minStimulus, result);
    }
}