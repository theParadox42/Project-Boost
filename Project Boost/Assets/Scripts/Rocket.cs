using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    // Power levels
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 25f;
    // Level Delay
    [SerializeField] float levelLoadDelay = 2f;

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

    // Mobile controls
    bool isHolding;
    int waitingTime = 20;
    int timeWait;
    bool thrustingThisFrame = false;

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
            RespondToRotateInput();
            RespondToThrustInput();
        } else
        {
            thrustParticles.Stop();
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
        Invoke("ReloadLevel", levelLoadDelay);
        deathParticles.Play();
    }

    // Called when the rocket has finished the level
    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successAudio);
        Invoke("LoadNextScene", levelLoadDelay);
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

        float rotationThisSpeed = rcsThrust * Time.deltaTime;

        HandleMobile(rotationThisSpeed);
        HandleKeyboard(rotationThisSpeed);

        rigidBody.freezeRotation = false;
    }

    // Keyboard controls
    private void HandleKeyboard(float rotationThisSpeed)
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            RotateLeft(rotationThisSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            RotateRight(rotationThisSpeed);
        }
    }

    // Mobile controls
    private void HandleMobile(float rotationThisSpeed)
    {
        // v1
        {/*
        bool onLeft = false;
        bool onRight = false;
        bool someFinger = false;
        thrustingThisFrame = false;
        foreach (Touch touch in Input.touches)
        {
            someFinger = true;
            if (touch.position.x <= Screen.width / 2)
            {
                onLeft = true;
            }
            if (touch.position.x >= Screen.width / 2)
            {
                onRight = true;
            }
        }
        if(!someFinger && isHolding)
        {
            isHolding = false;
        }
        if (isHolding)
        {
            if(!onLeft || !onRight)
            {
                timeWait--;
                if(timeWait <= 0)
                {
                    isHolding = false;
                }
            } else
            {
                timeWait = waitingTime;
            }
        }
        if ((onLeft && onRight) || isHolding)
        {
            ApplyThrust();
        }
        else if (onLeft)
        {
            ApplyThrust(0.3f);
            RotateLeft(rotationThisSpeed);
        }
        else if (onRight)
        {
            RotateRight(rotationThisSpeed);
            ApplyThrust(0.3f);
        }
        */}
        // v2
        {/*
        bool onLeft = false;
        bool onRight = false;
        bool onBottom = false;
        thrustingThisFrame = false;
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.y <= Screen.height / 3)
            {
                onBottom = true;
            } 
            if (touch.position.x <= Screen.width / 2)
            {
                onLeft = true;
            }
            if (touch.position.x >= Screen.width / 2)
            {
                onRight = true;
            }
        }
        if (onBottom || (onLeft && onRight))
        {
            ApplyThrust(1f);
        }
        else if (onLeft)
        {
            RotateLeft(rotationThisSpeed);
        }
        else if (onRight)
        {
            RotateRight(rotationThisSpeed);
        }
        */}
        // v3
        thrustingThisFrame = false;
        float yPercent = 0.7f;
        float x1Percent = 0.4f;
        float x2Percent = 0.7f;
        bool thrustPressed = false;
        bool rightPressed = false;
        bool leftPressed = false;
        foreach (Touch touch in Input.touches)
        {
            //print(touch.position.y);
            if (touch.position.y <= Screen.height * (1 - yPercent))
            {
                float tx = touch.position.x;
                float sw = Screen.width;
                if (tx <= sw * x1Percent)
                {
                    thrustPressed = true; ;
                } else if(tx <= sw * x2Percent)
                {
                    leftPressed = true;
                } else
                {
                    rightPressed = true;
                }
            }
            if (thrustPressed) ApplyThrust();
            if (rightPressed) RotateRight(rotationThisSpeed); 
            if (leftPressed) RotateLeft(rotationThisSpeed);
        }
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
        else if(!thrustingThisFrame)
        {
            audioSource.Stop();
            thrustParticles.Stop();
        }
    }

    // Apply a thrustlike force to the rocket
    private void ApplyThrust(float mult)
    {
        thrustingThisFrame = true;
        mult = mult > Mathf.Epsilon ? mult : 1f;
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime * 10 * mult);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustAudio);
        }
        thrustParticles.Play();
    }

    // Without input
    private void ApplyThrust()
    {
        ApplyThrust(1f);
    }
}
