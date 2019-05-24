using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    [FMODUnity.EventRef] public string VictorySound;
    public FMOD.Studio.EventInstance SoundEvent;

    [FMODUnity.EventRef] public string CliffHanger;
    public FMOD.Studio.EventInstance SoundEvent2;
    void Start()
    {
        SoundEvent = FMODUnity.RuntimeManager.CreateInstance(VictorySound);
        SoundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));

        SoundEvent2 = FMODUnity.RuntimeManager.CreateInstance(CliffHanger);
        SoundEvent2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    }

    public void LancerVictoire()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent.getPlaybackState(out fmodPbState);
        SoundEvent.start();
    }

    public void LancerCliff()
    {
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        SoundEvent2.getPlaybackState(out fmodPbState);
        SoundEvent2.start();
    }


}
