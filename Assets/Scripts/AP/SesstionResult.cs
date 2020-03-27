using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SesstionResult : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private bool isPlaying = false;

    public void Play()
    {
        isPlaying = true;
        if (!particleSystem.isPlaying)
            particleSystem.Play();
    }

    // Start is called before the first frame update
    void Awake()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying && !particleSystem.isPlaying)
        {
            isPlaying = false;
            this.gameObject.Kill();
        }
    }
}
