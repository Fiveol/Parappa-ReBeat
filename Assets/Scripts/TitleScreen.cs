using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip title1;
    public AudioClip title2;

    private bool title2Started = false;

    void Start()
    {
        // Start by playing Title1
        audioSource.clip = title1;
        audioSource.loop = false;
        audioSource.Play();
    }

    void Update()
    {
        // ENTER pressed at any time
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!title2Started)
            {
                // Skip to Title2 immediately
                PlayTitle2();
            }
            else
            {
                // Already in Title2 → Load Menu through LoadingScreen
                LoadMenu();
            }
        }

        // If Title1 finishes naturally, start Title2
        if (!title2Started && !audioSource.isPlaying)
        {
            PlayTitle2();
        }
    }

    void PlayTitle2()
    {
        title2Started = true;
        audioSource.clip = title2;
        audioSource.loop = false;
        audioSource.Play();
    }

    void LoadMenu()
    {
        // Tell the loading screen to load scene 2 (Menu)
        LoadingScreen.targetSceneIndex = 2;

        // Load the loading screen (scene 0)
        SceneManager.LoadScene(0);
    }
}