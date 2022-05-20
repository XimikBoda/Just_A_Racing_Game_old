using System;
using UnityEngine;
using System.Collections;
public class CarControl : MonoBehaviour
{

    private const string horizontal = "Horizontal", vertical = "Vertical";

    [SerializeField] private WheelCollider FLCollider, FRCollider, RLCollider, RRCollider;
    [SerializeField] private Transform FLWheel, FRWheel, RLWheel, RRWheel;

    [SerializeField] private float motorTorque = 200, brakeTorque = 400, steerAngle = 30, maxrpm = 1000, InputP = 3;
    [SerializeField] private float WheelBaseLength = 500, WheelDistance = 200;
    private float horizontalInput, verticalInput,
                  currentBreakForce, currentSteerAngle;
    private bool isBrake;

    private float turnRadius;

    void FixedUpdate()
    {
        GetInput();
        HandleSteering();
        HandleVelocity();
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
        float RL_cof = Math.Abs(RLCollider.rpm)> maxrpm?0:1;
        float RR_cof = Math.Abs(RRCollider.rpm) > maxrpm ? 0 : 1;

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

        //Debug.Log(RL_cof + " | " + RR_cof);

        /* if (Math.Abs(turnRadius) < 0.01)
         {*/
            RLCollider.motorTorque = RL_cof * verticalInput * motorTorque;
            RRCollider.motorTorque = RR_cof * verticalInput * motorTorque;
       /* }
        else
        {
            RLCollider.motorTorque = RL_cof * verticalInput * motorTorque * (turnRadius + WheelDistance / 2f) / (turnRadius * 2f);
            RRCollider.motorTorque = RR_cof * verticalInput * motorTorque * (turnRadius - WheelDistance / 2f) / (turnRadius * 2f);
        }*/

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
        currentSteerAngle = (float)(steerAngle * Math.Pow(Math.Abs( horizontalInput),InputP) )  * (horizontalInput>0?1:-1);
        if (Math.Abs(currentSteerAngle) > 0.01)
        {
            turnRadius = Mathf.Abs(WheelBaseLength * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(currentSteerAngle))));
            float angle1 = currentSteerAngle > 0 ? Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius + WheelDistance / 2f))
                    : (-1) * Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius - WheelDistance / 2f));
            float angle2 = currentSteerAngle > 0 ? Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius - WheelDistance / 2f))
                    : (-1) * Mathf.Rad2Deg * Mathf.Atan(WheelBaseLength / (turnRadius + WheelDistance / 2f));
            // Debug.Log(angle1);
            //Debug.Log(angle2);
            FLCollider.steerAngle = angle1;
            FRCollider.steerAngle = angle2;
        }
        else
        {
            FLCollider.steerAngle = 0;
            FRCollider.steerAngle = 0;
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