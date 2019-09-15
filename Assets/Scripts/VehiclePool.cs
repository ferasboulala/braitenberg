using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Generalise with a PoolGenerator
public class VehiclePool : MonoBehaviour
{
    public Vehicle[] vehicles;
    public static VehiclePool instance;
    public float scale = 10f;
    public int nVehicles = 10;
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

        vehicles = new Vehicle[nVehicles];
        for (int i = 0; i < nVehicles; ++i)
        {
            float xPos = Random.Range(-scale / 2 * 0.95f, scale / 2 * 0.95f);
            float yPos = Random.Range(-scale / 2 * 0.95f, scale / 2 * 0.95f);
            GameObject obj = Instantiate(prefab, new Vector2(xPos, yPos), Quaternion.identity);
            Vehicle vehicle = obj.GetComponent<Vehicle>();
            vehicles[i] = vehicle;
        }
    }
}
