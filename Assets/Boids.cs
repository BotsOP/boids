using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    public int amountBoids;
    public GameObject boidPrefab;

    private Boid[] boids;
    // Start is called before the first frame update
    void Start()
    {
        boids = new Boid[amountBoids];
        InitializeBoids();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var b in boids)
        {
            b.Update();
            
            float x = Mathf.Clamp(b.velocity.x, -0.03f, 0.03f);
            float y = Mathf.Clamp(b.velocity.y, -0.03f, 0.03f);
            float z = Mathf.Clamp(b.velocity.z, -0.03f, 0.03f);
            Vector3 clampedVelocity = new Vector3(x, y, z);
            Vector3 velocity = clampedVelocity + BoidsRule1(b) + BoidsRule2(b);
            b.Pos = b.Pos + velocity;
            Debug.Log(velocity.magnitude);
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
        return (pc - bj.Pos) / 1000;
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

        return c / 10;
    }

    private void InitializeBoids()
    {
        for (int i = 0; i < amountBoids; i++)
        {
            boids[i] = new Boid(boidPrefab, new Vector3(0, 0, i));
        }
    }
}

public class Boid
{
    private GameObject boidObject;
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
        //Debug.Log(boidObject.transform.position + "      " + previous);
        //Debug.Log((boidObject.transform.position - previous).magnitude);
        previous = boidObject.transform.position;
    }
}
