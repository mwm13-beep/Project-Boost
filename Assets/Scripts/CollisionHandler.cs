using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    AudioSource audioSource;
    bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        print("Landed");
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        Invoke("LoadNext", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        print("Dead");
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
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
