using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiantSounds : MonoBehaviour
{
    [FMODUnity.EventRef] public string Select_Sound;
    public FMOD.Studio.EventInstance SoundEvent;

    [FMODUnity.EventRef] public string Select_Sound2;
    public FMOD.Studio.EventInstance SoundEvent2;

    void Start()
    {
        SoundEvent = FMODUnity.RuntimeManager.CreateInstance(Select_Sound);
        SoundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
        PlayNormalAmbiant();

        SoundEvent2 = FMODUnity.RuntimeManager.CreateInstance(Select_Sound2);
        SoundEvent2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    }

    public void PlayNormalAmbiant()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent.getPlaybackState(out fmodPbState);
        SoundEvent.start();
    }

    public void PlayTempeteAmbiant()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent2.getPlaybackState(out fmodPbState);
        SoundEvent2.start();
    }

    public void StopAll()
    {
        SoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SoundEvent2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
