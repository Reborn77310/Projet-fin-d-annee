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

        StartCoroutine("Continue");
    }

    IEnumerator Continue()
    {
        var gameMaster = GameObject.Find("GameMaster");
        yield return new WaitForSeconds(5);
        movie.Stop();
        soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        CartesManager.PhaseLente = false;
        Camera.main.GetComponent<MusiqueScript>().LancerMusiqueCombat();
        gameMaster.GetComponent<EnnemiManager>().SpawnAdversaire();

        Camera.main.GetComponent<Animator>().SetTrigger("GoDown");
        int count = gameMaster.GetComponent<Equipement>().allEquipements.Count;
        for (int i = 0; i < count; i++)
        {
            gameMaster.GetComponent<Equipement>().allEquipements[i].go.GetComponent<Animator>().SetTrigger("Sortirlestourelles");
        }
        this.gameObject.SetActive(false);
    }
}
