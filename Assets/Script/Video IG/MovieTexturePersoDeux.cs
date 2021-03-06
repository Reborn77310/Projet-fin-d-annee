﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieTexturePersoDeux : MonoBehaviour
{
    public GameObject prefabBoss;
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
        CartesManager.PhaseLente = false;
        GetComponent<RawImage>().enabled = true;
        movie = GetComponent<RawImage>().mainTexture as MovieTexture;

        movie.Play();
        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        soundevent.getPlaybackState(out fmodPbState);
        if (fmodPbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            soundevent.start();
        }
    }

    public void Continue()
    {
        movie.Stop();
        soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
        GameObject.Find("GameMaster").GetComponent<GameMaster>().TransmissionCadre.SetActive(false);
        GameObject.Find("GameMaster").GetComponent<EnnemiManager>().prefabADV = prefabBoss;
        GameObject.Find("GameMaster").GetComponent<EnnemiManager>().PassageEnPhaseCombat();

        GameObject.Find("Video2").GetComponent<MovieTexture2>().Activevideo();
        GameObject.Find("GameMaster").GetComponent<GameMaster>().SecondCam.GetComponent<Animator>().SetTrigger("stop");
    }
}
