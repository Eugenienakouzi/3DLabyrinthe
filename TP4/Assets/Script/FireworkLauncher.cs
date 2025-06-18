using UnityEngine;
using System.Collections;

public class FireworkLauncher : MonoBehaviour
{
    public GameObject rocketTrailPrefab;    // Traînée de la fusée
    public GameObject explosionPrefab;      // Effet d’explosion
    public Transform launchPoint;           // Point de départ
    public float launchSpeed = 10f;         // Vitesse de montée
    public float explosionDelay = 1.5f;     // Délai avant explosion

    public AudioClip explosionSound;        // Son à jouer à l'explosion
    public float explosionVolume = 1f;      // Volume du son

    public int numberOfFireworks = 5;       // Nombre total de fusées
    public float intervalBetweenLaunches = 0.3f; // Délai entre chaque lancement

    private bool alreadyLaunched = false;


    void Start()
    {
        // Ne rien faire ici
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyLaunched)
        {
            alreadyLaunched = true;
            StartCoroutine(LaunchMultipleFireworks());
        }
    }

    IEnumerator LaunchMultipleFireworks()
    {
        for (int i = 0; i < numberOfFireworks; i++)
        {
            StartCoroutine(LaunchFirework());
            yield return new WaitForSeconds(intervalBetweenLaunches);
        }
    }

    IEnumerator LaunchFirework()
    {
        // Crée la fusée (petite sphère)
        GameObject rocket = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rocket.transform.position = launchPoint.position;
        rocket.transform.localScale = Vector3.one * 0.2f;
        Destroy(rocket.GetComponent<Collider>());

        // Change couleur fusée
        Renderer rocketRenderer = rocket.GetComponent<Renderer>();
        Color randomColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
        if (rocketRenderer != null)
        {
            rocketRenderer.material.color = randomColor;
        }

        Rigidbody rb = rocket.AddComponent<Rigidbody>();
        rb.useGravity = false;

        Vector3 randomDir = Vector3.up + new Vector3(
            Random.Range(-0.1f, 0.1f),
            0f,
            Random.Range(-0.1f, 0.1f)
        );
        rb.linearVelocity = randomDir.normalized * launchSpeed;

        // Traînée
        GameObject trail = Instantiate(rocketTrailPrefab, rocket.transform);
        trail.transform.localPosition = Vector3.zero;

        // Si ta traînée a un ParticleSystem, on peut aussi changer sa couleur ici :
        ParticleSystem trailPS = trail.GetComponent<ParticleSystem>();
        if (trailPS != null)
        {
            var mainTrail = trailPS.main;
            mainTrail.startColor = randomColor;
        }

        // Attente puis explosion
        yield return new WaitForSeconds(explosionDelay);

        Vector3 pos = rocket.transform.position;
        Destroy(rocket);

        GameObject explosionInstance = Instantiate(explosionPrefab, pos, Quaternion.identity);

        // Ajoute un AudioSource pour jouer le son
        AudioSource.PlayClipAtPoint(explosionSound, pos, explosionVolume);


        // Change couleur explosion si ParticleSystem présent
        ParticleSystem explosionPS = explosionInstance.GetComponent<ParticleSystem>();
        if (explosionPS != null)
        {
            var mainExplosion = explosionPS.main;
            mainExplosion.startColor = randomColor;
        }
    }
}
