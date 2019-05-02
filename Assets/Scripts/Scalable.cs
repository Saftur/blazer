using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalable : MonoBehaviour
{
    public delegate void ScaleHandler(float size);
    public event ScaleHandler OnScale;

    private float SizeScalar = 1.0f;
    private Vector3 Scale;

    // Start is called before the first frame update
    void Start()
    {
        Scale = gameObject.transform.localScale;
    }

    public void ChangeScale(float Scalar)
    {
        SizeScalar += Scalar;
        gameObject.transform.localScale.Set(Scale.x * SizeScalar, Scale.y * SizeScalar, Scale.z);
        OnScale?.Invoke(SizeScalar);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Scalar"))
        {
            Bounds selfBounds = GetComponent<BoxCollider2D>().bounds;
            Bounds otherBounds = other.GetComponent<BoxCollider2D>().bounds;

            if (selfBounds.Contains(otherBounds.max) && selfBounds.Contains(otherBounds.min))
            {
                ChangeScale(other.gameObject.GetComponent<SizeChanger>().ScaleAmount);
                Destroy(other.gameObject);
            }

        }
    }
}
