using System;
using UnityEngine;
using System.Collections;
public class CarControl : MonoBehaviour
{

    private const string horizontal = "Horizontal", vertical = "Vertical";

    [SerializeField] private WheelCollider FLCollider, FRCollider, RLCollider, RRCollider;
    [SerializeField] private Transform FLWheel, FRWheel, RLWheel, RRWheel;

    [SerializeField] private float motorTorque = 200, brakeTorque = 400, steerAngle = 30;
    [SerializeField] private float WheelBaseLength = 500, WheelDistance = 200;
    private float horizontalInput, verticalInput,
                  currentBreakForce, currentSteerAngle;
    private bool isBrake;

    private float turnRadius;
    
    void FixedUpdate ()
    {
        GetInput();
        HandleVelocity();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(horizontal);
        verticalInput = Input.GetAxis(vertical);
        isBrake = Input.GetKey(KeyCode.Space);
        //Debug.Log(horizontalInput);
        //Debug.Log(verticalInput);
        //Debug.Log(isBrake);
    }

    private void HandleVelocity()
    {
        RLCollider.motorTorque = verticalInput * motorTorque;
        RRCollider.motorTorque = verticalInput * motorTorque;
        currentBreakForce = isBrake ? brakeTorque : 0;
        HandleBreak();
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
        currentSteerAngle = steerAngle * horizontalInput;
        turnRadius = Mathf.Abs(WheelBaseLength * Mathf.Tan(Mathf.Deg2Rad*(90 - Mathf.Abs(currentSteerAngle))));
        float angle1 = currentSteerAngle > 0 ? Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius + WheelDistance / 2f))
                : (-1)*Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius - WheelDistance / 2f));
        float angle2 = currentSteerAngle > 0 ? Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius - WheelDistance / 2f))
                : (-1)*Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius + WheelDistance / 2f));
        Debug.Log(angle1);
        Debug.Log(angle2);
        FLCollider.steerAngle = angle1;
        FRCollider.steerAngle = angle2;
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