using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    public int amountBoids;
    public GameObject boidPrefab;
    public Transform boidGoal;
    public float maxSpeed;

    private Boid[] boids;
    void Start()
    {
        boids = new Boid[amountBoids];
        InitializeBoids();
    }

    void FixedUpdate()
    {
        foreach (var b in boids)
        {
            b.Update();
            
            Vector3 velocity = b.velocity + BoidsRule1(b) + BoidsRule2(b) + BoidsRule3(b) + BoidsRule4(b);
            
            float x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
            float y = Mathf.Clamp(velocity.y, -maxSpeed, maxSpeed);
            float z = Mathf.Clamp(velocity.z, -maxSpeed, maxSpeed);

            Vector3 clampedVelocity = new Vector3(x, y, z);
            Quaternion rotation = Quaternion.LookRotation(clampedVelocity, Vector3.up) * Quaternion.Euler(0f, 90f, 0f);
            b.boidObject.transform.rotation = Quaternion.Slerp(b.boidObject.transform.rotation, rotation, 5f * Time.deltaTime);;

            b.Pos = b.Pos + clampedVelocity;
            //Debug.Log(velocity.magnitude);
        }
    }

    private Vector3 BoidsRule1(Boid bj)
    {
        Vector3 pc = Vector3.zero;
        foreach (var b in boids)
        {
            if (b != bj)
            {
                pc += b.Pos;
            }
        }
        pc = pc / (amountBoids - 1);
        return (pc - bj.Pos) / 10000;
    }

    private Vector3 BoidsRule2(Boid bj)
    {
        Vector3 c = Vector3.zero;

        foreach (var b in boids)
        {
            if (b != bj)
            {
                if (Vector3.Distance(b.Pos, bj.Pos) < 0.5f)
                {
                    c = c - (b.Pos - bj.Pos);
                }
            }
        }

        return c / 100;
    }

    private Vector3 BoidsRule3(Boid bj)
    {
        Vector3 pvj = Vector3.zero;
        foreach (var b in boids)
        {
            if (b != bj)
            {
                pvj += b.velocity;
            }
        }
        pvj = pvj / (amountBoids - 1);
        return (pvj - bj.velocity) / 800;
    }

    private Vector3 BoidsRule4(Boid bj)
    {
        return (boidGoal.position - bj.Pos) / 10000;
    }

    private void InitializeBoids()
    {
        for (int i = 0; i < amountBoids; i++)
        {
            boids[i] = new Boid(boidPrefab, new Vector3(0, 0, i / 9000));
        }
    }
}

public class Boid
{
    public GameObject boidObject;
    public Vector3 Pos
    {
        get
        {
            return boidObject.transform.position;
        }
        set
        {
            boidObject.transform.position = value;
        }
    }
    public Vector3 velocity;

    private Vector3 previous;

    public Boid(GameObject prefab, Vector3 startingPoint)
    {
        boidObject = GameObject.Instantiate(prefab, startingPoint, Quaternion.identity);
        previous = startingPoint + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
    }

    public void Update()
    {
        velocity = (boidObject.transform.position - previous);
        float x = Mathf.Clamp(velocity.x, -0.03f, 0.03f);
        float y = Mathf.Clamp(velocity.y, -0.03f, 0.03f);
        float z = Mathf.Clamp(velocity.z, -0.03f, 0.03f);
        velocity = new Vector3(x, y, z);
        //Debug.Log(boidObject.transform.position + "      " + previous);
        //Debug.Log((boidObject.transform.position - previous).magnitude);
        previous = boidObject.transform.position;
    }
}
