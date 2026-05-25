using UnityEngine;

public class EnemigoDiablo : MonoBehaviour
{
    public float velocidad = 2f;
    public int vida = 1;
    private Transform jugadora;

    void Start()
    {
        jugadora = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (jugadora == null) return;

        Vector2 direccion = (jugadora.position - transform.position).normalized;
        transform.position += (Vector3)(direccion * velocidad * Time.deltaTime);

        if (direccion.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Token"))
        {
            vida--;
            Destroy(col.gameObject);
            if (vida <= 0)
            {
                if (GameManager.instancia != null)
                    GameManager.instancia.AgregarToken();
                Destroy(gameObject);
            }
        }
    }
}