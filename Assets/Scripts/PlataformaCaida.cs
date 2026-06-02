using UnityEngine;
using System.Collections;

public class PlataformaCaida : MonoBehaviour
{
    [Header("Caída")]
    public float tiempoAntesDeCaer = 2f;
    public float velocidadCaida = 10f;
    public float distanciaCaida = 30f;

    [Header("Recuperación")]
    public float tiempoAbajo = 4f;
    public float velocidadSubida = 5f;

    private bool activada = false;
    private bool cayendo = false;
    private bool subiendo = false;

    private Vector3 posicionInicial;
    private Vector3 posicionCaida;

    private void Start()
    {
        posicionInicial = transform.position;
        posicionCaida = posicionInicial + Vector3.down * distanciaCaida;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (activada) return;

        if (collision.gameObject.CompareTag("Jugador"))
        {
            activada = true;
            StartCoroutine(CicloPlataforma());
        }
    }

    IEnumerator CicloPlataforma()
    {
        yield return new WaitForSeconds(tiempoAntesDeCaer);

        cayendo = true;

        while (Vector3.Distance(transform.position, posicionCaida) > 0.05f)
        {
            yield return null;
        }

        cayendo = false;

        yield return new WaitForSeconds(tiempoAbajo);

        subiendo = true;

        while (Vector3.Distance(transform.position, posicionInicial) > 0.05f)
        {
            yield return null;
        }

        subiendo = false;
        activada = false;
    }

    private void Update()
    {
        if (cayendo)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicionCaida,
                velocidadCaida * Time.deltaTime
            );
        }

        if (subiendo)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicionInicial,
                velocidadSubida * Time.deltaTime
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 inicio = Application.isPlaying
            ? posicionInicial
            : transform.position;

        Vector3 fin = inicio + Vector3.down * distanciaCaida;

        Gizmos.DrawLine(inicio, fin);
        Gizmos.DrawWireSphere(fin, 0.3f);
    }
}