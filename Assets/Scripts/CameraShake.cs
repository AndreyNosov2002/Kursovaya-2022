using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform canTransform;

    private float shakeDur = 1f, shakeAmount = 0.1f, decreaseFactor = 1.5f;
    private Vector3 originPos;

    private void Start()
    {
        canTransform = GetComponent<Transform>();
        originPos = canTransform.localPosition;

    }

    private void Update()
    {
        if (shakeDur > 0)
        {
            canTransform.localPosition = originPos + Random.insideUnitSphere * shakeAmount;
            shakeDur -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shakeDur = 0;
            canTransform.localPosition = originPos;
        }
    }
}
