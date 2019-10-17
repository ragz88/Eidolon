using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCurrentChelleResponse : MonoBehaviour
{

    public MichelleWindController chelleController;
    public ParticleSystem particles;

    public float phaseOneWindSpeed = 0;
    public float phaseTwoWindSpeed = 0;
    public float pauseWindSpeed = 0;

    public float windSpeedChangeRate = 2;

    public bool getChangeRateFromChelle = false;

    public bool vertical = true;

    public float currentSpeed = 0;

    public float partSpeedMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (getChangeRateFromChelle)
        {
            windSpeedChangeRate = chelleController.windSpeedChangeRate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (chelleController.windState == MichelleWindController.WindState.BlowingPhase1)
        {
            // Change speed to a specific value
            if (Mathf.Abs(currentSpeed - phaseOneWindSpeed) > 0.05f)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, phaseOneWindSpeed, windSpeedChangeRate * Time.deltaTime);
            }
        }
        else if (chelleController.windState == MichelleWindController.WindState.BlowingPhase2)
        {
            // Change speed to a specific value
            if (Mathf.Abs(currentSpeed - phaseTwoWindSpeed) > 0.05f)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, phaseTwoWindSpeed, windSpeedChangeRate * Time.deltaTime);
            }
        }
        else if (chelleController.windState == MichelleWindController.WindState.Pause)
        {
            // Change speed to a specific value
            if (Mathf.Abs(currentSpeed - pauseWindSpeed) > 0.05f)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, pauseWindSpeed, windSpeedChangeRate * Time.deltaTime);
            }
        }

        //Here we edit the speed of particles to give feedback on the wind speed
        var v = particles.main;
        v.startSpeed = new ParticleSystem.MinMaxCurve(currentSpeed / 2, currentSpeed);

        if (vertical)
        {
            ParticleSystem.Particle[] p = new ParticleSystem.Particle[particles.particleCount + 1];
            int l = particles.GetParticles(p);

            int i = 0;
            while (i < l)
            {
                p[i].velocity = new Vector3(0, currentSpeed * partSpeedMultiplier, 0);
                i++;
            }

            particles.SetParticles(p, l);
        }
        else
        {
            ParticleSystem.Particle[] p = new ParticleSystem.Particle[particles.particleCount + 1];
            int l = particles.GetParticles(p);

            int i = 0;
            while (i < l)
            {
                p[i].velocity = new Vector3(currentSpeed, 0, 0);
                i++;
            }

            particles.SetParticles(p, l);
        }


    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

            if (vertical)
            {
                if (Mathf.Abs(rb.velocity.y - currentSpeed) > 0.05f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + chelleController.playerAcceleration * currentSpeed);
                }
            }
            else
            {
                if (Mathf.Abs(rb.velocity.x - currentSpeed) > 0.05f)
                {
                    other.gameObject.GetComponent<ClarenceMovement>().horizontalSpeedAdjustment =
                        chelleController.playerAcceleration * currentSpeed;
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ClarenceMovement>().horizontalSpeedAdjustment = 0;
        }

    }
}
