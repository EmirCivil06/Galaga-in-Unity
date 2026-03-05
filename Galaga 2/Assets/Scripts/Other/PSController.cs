using System.Collections;
using UnityEngine;

public class PSController : MonoBehaviour
{
    private ParticleSystem _ps;
    void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        StartCoroutine(CheckIfFinished());
    }

    IEnumerator CheckIfFinished()
    {
        while (_ps.isPlaying)
        {
            yield return new WaitForSeconds(0.7f);
        }

        ObjectPoolManager.ReleaseObject(gameObject, ObjectPoolManager.PoolType.ParticleSytems);
    }
}
