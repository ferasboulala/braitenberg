using System.Collections.Generic;
using UnityEngine;

public class SourcePool : MonoBehaviour
{
    [HideInInspector]
    public List<Source> sources;
    public static SourcePool instance;

    public void Register(Source source)
    {
        sources.Add(source);
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        sources = new List<Source>();
    }
}
