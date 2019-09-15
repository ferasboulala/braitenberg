using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourcePool : MonoBehaviour
{
    public Source[] sources;
    public static SourcePool instance;
    public float scale = 10f;
    public int nSources = 10;
    public GameObject prefab;
    public bool random = false;
    public int seed = 42;

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
    }

    void Start()
    {
        if (random)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
        }
        else
        {
            Random.InitState(seed);
        }

        sources = new Source[nSources];
        for (int i = 0; i < nSources; ++i)
        {
            float xPos = Random.Range(-scale / 2 * 0.95f, scale / 2 * 0.95f);
            float yPos = Random.Range(-scale / 2 * 0.95f, scale / 2 * 0.95f);
            GameObject obj = Instantiate(prefab, new Vector2(xPos, yPos), Quaternion.identity);
            Source source = obj.GetComponent<Source>();
            sources[i] = source;
        }
    }
}
