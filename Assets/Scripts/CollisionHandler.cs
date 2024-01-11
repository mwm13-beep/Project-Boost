using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;


    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;
    bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            print("Loading next level");
            LoadNext();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            isTransitioning = !isTransitioning;

            if (isTransitioning)
            {
                print("Collision disabled");
            }
            else
            {
                print("Collision enabled");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning) { return; } //ignore collisions when dead
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Landing":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        successParticles.Play();
        print("Landed");
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        Invoke("LoadNext", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        crashParticles.Play();
        print("Dead");
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        Invoke("ReloadLevel", levelLoadDelay);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        isTransitioning = false;
    }

    private void LoadNext()
    {
        //next scene + 1 modulo the number of scenes in the build settings
        int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
        isTransitioning = false;
    }
}
