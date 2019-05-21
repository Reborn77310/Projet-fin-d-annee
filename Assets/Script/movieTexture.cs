using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movieTexture : MonoBehaviour
{
    MovieTexture movie;

    [FMODUnity.EventRef]
    public string select_sound;
    public FMOD.Studio.EventInstance soundevent;

    void Awake()
    {
        soundevent = FMODUnity.RuntimeManager.CreateInstance(select_sound);
        soundevent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
    }

    public void Activevideo()
    {
        GetComponent<RawImage>().enabled = true;
        movie = GetComponent<RawImage>().mainTexture as MovieTexture;

        movie.Play();
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        soundevent.getPlaybackState(out fmodPbState);

        soundevent.start();
        print(soundevent.getPlaybackState(out fmodPbState));


        StartCoroutine("Continue");
    }

    IEnumerator Continue()
    {
        yield return new WaitForSeconds(8);
        movie.Stop();
        soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        GameObject.Find("GameMaster").GetComponent<EnnemiManager>().SpawnAdversaire();
        this.gameObject.SetActive(false);
    }
}
