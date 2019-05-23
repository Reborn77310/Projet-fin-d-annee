using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusiqueScript : MonoBehaviour
{
    [FMODUnity.EventRef] public string PhaseLente;
    public FMOD.Studio.EventInstance SoundEvent;
    [FMODUnity.EventRef] public string Select_Sound2;
    public FMOD.Studio.EventInstance SoundEvent2;
    [FMODUnity.EventRef] public string Select_Sound3;
    public FMOD.Studio.EventInstance SoundEvent3;

    void Start()
    {
        SoundEvent = FMODUnity.RuntimeManager.CreateInstance(PhaseLente);
        SoundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    }

    public void LancerPhaseLente()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent.getPlaybackState(out fmodPbState);
        SoundEvent.start();
    }
    public void StopPhaseLente()
    {
        SoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }  
}
