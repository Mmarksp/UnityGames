using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f); //call on its vector coordinates, moves immediately
    [SerializeField] float period = 5f; //2 seconds, how many cycles within this time
    //to-do: remove from inspector later
    float movementFactor; //0, 1 fully moved. Notice that range works similarly to serializefield, but provides a slider

    Vector3 startingPos; //establishing starting coordinates

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; } //protect against dividing by 0 using smallest possible float number epsilon

        float cycles = Time.time / period; //grows continually from 0

        const float tau = Mathf.PI * 2; //6.28, fixed, makes sense considering circle formation and sine wave
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f; //again, understand sinewave and these numbers make sense to constitute the wave formation we want (amplitude, length)
        Vector3 offset = movementVector * movementFactor; //vector, our value, times a factor between 0 and 1. To automate this, we need factor to update increment itself
        transform.position = startingPos + offset; //movement happens
    }
}
