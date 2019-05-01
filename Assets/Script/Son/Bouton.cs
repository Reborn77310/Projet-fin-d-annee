using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouton : MonoBehaviour
{
    [FMODUnity.EventRef] public string Select_Sound;
    public FMOD.Studio.EventInstance SoundEvent;
    [FMODUnity.EventRef] public string Select_Sound2;
    public FMOD.Studio.EventInstance SoundEvent2;

    void Start()
    {
        SoundEvent = FMODUnity.RuntimeManager.CreateInstance(Select_Sound);
        SoundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));

        SoundEvent2 = FMODUnity.RuntimeManager.CreateInstance(Select_Sound2);
        SoundEvent2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    }

    public void ClicBouton()
    {
        SoundEvent.start();
    }

    public void CancelBouton()
    {
        SoundEvent2.start();
    }


}
