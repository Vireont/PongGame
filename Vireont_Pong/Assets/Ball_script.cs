using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball_script : MonoBehaviour
{
    private Rigidbody2D ball_rb;
    private GameObject PlayerRacket;
    private player_script Player_script_ref;

    private GameObject UI_ref;
    private Text Score_label;
    private GameObject Lose_Panel;

    [SerializeField]
    private float speed = 10;
    private float start_speed;
    private int score = 0;
    private int best_score = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        ball_rb = GetComponent<Rigidbody2D>();
        PlayerRacket = GameObject.Find("PlayerRacket");
        UI_ref = GameObject.Find("UI");
        Score_label = UI_ref.transform.GetChild(0).GetComponent<Text>();
        Lose_Panel = UI_ref.transform.GetChild(1).gameObject;
        Player_script_ref = PlayerRacket.GetComponent<player_script>();
        start_speed = speed;

        //Load the best score from PlayerPrefs
        if (!PlayerPrefs.HasKey("BestScore")) PlayerPrefs.SetInt("BestScore", 0);
        best_score = PlayerPrefs.GetInt("BestScore");

        //Ball start velocity
        StartCoroutine(LaunchBallCoroutine());
    }

    void LaunchBall()
    {
        float direction = Random.Range(-3, 3);
        ball_rb.velocity = new Vector2(-speed, direction);
        Debug.Log("Ball velocity: " + ball_rb.velocity.ToString());
    }

    // Place ball back on start position, launch with 1.5 sec delay
    void BallRespawn()
    {
        ball_rb.velocity = Vector2.zero;
        ball_rb.position = Vector2.zero;
        StartCoroutine(LaunchBallCoroutine());
    }

    private IEnumerator LaunchBallCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        LaunchBall();
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerRacket)
        {
           float y = Player_script_ref.hitPoint(ball_rb.position);
           Vector2 dir = new Vector2(ball_rb.velocity.x, y).normalized;
           speed += start_speed / 100;
           ball_rb.velocity = dir * speed;
           //Debug.Log("Ball velocity: " + ball_rb.velocity.ToString());
           score += 1;
           Score_label.text = "Score: "+ score;
        }

        //Add some bouncyness to top and bot wall
        else if (collision.gameObject.tag == "TopWall")     
            ball_rb.velocity = new Vector2( ball_rb.velocity.x, ball_rb.velocity.y - 1f);

        else if (collision.gameObject.tag == "BotWall")
            ball_rb.velocity = new Vector2(ball_rb.velocity.x, ball_rb.velocity.y - 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "LoseBox")
        {
            //Check if player has lost
            if (Player_script_ref.MinusLife() == 0)
            {
                Lose_Panel.SetActive(true);
                if (score > best_score)
                {
                    best_score = score;
                    PlayerPrefs.SetInt("BestScore", score);
                }
            Lose_Panel.GetComponentInChildren<Text>().text = "Best Score: " + best_score;
            }

            else
            {
                BallRespawn();
            }
        }

        
    }
}
