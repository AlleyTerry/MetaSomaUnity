using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VCamCorrection : CinemachineExtension
{
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam, 
        CinemachineCore.Stage stage, 
        ref CameraState state, 
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Finalize)
        {
            state.RawPosition = state.CorrectedPosition;
            state.PositionCorrection = Vector3.zero;
            state.RawOrientation = state.CorrectedOrientation;
            state.OrientationCorrection = Quaternion.identity;
        }
    }
}
