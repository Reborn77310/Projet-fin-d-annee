using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatteShakeAndSound : MonoBehaviour
{
    
    [FMODUnity.EventRef] public string Select_Sound;
    public FMOD.Studio.EventInstance SoundEvent;
    CameraShake MainCamera;
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        SoundEvent = FMODUnity.RuntimeManager.CreateInstance(Select_Sound);
        SoundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    }
    public void PlaySound()
    {
        SoundEvent.start();
    }
    public void Shake()
    {
        MainCamera.Shake(0.2f,0.2f,0.5f);
    }
}
