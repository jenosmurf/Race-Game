using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class carcontroller : MonoBehaviour
{
    [SerializeField] WheelCollider wheelFL;
    [SerializeField] WheelCollider wheelFR;
    [SerializeField] WheelCollider wheelRL;
    [SerializeField] WheelCollider wheelRR;

    [SerializeField] Transform wheelFLTransform;
    [SerializeField] Transform wheelFRTransform;
    [SerializeField] Transform wheelRLTransform;
    [SerializeField] Transform wheelRRTransform;

    [SerializeField] Vector3 frontLeftRotationOffset = new Vector3(0f, 90f, 0f);

    public AudioSource audioSource, audioSource2;

    public Transform centerOfMass;
    private Rigidbody rb;

    public float acceleration = 2500, brakingForce, maxTurnAngle = 30; 
    public float currentAcceleration = 0f, currentBrakeForce = 0f, currentTurnAngle = 0f;
    public float speed;
    public int currentWaypoint, lastWaypointHit, lapsCompleted;
    public TMP_Text speedTMP;

    public void SetInput(float accelerationInput, float steeringInput, float brakeInput, float handbrakeInput)
    {
        currentAcceleration = acceleration * Mathf.Clamp(accelerationInput, -1f, 1f);
        currentTurnAngle = Mathf.Clamp(steeringInput, -maxTurnAngle, maxTurnAngle);
        currentBrakeForce = brakingForce * Mathf.Clamp01(Mathf.Max(brakeInput, handbrakeInput));
    }

    private void FixedUpdate()
    {
        speed = rb.linearVelocity.magnitude * 3.6f;

        wheelRL.motorTorque = currentAcceleration;
        wheelRR.motorTorque = currentAcceleration;

        wheelFL.brakeTorque = currentBrakeForce;
        wheelFR.brakeTorque = currentBrakeForce;
        wheelRL.brakeTorque = currentBrakeForce;
        wheelRR.brakeTorque = currentBrakeForce;

        wheelFL.steerAngle = currentTurnAngle;
        wheelFR.steerAngle = currentTurnAngle;

        UpdateWheel(wheelFL, wheelFLTransform, frontLeftRotationOffset);
        UpdateWheel(wheelFR, wheelFRTransform);
        UpdateWheel(wheelRL, wheelRLTransform);
        UpdateWheel(wheelRR, wheelRRTransform);
    }

    void Update()
    {
        if (speedTMP == null)
        {
            return;
        }

        float speed = rb == null
            ? 0f
            :rb.linearVelocity.magnitude * 3.6f;

        speedTMP.text = Mathf.RoundToInt(speed) + " km/h";
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        audioSource.Play();
        audioSource2.Play();
    }

    void UpdateWheel(WheelCollider col, Transform trans, Vector3 rotationOffset = default)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);

        trans.position = pos;
        trans.rotation = rot * Quaternion.Euler(rotationOffset);
    }

}
