using UnityEngine;

public class Source : MonoBehaviour
{
    public enum Affinity {Heat, Pressure, Light, Sound, None};
    public Affinity affinity;
    public Vehicle holder = null;

    void Start()
    {
        SourcePool.instance.Register(this);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch (affinity)
        {
            case Affinity.Heat:
            sr.color = Color.red;
            break;
            case Affinity.Pressure:
            sr.color = Color.magenta;
            break;
            case Affinity.Light:
            sr.color = Color.yellow;
            break;
            case Affinity.Sound:
            sr.color = Color.green;
            break;
            case Affinity.None:
            sr.color = Color.gray;
            break;
        }
    }
}
