using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Roam : MonoBehaviour
{
    [SerializeField] [Tooltip("Movement speed.")] private float speed;
    [SerializeField] [Tooltip("Rotation speed, in radians/sec.")] private float rotationSpeed;

    [Header("Fleeing")]
    [SerializeField] [Tooltip("Do we flee when the player is near us?")] private bool flees;
    [SerializeField] [Tooltip("How long to flee for.")] private float fleeDuration;
    [SerializeField] [Tooltip("Player detection range.")] private float detectionRange;
    [SerializeField] [Tooltip("Movement speed while fleeing.")] private float fleeSpeed;
    [SerializeField] [Tooltip("Rotation speed while fleeing.")] private float fleeRotationSpeed;
    [SerializeField] [Tooltip("Odds of changing target every frame while fleeing.")] private float fleeTargetChangeOdds = 0.1f;
    [SerializeField] [Tooltip("How likely we are to flee every frame while the player is within range. 1 to always flee.")]
    private float fleeOdds = 1.0f;

    [Header("Boundaries")]
    [SerializeField] [Tooltip("The upper-left hand corner of the behavior's region.")] private Vector3 minCorner;
    [SerializeField] [Tooltip("The bottom-right hand corner of the behavior's region.")] private Vector3 maxCorner;
    [SerializeField] [Tooltip("Error margain for distance calculations.")] private float epsilon = 1.0f;

    private float fleeTimer = 0.0f; // Time remaining before we stop fleeing.
    private Vector3 roamTarget;     // Current move target.
    private Rigidbody2D body;       // Our Rigidbody.
    private GameObject player;      // The player.

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        setTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate facing dir
        Vector3 vectorToTarget = roamTarget - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        // Are we fleeing?
        if (fleeTimer > 0.0f)
        {
            // Choose a new movement target to simulate eratic movement.
            if (Random.Range(0.0f, 1.0f) <= fleeTargetChangeOdds)
            setTarget();

            // Set velocity
            body.velocity = transform.right * fleeSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * fleeRotationSpeed);

            // Update timer.
            fleeTimer -= Time.deltaTime;
        }
        // ...should we be?
        else if (flees && Vector3.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            // PRAISE RNGESUS!
            if (Random.Range(0.0f, 1.0f) <= fleeOdds)
            {
                // Flee the player.
                fleeTimer = fleeDuration;
            }
        }
        else
        {
            // Set velocity and rotation.
            body.velocity = transform.right * speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
        }
        
        // Pick a new target.
        if (Vector3.Distance(transform.position, roamTarget) <= epsilon)
        {
            setTarget();
        }
    }

    // Sets our movement target to a random location, within the rectangular region defined by [minCorner, maxCorner]
    void setTarget()
    {
        roamTarget = new Vector3(Random.Range(minCorner.x, maxCorner.x), Random.Range(minCorner.y, maxCorner.y), transform.position.z);
    }
}
