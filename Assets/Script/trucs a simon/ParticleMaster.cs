using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaster : MonoBehaviour
{
    //()[]
    [Range(0.0f, 1000.0f)]
    public float howMuchParticles;
    public ParticleSystem[] childparticleSystems;
    // Start is called before the first frame update
    void Start()
    {
      

        



    }
    public void ToggleOnParticles()
    {
        foreach (ParticleSystem item in childparticleSystems)
        {
            item.emissionRate = howMuchParticles;
        }
    }
    public void ToggleOffParticles()
    {
        foreach (ParticleSystem item in childparticleSystems)
        {
            item.emissionRate = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
