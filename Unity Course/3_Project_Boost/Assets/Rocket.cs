using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //To-do: allow for more than two levels
    [SerializeField] float rcsThrust = 100f; //create variables for use in-editor (and probably more)
    [SerializeField] float mainThrust = 10f; //float for number value, but not int
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip win;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem winParticles;

    Rigidbody rigidBody; //Game object being given variable
    AudioSource audioSource;
    
    enum State { Alive, Dying, Transcending }; //array
    State state = State.Alive;

    [SerializeField] bool collissionsAreEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); //GetComponent from Unity, here instantiated GameObject. Don't know the difference between this and above.
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        if (Input.GetKey(KeyCode.C))
        {
            collissionsAreEnabled = !collissionsAreEnabled; //toggle
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime; //Compensating for varying framerates

        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame); 
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; //take manual control of the rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame); //forward. Remember finger positioning. Transform. Game Object.
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation
    }

    void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive || !collissionsAreEnabled) { return; } //ignore collisions when dead

        switch (collision.gameObject.tag) //reference game objects and then look at individual tags
        {
            case "Friendly":
                break;
            case "Finish":
                StartWinSequence();
                break;
            default:
                    StartCrashSequence();
                break;

        }
    }

    private void StartWinSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(win);
        winParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay); //parameterize time
    }

    private void StartCrashSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        Invoke("ReloadCurrentLevel", levelLoadDelay);
    }

    private void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        } 
        SceneManager.LoadScene(nextSceneIndex);
    }
}
