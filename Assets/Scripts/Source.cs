using UnityEngine;

public class Source : MonoBehaviour
{
    public enum Affinity {Heat, Pressure, Light, Sound, None};
    public GameObject particles;
    public Affinity affinity;
    public Vehicle holder = null;

    void Start()
    {
        SourcePool.instance.Register(this);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color color = Color.black;
        switch (affinity)
        {
            case Affinity.Heat:
            color = Color.red;
            break;
            case Affinity.Pressure:
            color = Color.magenta;
            break;
            case Affinity.Light:
            color = Color.yellow;
            break;
            case Affinity.Sound:
            color = Color.green;
            break;
            case Affinity.None:
            color = Color.gray;
            break;
        }

        sr.color = color;

        if (particles == null)
        {
            particles = Resources.Load<GameObject>("SourceParticle");
        }
        GameObject obj = Instantiate(particles, transform.position, Quaternion.identity);
        var main = obj.GetComponent<ParticleSystem>().main;
        main.startColor = color;
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
