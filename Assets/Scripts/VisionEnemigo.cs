using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform jugador; // 🔥 ya lo usas

    public float rangoDeteccion = 10f;
    public float velocidadRotacion = 5f;

    // 🔥 NUEVO: disparo
    [Header("Disparo")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 10f;
    public float tiempoEntreDisparos = 2f;

    private float siguienteDisparo = 0f;

    void Update()
    {
        if (jugador == null)
        {
            Debug.LogError("❌ NO HAY JUGADOR ASIGNADO");
            return;
        }

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= rangoDeteccion)
        {
            MirarAlJugador();

            // 🔥 SOLO añade esto
            if (Time.time >= siguienteDisparo)
            {
                Disparar();
                siguienteDisparo = Time.time + tiempoEntreDisparos;
            }
        }
    }

    void MirarAlJugador()
    {
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0f;

        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionObjetivo,
                velocidadRotacion * Time.deltaTime
            );
        }
    }

    // 🔥 NUEVO MÉTODO
    void Disparar()
    {
        GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);

        Rigidbody rb = proyectil.GetComponent<Rigidbody>();

        Vector3 direccion = jugador.position - puntoDisparo.position;

        // 🔥 altura de la parábola
        direccion.y += 2f;

        rb.linearVelocity = direccion.normalized * fuerzaDisparo;
    }
}