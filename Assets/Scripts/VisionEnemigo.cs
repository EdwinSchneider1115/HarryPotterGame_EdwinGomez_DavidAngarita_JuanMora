using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detección")]
    public Transform jugador;
    public float rangoDeteccion = 10f;
    public float velocidadRotacion = 5f;

    [Header("Disparo")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 10f;
    public float tiempoEntreDisparos = 2f;

    private float timerDisparo = 0f;
    private bool esperandoDisparar = true;
    private GameObject proyectilActivo = null;

    private Rigidbody jugadorRb;

    // NUEVO
    private Animator anim;

    void Start()
    {
        if (jugador != null)
            jugadorRb = jugador.GetComponent<Rigidbody>();

        // NUEVO
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (jugador == null || proyectilPrefab == null || puntoDisparo == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        // NUEVO
        bool detectado = distancia <= rangoDeteccion && TieneLineaDeVision();

        if (anim != null)
            anim.SetBool("DetectaJugador", detectado);

        if (detectado)
        {
            MirarAlJugador();
            ManejarDisparo();
        }
    }

    void ManejarDisparo()
    {
        if (proyectilActivo != null) return;

        if (esperandoDisparar)
        {
            timerDisparo -= Time.deltaTime;
            if (timerDisparo <= 0f && TieneLineaDeVision())
            {
                Disparar();
                esperandoDisparar = false;
            }
        }
        else
        {
            timerDisparo = tiempoEntreDisparos;
            esperandoDisparar = true;
        }
    }

    bool TieneLineaDeVision()
    {
        Vector3 origen = transform.position + Vector3.up;
        Vector3 destino = jugador.position + Vector3.up;
        Vector3 direccion = destino - origen;

        if (Physics.Raycast(origen, direccion.normalized, out RaycastHit hit, rangoDeteccion))
        {
            return hit.collider.CompareTag("Jugador");
        }
        return false;
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

    void Disparar()
    {
        Vector3 posObjetivo = jugador.position + Vector3.up * 1f;
        Vector3 direccion;

        if (jugadorRb != null && jugadorRb.linearVelocity.magnitude > 0.1f)
        {
            float distancia = Vector3.Distance(puntoDisparo.position, posObjetivo);
            float tiempoVuelo = distancia / fuerzaDisparo;
            Vector3 posicionPredicha = posObjetivo + jugadorRb.linearVelocity * tiempoVuelo;
            direccion = (posicionPredicha - puntoDisparo.position).normalized;
        }
        else
        {
            direccion = (posObjetivo - puntoDisparo.position).normalized;
        }

        proyectilActivo = Instantiate(
            proyectilPrefab,
            puntoDisparo.position,
            Quaternion.LookRotation(direccion)
        );

        Proyectil script = proyectilActivo.GetComponent<Proyectil>();
        if (script != null)
        {
            script.Lanzar(direccion, fuerzaDisparo, gameObject);
        }
        else
        {
            Rigidbody rb = proyectilActivo.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = direccion * fuerzaDisparo;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}