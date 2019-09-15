using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

[System.Serializable]
public class VehicleTemplate
{
    public GameObject prefab;
    public int n;
}

public class VehiclePool : MonoBehaviour
{
    public VehicleTemplate[] vehicleTemplates;
    private List<Vehicle> vehicles;
    public static VehiclePool instance;
    public float scale = 10f;
    public int nThreads = 12;

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

        Physics.gravity = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        vehicles = new List<Vehicle>();
        foreach (VehicleTemplate template in vehicleTemplates)
        {
            for (int i = 0; i < template.n; ++i)
            {
                float xPos = Random.Range(-scale / 2 * 0.95f, scale / 2 * 0.95f);
                float yPos = Random.Range(-scale / 2 * 0.95f, scale / 2 * 0.95f);

                GameObject obj = Instantiate(
                    template.prefab,
                    new Vector2(xPos, yPos),
                    Quaternion.identity
                );
                Vehicle vehicle = obj.GetComponent<Vehicle>();
                vehicles.Add(vehicle);
            }
        }
    }

    // TODO : Parallel computation
    // struct ComputeJob : IJobParallelFor
    // {
    //     public NativeArray<Vehicle> vehicles;

    //     public void Execute(int i)
    //     {
    //         vehicles[i].Compute();
    //     }
    // }

    // struct ExecuteJob : IJobParallelFor
    // {
    //     public List<Vehicle> vehicles;
    //     public float deltaTime;

    //     public void Execute(int i)
    //     {
    //         vehicles[i].Execute(deltaTime);
    //     }
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     var computeJob = new ComputeJob()
    //     {
    //         vehicles = this.vehicles
    //     };

    //     var executeJob = new ExecuteJob()
    //     {
    //         vehicles = this.vehicles,
    //         deltaTime = Time.deltaTime
    //     };

    //     JobHandle handle = computeJob.Schedule(vehicles.Count, nThreads);
    //     handle.Complete();

    //     handle = executeJob.Schedule(vehicles.Count, nThreads);
    //     handle.Complete();
    // }
}
