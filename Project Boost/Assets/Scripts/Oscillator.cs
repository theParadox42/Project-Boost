using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 0f, 0f);
    [SerializeField] float period = 2f;
    //[SerializeField] [Range(0, 360)] float rotated = 0f;
    [SerializeField] bool useCos = false;
    float movementFactor = 0; // 0 = start, 1 = end

    Vector3 startingPos;
    Vector3 previousOffset = new Vector3(0, 0, 0);

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
        float rawSinWave;
        if (useCos)
        {
            rawSinWave = Mathf.Sin(cycles * tau);
        } else
        {
            rawSinWave = Mathf.Cos(cycles * tau);
        }
        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        if (offset.x <= Mathf.Epsilon)
        {
            offset.x = transform.position.x - startingPos.x;
        }
        if(offset.y <= Mathf.Epsilon)
        {
            offset.y = startingPos.y - transform.position.y;
        }
        if(offset.z <= Mathf.Epsilon)
        {
            offset.z = startingPos.z - transform.position.z;
        }
        transform.position = startingPos + offset;

        previousOffset = offset;
    }
}
