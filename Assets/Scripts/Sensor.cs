using UnityEngine;
using System;

public class Sensor : MonoBehaviour
{
    public Source.Affinity affinity;
    public bool random = false;
    public float slope;
    public bool crossed;
    public static float minStimulus = 0.05f;
    public static float scale = 5f;
    public Vehicle holder = null;

    void Start()
    {
        hideFlags = HideFlags.HideInHierarchy;
        if (random)
        {
            Randomize();
        }
    }

    public void Randomize()
    {
        affinity = (Source.Affinity)UnityEngine.Random.Range(0f, System.Enum.GetValues(typeof(Source.Affinity)).Length);

        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
        {
            slope = 1f;
        }
        else
        {
            slope = -1f;
        }
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
            float normedStimulus = stimulusNorm(distance);
            stimulus += normedStimulus;
            float alpha = normedStimulus;
            if (slope > 0)
            {
                alpha = 1 - alpha;
            }

            Color color;
            switch (affinity)
            {
                case Source.Affinity.Heat:
                color = Color.red;
                break;
                case Source.Affinity.Light:
                color = Color.yellow;
                break;
                case Source.Affinity.Pressure:
                color = Color.magenta;
                break;
                case Source.Affinity.Sound:
                color = Color.green;
                break;
                default:
                color = Color.black;
                break;
            }

            color.a = alpha;
            Debug.DrawLine((Vector3)pos, source.transform.position, color);
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