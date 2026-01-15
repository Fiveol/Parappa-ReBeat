using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip LoadLoop1;
    public AudioClip LoadLoop2;
    public AudioClip LoadLoop3;

    // Optional target scene index (set by other scenes)
    public static int targetSceneIndex = -1;

    // Tracks whether the loading screen has already been used once
    private static bool hasRunOnce = false;

    private AsyncOperation op;

    void Start()
    {
        int sceneToLoad;

        if (targetSceneIndex >= 0)
        {
            // A specific target was provided
            sceneToLoad = targetSceneIndex;
        }
        else
        {
            // No target provided: use first-run / later-run logic
            if (!hasRunOnce)
            {
                hasRunOnce = true;
                sceneToLoad = 1;   // First-run scene
            }
            else
            {
                sceneToLoad = 2;   // Menu scene
            }
        }

        // Clear target so future calls can fall back if needed
        targetSceneIndex = -1;

        StartCoroutine(LoadSceneFlow(sceneToLoad));
    }

    IEnumerator LoadSceneFlow(int sceneIndex)
    {
        // Begin loading the scene in the background
        op = SceneManager.LoadSceneAsync(sceneIndex);
        op.allowSceneActivation = false;

        // --- PLAY LOAD LOOP 1 ---
        audioSource.clip = LoadLoop1;
        audioSource.loop = false;
        audioSource.Play();

        while (audioSource.isPlaying)
            yield return null;

        // --- IF STILL LOADING, LOOP LOAD LOOP 2 ---
        if (op.progress < 0.9f)
        {
            audioSource.clip = LoadLoop2;
            audioSource.loop = true;
            audioSource.Play();

            while (op.progress < 0.9f)
                yield return null;

            audioSource.Stop();
        }

        // --- PLAY LOAD LOOP 3 ---
        audioSource.clip = LoadLoop3;
        audioSource.loop = false;
        audioSource.Play();

        while (audioSource.isPlaying)
            yield return null;

        // --- ACTIVATE THE SCENE ---
        op.allowSceneActivation = true;
    }
}