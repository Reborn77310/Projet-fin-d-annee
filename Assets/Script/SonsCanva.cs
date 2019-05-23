using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsCanva : MonoBehaviour
{
    [FMODUnity.EventRef] public string Almanach;
    public FMOD.Studio.EventInstance SoundEvent;
    [FMODUnity.EventRef] public string Fermeture;
    public FMOD.Studio.EventInstance SoundEvent2;
    [FMODUnity.EventRef] public string GoEquipement;
    public FMOD.Studio.EventInstance SoundEvent3;

    [FMODUnity.EventRef] public string Interaction;
    public FMOD.Studio.EventInstance SoundEvent4;

    void Start()
    {
        SoundEvent = FMODUnity.RuntimeManager.CreateInstance(Almanach);
        SoundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
        SoundEvent2 = FMODUnity.RuntimeManager.CreateInstance(Fermeture);
        SoundEvent2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
        SoundEvent3 = FMODUnity.RuntimeManager.CreateInstance(GoEquipement);
        SoundEvent3.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
        SoundEvent4 = FMODUnity.RuntimeManager.CreateInstance(Interaction);
        SoundEvent4.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    }

    public void LancerAlmanach()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent.getPlaybackState(out fmodPbState);
        SoundEvent.start();
    }

    public void LancerFermeture()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent2.getPlaybackState(out fmodPbState);
        SoundEvent2.start();
    }

    public void LancerEquipement()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent3.getPlaybackState(out fmodPbState);
        SoundEvent3.start();
    }
    public void InteractionLaunch()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent4.getPlaybackState(out fmodPbState);
        SoundEvent4.start();
    }

}
