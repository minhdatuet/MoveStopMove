using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem levelUpParticle;

    private void Start()
    {
        deathParticle = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        levelUpParticle = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public void StartDeathParticle()
    {
        deathParticle.Play();
    }

    public void StartLevelUpParticle()
    {
        levelUpParticle.Play();
    }

    public void SetParticleSystemColor(Material material)
    { 
        if (deathParticle && levelUpParticle)
        {
            deathParticle.startColor = material.color;
            levelUpParticle.startColor = material.color;
            deathParticle.gameObject.GetComponent<Renderer>().material = material;
            levelUpParticle.gameObject.GetComponent<Renderer>().material = material;
        }
    }
}
