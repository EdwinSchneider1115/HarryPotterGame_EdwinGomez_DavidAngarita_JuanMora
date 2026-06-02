using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public enum EjeMov { Horizontal, Vertical, Profundidad }

    [Header("Configuración de Movimiento")]
    public EjeMov eje = EjeMov.Horizontal;

    [Tooltip("Distancia total que recorre la plataforma (mitad hacia cada lado)")]
    public float rango = 3f;

    [Tooltip("Velocidad de movimiento")]
    public float velocidad = 2f;

    private Vector3 posicionInicial;
    private float progreso = 0f;
    private int direccion = 1;

    // NUEVO
    private Vector3 ultimaPosicion;
    private Transform jugadorEncima;

    void Start()
    {
        posicionInicial = transform.position;

        // NUEVO
        ultimaPosicion = transform.position;
    }

    void Update()
    {
        progreso += velocidad * direccion * Time.deltaTime;

        if (progreso >= rango)
        {
            progreso = rango;
            direccion = -1;
        }
        else if (progreso <= -rango)
        {
            progreso = -rango;
            direccion = 1;
        }

        Vector3 offset = eje switch
        {
            EjeMov.Horizontal => new Vector3(progreso, 0f, 0f),
            EjeMov.Vertical => new Vector3(0f, progreso, 0f),
            EjeMov.Profundidad => new Vector3(0f, 0f, progreso),
            _ => Vector3.zero
        };

        transform.position = posicionInicial + offset;

        // NUEVO
        Vector3 movimiento = transform.position - ultimaPosicion;

        if (jugadorEncima != null)
        {
            jugadorEncima.position += movimiento;
        }

        ultimaPosicion = transform.position;
    }

    // CORREGIDO
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Jugador"))
        {
            jugadorEncima = collision.transform;
        }
    }

    // CORREGIDO
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Jugador"))
        {
            jugadorEncima = null;
        }
    }

    // Dibuja el rango en el editor para que lo veas visualmente
    private void OnDrawGizmosSelected()
    {
        Vector3 pos = Application.isPlaying ? posicionInicial : transform.position;

        Vector3 dir = eje switch
        {
            EjeMov.Horizontal => Vector3.right,
            EjeMov.Vertical => Vector3.up,
            EjeMov.Profundidad => Vector3.forward,
            _ => Vector3.right
        };

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(pos - dir * rango, pos + dir * rango);
        Gizmos.DrawWireSphere(pos - dir * rango, 0.15f);
        Gizmos.DrawWireSphere(pos + dir * rango, 0.15f);
    }
}