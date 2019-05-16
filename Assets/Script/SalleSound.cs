using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalleSound : MonoBehaviour
{
    [FMODUnity.EventRef] public string Select_Sound;
    public FMOD.Studio.EventInstance SoundEvent;
    [FMODUnity.EventRef] public string Select_Sound2;
	public FMOD.Studio.EventInstance SoundEvent2;
	
    void Start ()
    {
        SoundEvent = FMODUnity.RuntimeManager.CreateInstance(Select_Sound);
        SoundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
		
        SoundEvent2 = FMODUnity.RuntimeManager.CreateInstance(Select_Sound2);
        SoundEvent2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    }

    public void ReparerLaSalle()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent.getPlaybackState(out fmodPbState);
        SoundEvent.start();		
        print("rep");
    }

    public void DetruireLaSalle()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent2.getPlaybackState(out fmodPbState);
        SoundEvent2.start();
        print("des");
    }
}
