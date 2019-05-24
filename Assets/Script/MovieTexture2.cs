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
        yield return new WaitForSeconds(9);
        movie.Stop();
        soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        CartesManager.PhaseLente = false;
        for (int i = 0; i < 4; i++)
        {
            GameObject.Find("PattesController").GetComponent<PlayWithDécalage>().AllLegs[i].speed = 0;
        }
        var emission = GameObject.Find("NeigeTemporaire").GetComponent<ParticleSystem>().emission;
        emission.enabled = false;
        Camera.main.GetComponent<MusiqueScript>().StopPhaseLente();
        Camera.main.GetComponent<MusiqueScript>().LancerMusiqueCombat();
        Camera.main.GetComponent<Animator>().SetTrigger("GoDown");
        GameObject.Find("GameMaster").GetComponent<EnnemiManager>().SpawnAdversaire();
        this.gameObject.SetActive(false);
    }
}
