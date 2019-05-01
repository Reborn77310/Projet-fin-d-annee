using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camTransform;
    public float shakeDuration = 0f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }
    public void Shake(float newShakeAmount,float newShakeDuration,float newDecreaseFactor)
    {
        shakeAmount = newShakeAmount;
        shakeDuration = newShakeDuration;
        decreaseFactor = newDecreaseFactor;
    }

    struct ToSave
    {
        public float _shakeAmount;
        public float _shakeDuration;
        public float _decreaseFactor;
    }

    public void ScreenShake()
    {
        ToSave b = new ToSave();
        b._shakeAmount = shakeAmount;
        b._shakeDuration = shakeDuration;
        b._decreaseFactor = decreaseFactor;
        shakeAmount = 1;
        shakeDuration = 0.5f;
        decreaseFactor = 1;
        StartCoroutine("alkfha", b);
    }

    IEnumerator alkfha(ToSave _a)
    {
        yield return new WaitForSeconds(0.6f);
        Shake(_a._shakeAmount, _a._shakeDuration, _a._decreaseFactor);
        yield break;
    }


}