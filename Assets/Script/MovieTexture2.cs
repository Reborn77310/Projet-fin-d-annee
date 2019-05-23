using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieTexture2 : MonoBehaviour
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

        StartCoroutine("Continue");
    }

    IEnumerator Continue()
    {
        yield return new WaitForSeconds(5);
        movie.Stop();
        soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        CartesManager.PhaseLente = false;
        GameObject.Find("GameMaster").GetComponent<EnnemiManager>().SpawnAdversaire();
        Camera.main.GetComponent<MusiqueScript>().LancerMusiqueCombat();
        var newPos = new Vector3(0, 58.4f, 0);

        Camera.main.transform.position = newPos;
        this.gameObject.SetActive(false);
    }
}
