using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Referencia al Rigidbody2D del personaje")]
    [SerializeField] private Rigidbody2D rb;

    [Tooltip("Referncia al Animator del personaje")]
    [SerializeField] private Animator animator;

    [Tooltip("Referencia al SpriteRenderer del personaje")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private float fallForce;
    [SerializeField] private float jumpForce;

    [SerializeField] private float speed;

    [SerializeField] public int health;

    [SerializeField] public int coinsCounter;

    // Start is called before the first frame update
    private void Start()
    {
        coinsCounter = PlayerPrefs.GetInt("coins", 0);
        coinsText.text = coinsCounter.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        // Compruebo el movimiento del personaje hacia la derecha
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            animator.SetBool("walking", true);
            spriteRenderer.flipX = false;
        }
        // Compruebo el movimiento del personaje hacia la derecha
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            animator.SetBool("walking", true);
            spriteRenderer.flipX = true;
        } 
        else
        {
            animator.SetBool("walking", false);
        }


        // Compruebo el salto del personaje
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Aumento de la velocidad de caída del personaje
        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector2.down * fallForce);
        }

        //animator.SetBool("walking", rb.velocity.x != 0);
        //spriteRenderer.flipX = rb.velocity.x < 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Coin"))
        {
            Destroy(other.collider.gameObject);
            ++coinsCounter;
            coinsText.text = coinsCounter.ToString();
            PlayerPrefs.SetInt("coins", coinsCounter);
            SceneManager.LoadScene("SecondLevel");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            --health;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
