using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerReticle playerReticle;
    [SerializeField] private float lensWideningFactor;
    [SerializeField] private float trackingOffsetFactor;
    [SerializeField] private float lerpTime;

    private float originalLensSize;
    private Vector3 originalTrackingOffset;
    private CinemachineVirtualCamera cinemachine;
    private CinemachineFramingTransposer framingTransposer;

    private void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();

        originalLensSize = cinemachine.m_Lens.OrthographicSize;
        originalTrackingOffset = framingTransposer.m_TrackedObjectOffset;
    }

    private void Update()
    {
        ReticleCameraMovement();
    }

    /// <summary>
    /// Expand the camera's vision when the player tries looking further in the distance.
    /// </summary>
    private void ReticleCameraMovement()
    {
        Vector3 reticleVector = playerReticle.MouseOffsetFromPlayer;

        if (reticleVector.magnitude > 2f)
        {
            cinemachine.m_Lens.OrthographicSize = Mathf.Lerp(cinemachine.m_Lens.OrthographicSize, originalLensSize + reticleVector.magnitude * lensWideningFactor, lerpTime);

            Vector3 offsetVector = new Vector3(reticleVector.x * trackingOffsetFactor, reticleVector.y * trackingOffsetFactor, originalTrackingOffset.z);
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(framingTransposer.m_TrackedObjectOffset, originalTrackingOffset + offsetVector, lerpTime);

        }
        else
        {
            cinemachine.m_Lens.OrthographicSize = Mathf.Lerp(cinemachine.m_Lens.OrthographicSize, originalLensSize, lerpTime);
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(framingTransposer.m_TrackedObjectOffset, originalTrackingOffset, lerpTime);
        }
    }
}
