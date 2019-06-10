using UnityEngine;

public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 0f, 0f);
    [SerializeField] float period = 2f;
    float movementFactor = 0; // 0 = start, 1 = end

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) period = 1f;
        float cycles = Time.time / period; // grows
        const float tau = Mathf.PI * 2f; // A number
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
