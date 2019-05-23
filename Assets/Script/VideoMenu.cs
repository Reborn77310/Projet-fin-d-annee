using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            movie.Pause();
            soundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            StartCoroutine("changescene");

        }
    }

    public TextMeshPro[] FirstMat;
    public SpriteRenderer secondMat;
    public Image fondnoir;

    IEnumerator changescene()
    {
        GameObject.Find("Texts").GetComponent<Animator>().enabled = false;
        

        while (fondnoir.color.a < 1)
        {
            print(fondnoir.color.a);
            Color col = fondnoir.color;
            col.a += Time.deltaTime;
            fondnoir.color = col;

            for (int i = 0; i < FirstMat.Length; i++)
            {
                col = FirstMat[i].color;
                col.a -= Time.deltaTime;
                FirstMat[i].color = col;
            }


            col = secondMat.color;
            col.a -= Time.deltaTime;
            secondMat.color = col;
            yield return new WaitForSeconds(Time.deltaTime  * 2);
        }
        SceneManager.LoadScene("TEST 1 Double NavMesh", LoadSceneMode.Single);
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
        yield return new WaitForSeconds(movie.duration - 1);
        text.SetActive(true);
        yield return new WaitForSeconds(1);
        movie.Stop();
        secondVid.SetActive(true);
    }
}
