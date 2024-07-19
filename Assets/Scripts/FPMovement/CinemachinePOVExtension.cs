using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;

    private InputManager inputManager;
    private Vector3 startingRotation;

    protected override void Awake()
    {
        inputManager = InputManager.instance;
        startingRotation = transform.localRotation.eulerAngles;
        base.Awake();
    } 

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime )
    {
        if(inputManager == null)
        {
            return;
        }
        if(vcam != null && vcam.Follow)
        {
            if(stage != null && stage == CinemachineCore.Stage.Aim)
            {
                // if(!GameManager.instance.isPaused)
                // {
                    Vector2 delta = inputManager.GetMouseDelta();
                    startingRotation.x += delta.x * Time.deltaTime * verticalSpeed;
                    startingRotation.y -= delta.y * Time.deltaTime * horizontalSpeed;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                    state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0f);
                // }
                // else
                // {
                //     state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0f);
                // }
            }
        }
    }
}
