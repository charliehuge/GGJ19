
using UnityEngine;

public class Spore : MonoBehaviour
{
    private Sampler _sampler;

    private void Awake()
    {
        _sampler = GetComponent<Sampler>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        _sampler.test = true;
    }
}