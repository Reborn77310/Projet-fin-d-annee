using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculeFB : MonoBehaviour
{

    public static int vehiculeState = 0;
    public GameObject[] particuleFenetreGO;
    public CameraShake cameraShake;
    public GameObject[] tempestGO;
    public GameObject[] particuleMoteur;

	AmbiantSounds ambiantSounds;
    public bool etatTempestOn = false;

	void Awake()
	{
		ambiantSounds = GetComponent<AmbiantSounds>();
	}

    private void Start() {
        EtatDeplacement();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            EtatStop();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            EtatDeplacement();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            EtatSurregime();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            EtatTempete();
        }
		if (Input.GetKeyDown(KeyCode.M))
        {
            StopTempete();
        }
    }

    // ETAT SURREGIME
    // • Play son surrégime


    public void EtatStop()
    {
        ambiantSounds.StopAll();
        for (int i = 0; i < particuleFenetreGO.Length; i++)
        {
            particuleFenetreGO[i].GetComponent<ParticleSystem>().Pause();
        }
        foreach (GameObject go in particuleMoteur)
        {
            go.SetActive(false);
        }
        cameraShake.Shake(0, 0, 0);
        particuleMoteur[0].transform.parent.GetComponent<Animator>().SetBool("playanim",false);
        etatTempestOn = false;
        
    }

    public void EtatDeplacement()
    {
        ambiantSounds.StopAll();
        ambiantSounds.PlayNormalAmbiant();
        for (int i = 0; i < particuleFenetreGO.Length; i++)
        {
            particuleFenetreGO[i].GetComponent<ParticleSystem>().Stop();

            var main = particuleFenetreGO[i].GetComponent<ParticleSystem>().main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(2.5f, 5);

            particuleFenetreGO[i].GetComponent<ParticleSystem>().Play();
        }
        cameraShake.Shake(0.05f, 99999, 1);
        foreach (GameObject go in particuleMoteur)
        {
            go.SetActive(false);
        }
        particuleMoteur[0].transform.parent.GetComponent<Animator>().SetBool("playanim",false);
        etatTempestOn = false;
    }

    public void EtatSurregime()
    {
        for (int i = 0; i < particuleFenetreGO.Length; i++)
        {
            particuleFenetreGO[i].GetComponent<ParticleSystem>().Stop();

            var main = particuleFenetreGO[i].GetComponent<ParticleSystem>().main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(5, 10);

            particuleFenetreGO[i].GetComponent<ParticleSystem>().Play();
        }

        foreach (GameObject go in particuleMoteur)
        {
            go.SetActive(true);
        }
        particuleMoteur[0].transform.parent.GetComponent<Animator>().SetBool("playanim",true);
        cameraShake.Shake(0.15f, 9999, 1);
        etatTempestOn = false;
    }

    public void EtatTempete()
    {
        ambiantSounds.StopAll();
        ambiantSounds.PlayTempeteAmbiant();
        for (int i = 0; i < tempestGO.Length; i++)
        {
            tempestGO[i].SetActive(true);
            particuleFenetreGO[i].GetComponent<ParticleSystem>().Stop();
        }
        foreach (GameObject go in particuleMoteur)
        {
            go.SetActive(false);
        }
        etatTempestOn = true;
        StartCoroutine("RandomTempest", GiveMeARandom());
        StartCoroutine("RandomScreenShake", GiveMeARandom());
    }

    public void StopTempete()
    {
        ambiantSounds.StopAll();
        for (int i = 0; i < tempestGO.Length; i++)
        {
            tempestGO[i].SetActive(false);
        }
        etatTempestOn = false;
    }

    public void LaunchLighnting()
    {
        for (int i = 0; i < tempestGO.Length; i++)
        {
            tempestGO[i].GetComponentInChildren<Animator>().SetTrigger("go");
        }
        GameObject.Find("Thunder").GetComponent<SonEclair>().Eclair();
    }

    IEnumerator RandomTempest(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(etatTempestOn)
        {
            LaunchLighnting();
            StartCoroutine("RandomTempest", GiveMeARandom());
            
        }
        yield break;
    }

    float GiveMeARandom()
    {
        float rnd = Random.value * 3 + 5 ;
        return rnd;
    }

    IEnumerator RandomScreenShake(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(etatTempestOn)
        {
            //GameObject.Find("IMPACT").GetComponent<impPACT>().impacct();
            StartCoroutine("RandomScreenShake", GiveMeARandom());
            cameraShake.ScreenShake();
        }
        yield break;
    }
}
