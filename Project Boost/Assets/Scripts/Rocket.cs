using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    // Power levels
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 25f;

    // Audio
    [SerializeField] AudioClip thrustAudio;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] AudioClip successAudio;

    // Particle systems
    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    // Components
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Current State
    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
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
    }

    // Called on a collision, object passed in
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // Do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    // Called when the rocket has died
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathAudio);
        Invoke("ReloadLevel", 2f);
        deathParticles.Play();
    }

    // Called when the rocket has finished the level
    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successAudio);
        Invoke("LoadNextScene", 1f);
        successParticles.Play();
    }

    // Reload the current level, usually on death
    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Load next level, on level finish
    private void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            LoadFirstLevel();
        } else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    // Load the very first level, on finish of all existing levels in the build settings
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    // Respond to the user input relating to rotational controls
    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;

        //Vector3 angularVelocity = rigidBody.angularVelocity
        float rotationThisSpeed = rcsThrust * Time.deltaTime;

        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.LogWarning("Do mobile here");
            /*
            foreach (Touch touch in Input.touches)
            {

            }
            */          
        } else {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                RotateLeft(rotationThisSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                RotateRight(rotationThisSpeed);
            }
        }

        rigidBody.freezeRotation = false;
    }

    // Rotate the rocket clockwise
    private void RotateRight(float rotationThisSpeed)
    {
        transform.Rotate(-Vector3.forward * rotationThisSpeed);
    }

    // Rotate the rocket counterclockwise
    private void RotateLeft(float rotationThisSpeed)
    {
        transform.Rotate(Vector3.forward * rotationThisSpeed);
    }

    // Respond to user input relating to thrusting controls
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            thrustParticles.Stop();
        }
    }

    // Apply a thrustlike force to the rocket
    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustAudio);
        }
        thrustParticles.Play();
    }
}
