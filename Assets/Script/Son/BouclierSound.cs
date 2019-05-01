using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouclierSound : MonoBehaviour
{

    [FMODUnity.EventRef] public string Select_Sound;
    public FMOD.Studio.EventInstance SoundEvent;
    void Start()
    {
        SoundEvent = FMODUnity.RuntimeManager.CreateInstance(Select_Sound);
        SoundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    }

   public void PlayBouclierSound()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent.getPlaybackState(out fmodPbState);
        SoundEvent.start();
    }

}
