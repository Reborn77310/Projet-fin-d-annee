using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoMenu : MonoBehaviour
{
    public GameObject secondVid;
    public GameObject text;
    MovieTexture movie;

    [FMODUnity.EventRef]
    public string select_sound;
    public FMOD.Studio.EventInstance soundevent;

    void Awake()
    {
        soundevent = FMODUnity.RuntimeManager.CreateInstance(select_sound);
        soundevent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));

        Activevideo();
    }

    public void Activevideo()
    {
        GetComponent<RawImage>().enabled = true;
        movie = GetComponent<RawImage>().mainTexture as MovieTexture;

        movie.Play();
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        soundevent.getPlaybackState(out fmodPbState);

        soundevent.start();


        StartCoroutine("Continue");
    }

    IEnumerator Continue()
    {
        yield return new WaitForSeconds(movie.duration);
        movie.Stop();
        secondVid.SetActive(true);
        text.SetActive(true);
    }
}
