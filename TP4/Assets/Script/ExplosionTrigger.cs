using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public ParticleSystem explosionFX;
    public ParticleSystem smokeFX;
    public ParticleSystem gasFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            explosionFX.Play();
            smokeFX.Play();
            gasFX.Play(); // Ou gasFX.Emit(10) pour contrôle précis
        }
    }
}

