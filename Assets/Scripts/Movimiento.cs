using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Movimiento : MonoBehaviour
{
    public float velocidad = 6f;
    public float velocidadCorrer = 12f;
    public float sensibilidadMouse = 200f;
    public float fuerzaSalto = 10f;

    private Rigidbody rb;
    private int saltosRestantes = 2;

    private Vector3 normalPared = Vector3.zero;
    private bool enPared = false;

    private Vector3 checkpointPosition;

    // 🔥 Animator
    private Animator animator;

    // 🔥 Crouch
    private bool isCrouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        checkpointPosition = transform.position;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        transform.Rotate(0f, mouseX, 0f);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direccion = transform.forward * z + transform.right * x;
        direccion.Normalize();

        if (enPared)
        {
            direccion = Vector3.ProjectOnPlane(direccion, normalPared);
        }

        float velocidadActual = velocidad;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocidadActual = velocidadCorrer;
        }

        rb.linearVelocity = new Vector3(direccion.x * velocidadActual, rb.linearVelocity.y, direccion.z * velocidadActual);

        // 🔥 WALK
        if (direccion.magnitude > 0)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }

        // 🔥 RUN
        if (Input.GetKey(KeyCode.LeftShift) && direccion.magnitude > 0 && !isCrouching)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }

        // 🔥 CROUCH (toggle con C)
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            animator.SetBool("crouch", isCrouching);
        }

        // 🔥 JUMP
        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            saltosRestantes--;

            animator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            saltosRestantes = 2;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contacto in collision.contacts)
        {
            if (Vector3.Dot(contacto.normal, Vector3.up) < 0.5f)
            {
                enPared = true;
                normalPared = contacto.normal;
                return;
            }
        }

        enPared = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        enPared = false;
    }

    public void SetCheckpoint(Vector3 nuevaPosicion)
    {
        checkpointPosition = nuevaPosicion;
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        transform.position = checkpointPosition;
    }
}