using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCurrent : MonoBehaviour
{
    


    public ParticleSystem particles;

    public float maxStrength = 2;
    public float minStrength = 0.1f;
    public float acceleration = 1;  // speed at which player's velocity changes

    public bool changingSpeed = false;
    public bool vertical = true;
    public float cycleSpeed = 1;

    public float currentStrength;

    bool increasingSpeed = true;
    float strengthLerpLeway = 0.1f;   // how close currentStrength needs to get to the max or min before changing direction
    
    // Start is called before the first frame update
    void Start()
    {
        currentStrength = maxStrength;
    }

    // Update is called once per frame
    void Update()
    {
        //here we handle the speed slowing and increasing over time
        if (changingSpeed)
        {
            if (increasingSpeed)
            {
                currentStrength = Mathf.Lerp(currentStrength, maxStrength, cycleSpeed * Time.deltaTime);
                if (currentStrength > (maxStrength - strengthLerpLeway))
                {
                    increasingSpeed = false;
                }
            }
            else
            {
                currentStrength = Mathf.Lerp(currentStrength, minStrength, cycleSpeed * Time.deltaTime);
                if (currentStrength < (minStrength + strengthLerpLeway))
                {
                    increasingSpeed = true;
                }
            }
        }

        //Here we edit the speed of particles to give feedback on the wind speed
        var v = particles.main;
        v.startSpeed = new ParticleSystem.MinMaxCurve(currentStrength/2, currentStrength);

        if (vertical)
        {
            ParticleSystem.Particle[] p = new ParticleSystem.Particle[particles.particleCount + 1];
            int l = particles.GetParticles(p);

            int i = 0;
            while (i < l)
            {
                p[i].velocity = new Vector3(0, currentStrength, 0);
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
                p[i].velocity = new Vector3(currentStrength, 0, 0);
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
                if (rb.velocity.y < currentStrength)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + acceleration*currentStrength);
                }
            }
            else
            {
                if (rb.velocity.x < currentStrength)
                {
                    rb.velocity = new Vector2(rb.velocity.x + acceleration * currentStrength, rb.velocity.y);
                }
            }
            
        }
    }
}
