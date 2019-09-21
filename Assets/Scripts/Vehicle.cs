using UnityEngine;
using UnityEditor;

public class Vehicle : MonoBehaviour
{
    public GameObject[] sensors;
    public Source.Affinity sourceAffinity;
    private Source source;

    public enum Behavior { FEAR, AGGRESSION, LOVE, EXPLORATION, RANDOM, CUSTOM };
    public Behavior behavior = Behavior.CUSTOM;
    public Source.Affinity affinity;

    private float radius;
    public float maxTranslationVelocity = 1f;
    public float maxAngularVelocity = 1f;

    private float angularRotation;
    private float translationVelocity;

    private void randomStartPosition()
    {
        radius = transform.localScale.x / 2f;
        transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
    }

    private void selectBehavior()
    {
        if (behavior == Behavior.CUSTOM)
        {
            return;
        }

        if (behavior == Behavior.RANDOM)
        {
            behavior = (Behavior)Random.Range(0f, System.Enum.GetValues(typeof(Behavior)).Length - 2);
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sensors = new GameObject[1];
        sensors[0] = new GameObject();
        Sensor sensor = sensors[0].AddComponent<Sensor>();
        switch (behavior)
        {
            case Behavior.FEAR:
                sensor.Setup(affinity, -1f, false);
                sr.color = Color.yellow;
                break;

            case Behavior.AGGRESSION:
                sensor.Setup(affinity, -1f, true);
                sr.color = Color.red;
                break;

            case Behavior.LOVE:
                sensor.Setup(affinity, 1f, false);
                sr.color = new Color(1f, 0.5f, 0.66f, 1f); // pink
                break;

            case Behavior.EXPLORATION:
                sensor.Setup(affinity, 1f, true);
                sr.color = Color.green;
                break;
        }
        sensor.Randomize();
    }

    // Start is called before the first frame update
    void Start()
    {
        hideFlags = HideFlags.HideInHierarchy;
        randomStartPosition();
        selectBehavior();

        foreach (GameObject obj in sensors)
        {
            obj.GetComponent<Sensor>().holder = this;
        }

        if (sourceAffinity != Source.Affinity.None)
        {
            source = gameObject.AddComponent<Source>();
            source.affinity = sourceAffinity;
            source.holder = this;
        }
    }

    void Update()
    {
        Compute();
        Execute(Time.deltaTime);
    }

    public void Compute()
    {
        Vector2 normal = Vector2.Perpendicular(transform.up).normalized;
        Vector2 leftSensorsPosition = (Vector2)transform.position + normal * radius;
        Vector2 rightSensorsPosition = (Vector2)transform.position - normal * radius;

        float leftVelocity = 0f;
        float rightVelocity = 0f;

        foreach (GameObject obj in sensors)
        {
            Sensor sensor = obj.GetComponent<Sensor>();
            float left = sensor.Sense(leftSensorsPosition);
            float right = sensor.Sense(rightSensorsPosition);
            if (sensor.crossed)
            {
                leftVelocity += right;
                rightVelocity += left;
            }
            else
            {
                leftVelocity += left;
                rightVelocity += right;
            }
        }

        leftVelocity /= sensors.Length;
        rightVelocity /= sensors.Length;

        translationVelocity = maxTranslationVelocity * (leftVelocity + rightVelocity) / 2f;
        angularRotation = maxAngularVelocity * (rightVelocity - leftVelocity) / radius / 2f;
        angularRotation *= Mathf.Rad2Deg;
    }

    public void Execute(float deltaTime)
    {
        transform.position += transform.up * deltaTime * translationVelocity;
        transform.Rotate(new Vector3(0f, 0f, angularRotation * deltaTime));
    }
}

[CustomEditor(typeof(Vehicle))]
public class VehicleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Vehicle vehicle = target as Vehicle;
        vehicle.behavior = (Vehicle.Behavior)EditorGUILayout.EnumPopup("Behavior:", vehicle.behavior);
        vehicle.sourceAffinity = (Source.Affinity)EditorGUILayout.EnumPopup("Source Affinity:", vehicle.sourceAffinity);
        vehicle.maxTranslationVelocity = EditorGUILayout.FloatField("Maximum Translation Velocity:", vehicle.maxTranslationVelocity);
        vehicle.maxAngularVelocity = EditorGUILayout.FloatField("Maximum Angular Velocity:", vehicle.maxAngularVelocity);

        if (vehicle.behavior == Vehicle.Behavior.CUSTOM)
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sensors"), true);
            serializedObject.ApplyModifiedProperties();
        }
        else if (vehicle.behavior != Vehicle.Behavior.RANDOM)
        {
            vehicle.affinity = (Source.Affinity)EditorGUILayout.EnumPopup("Affinity:", vehicle.affinity);
        }
    }
}
