using UnityEngine;
using System;

public class Sensor : MonoBehaviour
{
    public Source.Affinity affinity;
    public float slope;
    public bool crossed;
    public static float minStimulus = 0f;
    public static float scale = 5f;
    public Vehicle holder = null;

    void Start()
    {
        hideFlags = HideFlags.HideInHierarchy;
    }

    public void Setup(Source.Affinity affinity, float slope, bool crossed)
    {
        this.affinity = affinity;
        this.slope = slope;
        this.crossed = crossed;
    }

    public float Sense(Vector2 pos)
    {
        float stimulus = 0f;
        foreach (Source source in SourcePool.instance.sources)
        {
            if (source.affinity != this.affinity || holder == source.holder)
            {
                continue;
            } 
            float distance = Vector2.Distance(pos, (Vector2)source.transform.position);
            stimulus += stimulusNorm(distance);
        }

        return Math.Min(1f, Math.Max(minStimulus, stimulus));
    }

    private float stimulusNorm(float distance)
    {
        float result = slope * distance / scale;
        if (slope < 0)
        {
            result += 1f;
        }

        return Math.Max(minStimulus, result);
    }
}