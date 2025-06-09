using UnityEngine;
using UnityEngine.InputSystem; // Nouveau syst�me d�input
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Caracter : MonoBehaviour
{
    public Rigidbody2D body;
    public float flipStrenght;
    public ScoreCacul sc;
    public ChancesManager ch;

    public bool birdIsAlive = true;
    public GameObject fly;

    private astronautControls controls;

    void Awake()
    {
        controls = new astronautControls();

        // Lier l'action "Jump" � une m�thode
        controls.Gameplay.Jump.performed += ctx => OnJump();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        sc = GameObject.FindGameObjectWithTag("ScoreLogic").GetComponent<ScoreCacul>();
        ch = GameObject.Find("Life Manager").GetComponent<ChancesManager>();

        fly = transform.Find("fly")?.gameObject;
        if (fly == null)
        {
            Debug.LogError("L'objet 'fly' n'a pas �t� trouv� dans 'bird'.");
        }
    }

    void Update()
    {
        float topLimit = Camera.main.orthographicSize;
        float bottomLimit = -Camera.main.orthographicSize;

        if (transform.position.y > topLimit || transform.position.y < bottomLimit)
        {
            sc.GameOver();
            birdIsAlive = false;
        }
        if(sc.playerScore == 100)
        {
            birdIsAlive = false;
            SceneManager.LoadScene("loby");

        }
    }

    public void OnJump()
    {
        if (!birdIsAlive) return;

        body.linearVelocity = new Vector2(body.linearVelocity.x, flipStrenght);
        StartCoroutine(ActivateFlyTemporarily());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AnswerOption option = other.GetComponent<AnswerOption>();
        if (option != null)
        {
            if (option.isCorrect)
            {
                sc.addScore();
            }
            else
            {
                ch.LoseLife();
                if (ch.life == 0)
                {
                    sc.GameOver();
                    birdIsAlive = false;
                }
            }
        }
    }

    IEnumerator ActivateFlyTemporarily()
    {
        fly.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        fly.SetActive(false);
    }
}
