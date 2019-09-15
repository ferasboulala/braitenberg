using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    private Sensor leftSensor;
    private Sensor rightSensor;
    private bool crossed;
    public enum Behavior {FEAR, AGGRESSION, LOVE, EXPLORATION};
    public Behavior behavior;
    private float radius;
    public float maxTranslationVelocity = 1f;
    public float maxAngularVelocity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 2f;
        transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
        behavior = (Behavior)Random.Range(0f, System.Enum.GetValues(typeof(Behavior)).Length);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch (behavior)
        {
            case Behavior.FEAR:
            leftSensor = new Sensor(Source.Type.Heat, -1f);
            rightSensor = new Sensor(Source.Type.Heat, -1f);
            crossed = false;
            sr.color = Color.yellow;
            break;

            case Behavior.AGGRESSION:
            leftSensor = new Sensor(Source.Type.Heat, -1f);
            rightSensor = new Sensor(Source.Type.Heat, -1f);
            crossed = true;
            sr.color = Color.red;
            break;

            case Behavior.LOVE:
            leftSensor = new Sensor(Source.Type.Heat, 1f);
            rightSensor = new Sensor(Source.Type.Heat, 1f);
            crossed = false;
            sr.color = new Color(1f, 0.5f, 0.66f, 1f); // pink
            break;

            case Behavior.EXPLORATION:
            leftSensor = new Sensor(Source.Type.Heat, 1f);
            rightSensor = new Sensor(Source.Type.Heat, 1f);
            sr.color = Color.green;
            crossed = true;
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 normal = Vector2.Perpendicular(transform.up).normalized;
        Vector2 leftSensorPosition = (Vector2)transform.position + normal * radius;
        Vector2 rightSensorPosition = (Vector2)transform.position - normal * radius;

        float leftVelocity = leftSensor.Sense(leftSensorPosition);
        float rightVelocity = rightSensor.Sense(rightSensorPosition);

        float translationVelocity = maxTranslationVelocity * (leftVelocity + rightVelocity) / 2f;
        float angularRotation = maxAngularVelocity * (rightVelocity - leftVelocity) / radius / 2f;
        if (crossed) {
            angularRotation *= -1f;
        }
        angularRotation = Mathf.Rad2Deg * angularRotation;

        transform.position += transform.up * Time.deltaTime * translationVelocity;
        transform.Rotate(new Vector3(0f, 0f, angularRotation * Time.deltaTime));
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Vehicle>())
        {
            return;
        }

        transform.Rotate(new Vector3(0f, 0f, 180));
    }
}
