using System;
using UnityEngine;
using System.Collections;
public class CarDemo : MonoBehaviour
{

    private const string horizontal = "Horizontal", vertical = "Vertical";

    [SerializeField] private WheelCollider FLCollider, FRCollider, RLCollider, RRCollider;
    [SerializeField] private Transform FLWheel, FRWheel, RLWheel, RRWheel;
    [SerializeField] private float motorTorque = 200,
                                   brakeTorque = 400,
                                   steerAngle = 30,
                                   maxrpm = 1000,
                                   wheelRadius = 0.35f;
    [SerializeField] private float WheelBaseLength = 500, WheelDistance = 200;
    [SerializeField] private Transform com;
    private float horizontalInput, verticalInput = 0f,
                  currentBreakForce, currentSteerAngle;
    private bool isBrake;
    private float turnRadius;
    private float maxrpmPossible;

    void Start()
    {
        //Time.timeScale = 1.5f;
        maxrpmPossible = (float)(wheelRadius * 2 * Math.PI * maxrpm);
        gameObject.GetComponent<Rigidbody>().centerOfMass = com.localPosition;
    }
    
    void FixedUpdate ()
    {
        GetInput();
        HandleSteering();
        HandleVelocity();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = 1;
        verticalInput = 1;
        //isBrake = false;
        //horizontalInput = Input.GetAxis(horizontal);
        //verticalInput = Input.GetAxis(vertical);
        //isBrake = Input.GetKey(KeyCode.Space);
        //Debug.Log(horizontalInput);
        //Debug.Log(verticalInput);
        //Debug.Log(isBrake);
    }

    private void HandleVelocity()
    {
        float RL_cof = (float)Math.Pow(maxrpm, maxrpm - Math.Abs(RLCollider.rpm));
        float RR_cof = (float)Math.Pow(maxrpm, maxrpm - Math.Abs(RRCollider.rpm));

        if (RL_cof < 0)
            RL_cof = 0;
        if (RL_cof > 1)
            RL_cof = 1;
        if (RLCollider.rpm * verticalInput < 0)
            RL_cof = 1;

        if (RR_cof < 0)
            RR_cof = 0;
        if (RR_cof > 1)
            RR_cof = 1;
        if (RRCollider.rpm * verticalInput < 0)
            RR_cof = 1;
        
        if (Math.Abs(turnRadius) < 0.01)
        {
            RLCollider.motorTorque = RL_cof * verticalInput * motorTorque;
            RRCollider.motorTorque = RR_cof * verticalInput * motorTorque;
        }
        else
        {
            float radiusInside = turnRadius != 0 ? turnRadius - WheelDistance / 2f : 1;
            float radiusOutside = turnRadius != 0 ? turnRadius + WheelDistance / 2f : 1;
            float RLCof2 = horizontalInput > 0 ? radiusOutside / radiusInside : radiusInside / radiusOutside;
            float RRCof2 = horizontalInput > 0 ? radiusInside / radiusOutside : radiusOutside / radiusInside;
            RLCollider.motorTorque = RL_cof * verticalInput * motorTorque * RLCof2;
            RRCollider.motorTorque = RR_cof * verticalInput * motorTorque * RRCof2;
        }
        //Debug.Log(RLCollider.rpm + " " + RRCollider.rpm + " " + verticalInput); 
        currentBreakForce = isBrake ? brakeTorque : 0;
        //HandleBreak();
    }

    private void HandleBreak()
    {
        FLCollider.brakeTorque = currentBreakForce;
        FRCollider.brakeTorque = currentBreakForce;
        RLCollider.brakeTorque = currentBreakForce;
        RRCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        float steerCoef = 1 - (Math.Abs(FLCollider.rpm) / maxrpmPossible);
        //if (steerCoef > 1) steerCoef = 1;
        currentSteerAngle = steerAngle * horizontalInput * steerCoef;
        if (Math.Abs(currentSteerAngle) > 0.01)
        {
            turnRadius = Mathf.Abs(WheelBaseLength * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(currentSteerAngle))));
            float angle1 = currentSteerAngle > 0
                ? Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius + WheelDistance / 2f))
                : (-1) * Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius - WheelDistance / 2f));
            float angle2 = currentSteerAngle > 0
                ? Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius - WheelDistance / 2f))
                : (-1) * Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius + WheelDistance / 2f));
            FLCollider.steerAngle = angle1;
            FRCollider.steerAngle = angle2;
        }
        else
        {
            FLCollider.steerAngle = FRCollider.steerAngle = 0;
            turnRadius = 0;
        }
    }

    private void UpdateWheels()
    {
        UpdateWheelRotation(FLCollider, FLWheel);
        UpdateWheelRotation(FRCollider, FRWheel);
        UpdateWheelRotation(RLCollider, RLWheel);
        UpdateWheelRotation(RRCollider, RRWheel);
    }
    private void UpdateWheelRotation(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 angle;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out angle, out rotation);
        wheelTransform.rotation = rotation;
        wheelTransform.position = angle;
        
    }
    
}