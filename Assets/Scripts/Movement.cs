using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float upThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip thrusterSFX;
    [SerializeField] ParticleSystem mainThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    
    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartMainThruster();
        }
        else
        {
            StopMainThruster();
        }
    }

    private void StopMainThruster()
    {
        StopThrusterSFX();
        mainThrusterParticles.Stop();
    }

    private void StopThrusterSFX()
    {
        //stop the thruster audio if there is no input
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.Space))
        {
            audioSource.Stop();
        }
    }

    private void StartMainThruster()
    {
        //add force to the rocket
        rb.AddRelativeForce(Vector3.up * upThrust * Time.deltaTime);
        PlayThrusterParticleFX(mainThrusterParticles);
        PlayThrusterSFX(thrusterSFX);
    }

    private void PlayThrusterSFX(AudioClip sfx)
    {
        //play thruster audio
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sfx);
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A)) //rotate to the left
        {
            StartRotationThrust(-1, leftThrusterParticles);
        }
        else if (Input.GetKey(KeyCode.D)) //rotate to the right
        {
            StartRotationThrust(1, rightThrusterParticles);
        }
        else
        {
            StopRotationThrusters();
        }
    }

    private void StopRotationThrusters()
    {
        //stop the thruster particles and don't rotate
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
        StopThrusterSFX();
    }

    private void StartRotationThrust(int rotation, ParticleSystem rightThrusterParticles)
    {
        PlayThrusterParticleFX(rightThrusterParticles);
        PlayThrusterSFX(thrusterSFX);
        Rotate(-rotation * rotationThrust);
    }

    private void PlayThrusterParticleFX(ParticleSystem effect)
    {
        //play the thruster particles
        if (!effect.isPlaying)
        {
            effect.Play();
        }
    }

    void Rotate(float rotation)
    {
        rb.freezeRotation = true; //freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
        rb.freezeRotation = false; //unfreezing rotation so the physics system can take over
    }
}
