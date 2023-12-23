using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcDamage : MonoBehaviour
{
    PostProcessVolume _volume;
    private float intensity = 0;
    Vignette _vignette;
    void Start()
    {
        _volume = GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings<Vignette>(out _vignette);
        if (!_vignette)
        {
            Debug.LogError("No vignette found");
        }
        else
        {
            _vignette.enabled.Override(false);
        }
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().isHit)
        {
           StartCoroutine(TakeDamageEffect());
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        intensity = 1f;
        _vignette.enabled.Override(true);
        _vignette.intensity.Override(intensity);
        yield return new WaitForSeconds(0.4f);
        while (intensity > 0)
        {
            intensity -= 0.01f;
            if (intensity < 0) intensity = 0;
            _vignette.intensity.Override(intensity);
            yield return new WaitForSeconds(0.1f);

        }

        _vignette.enabled.Override(false);
        yield break;
    }
}
