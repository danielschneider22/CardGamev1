using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private float shakeTimeRemaining, shakePower;
    private Vector3 origPosition;
    // Start is called before the first frame update
    void Start()
    {
        origPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartShake(.5f, 1f);
        }
    }

    void LateUpdate()
    {
        if(shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);
        } else
        {
            transform.position = new Vector3(origPosition.x, origPosition.y, origPosition.z);
        }
    }

    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;
    }
}
