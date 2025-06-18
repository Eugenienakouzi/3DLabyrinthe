using Unity.AI.Navigation;
using UnityEngine;

public class BuzzerInteraction : MonoBehaviour
{
    public Transform sphereBouton;   // La sphère à déplacer
    public GameObject murCible;      // Le mur à désactiver
    public GameObject grille;      // Le mur à désactiver
    public float enfoncement = 0.1f; // Distance vers le bas

    public NavMeshSurface navMeshSurface; // Surface à reconstruire


    private bool déjàAppuyé = false;
    private bool joueurEstProche = false;
    private Vector3 positionInitiale;

    void Start()
    {
        if (sphereBouton != null)
            positionInitiale = sphereBouton.localPosition;
    }

    void Update()
    {
        if (joueurEstProche && !déjàAppuyé && Input.GetKeyDown(KeyCode.E))
        {
            if (sphereBouton != null)
                sphereBouton.localPosition = positionInitiale - new Vector3(0, enfoncement, 0);

            if (murCible != null)
            {
                murCible.SetActive(false);
                grille.SetActive(false);
            }

            if (navMeshSurface != null)
                navMeshSurface.BuildNavMesh(); // Mise à jour ici

            déjàAppuyé = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assure-toi que le joueur a bien le tag "Player"
            joueurEstProche = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            joueurEstProche = false;
    }
}
