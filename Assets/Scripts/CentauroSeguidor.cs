using UnityEngine;
using System.Collections;

public class CentauroSeguidor : MonoBehaviour
{
    [Header("Persecucion")]
    public Transform jugador;
    public float velocidad = 5f;
    public float distanciaMinima = 1f;

    [Header("Daño")]
    public int danioPorGolpe = 1;
    public float tiempoEntreGolpes = 1f;

    private bool persiguiendo = false;
    private Animator anim;

    private bool jugadorEnContacto = false;
    private PlayerStats playerStats;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (anim != null)
        {
            anim.SetBool("Persiguiendo", persiguiendo);
        }

        if (!persiguiendo || jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaMinima) return;

        Vector3 direccion = (jugador.position - transform.position).normalized;

        transform.position += direccion * velocidad * Time.deltaTime;

        transform.LookAt(
            new Vector3(
                jugador.position.x,
                transform.position.y,
                jugador.position.z
            )
        );
    }

    public void ActivarPersecucion()
    {
        persiguiendo = true;
    }

    public void DesactivarPersecucion()
    {
        persiguiendo = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Jugador"))
        {
            playerStats = collision.gameObject.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                jugadorEnContacto = true;
                StartCoroutine(HacerDanioContinuo());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Jugador"))
        {
            jugadorEnContacto = false;
            playerStats = null;
        }
    }

    IEnumerator HacerDanioContinuo()
    {
        while (jugadorEnContacto && playerStats != null)
        {
            playerStats.RecibirDanio(danioPorGolpe);
            yield return new WaitForSeconds(tiempoEntreGolpes);
        }
    }
}