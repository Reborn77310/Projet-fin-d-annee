using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    [FMODUnity.EventRef] public string Using_Sound;
    public FMOD.Studio.EventInstance SoundEvent;
    [FMODUnity.EventRef] public string In_Sound;
    public FMOD.Studio.EventInstance SoundEvent2;
    [FMODUnity.EventRef] public string Alteration_Sound;
    public FMOD.Studio.EventInstance SoundEvent3;
    [FMODUnity.EventRef] public string Offensif_Sound;
    public FMOD.Studio.EventInstance SoundEvent4;
    [FMODUnity.EventRef] public string Defensif_Sound;
    public FMOD.Studio.EventInstance SoundEvent5;

    void Start()
    {
        SoundEvent = FMODUnity.RuntimeManager.CreateInstance(Using_Sound);
        SoundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
        
        SoundEvent2 = FMODUnity.RuntimeManager.CreateInstance(In_Sound);
        SoundEvent2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    
        SoundEvent3 = FMODUnity.RuntimeManager.CreateInstance(Alteration_Sound);
        SoundEvent3.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));

        SoundEvent4 = FMODUnity.RuntimeManager.CreateInstance(Offensif_Sound);
        SoundEvent4.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));

        SoundEvent5 = FMODUnity.RuntimeManager.CreateInstance(Defensif_Sound);
        SoundEvent5.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));

    }

    // Update is called once per frame
    public void PlayUsingEvent()
    {
        SoundEvent.start();
    }
    public void PlayInEvent()
    {
        SoundEvent2.start();
    }
    public void PlaySetupType(string whatType)
    {
        if (whatType == "alteration")
        {
            SoundEvent3.start();
        }
        if (whatType == "offensif")
        {
            SoundEvent4.start();
        }
        if (whatType == "defensif")
        {
            SoundEvent5.start();
        }
    }
}
