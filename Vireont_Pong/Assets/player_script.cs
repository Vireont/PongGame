using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_script : MonoBehaviour
{
    private Rigidbody2D racket_rb;
    private Vector3 inputs = Vector3.zero;
    [SerializeField]
    private float speed = 3;
    private int lives = 3;
    [SerializeField]
    private List<GameObject> Lives_pict = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        racket_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        #region Movement
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        inputs = Vector2.zero;
        inputs.y = Input.GetAxis("Vertical");

        if (Input.GetButton("Vertical"))
        {
            if (inputs.y != 0)
            {
                if (inputs.y > 0 && racket_rb.transform.position.y < 5)
                    racket_rb.transform.position += Vector3.up * Time.deltaTime * speed;
                else if (racket_rb.transform.position.y > -5)
                    racket_rb.transform.position -= Vector3.up * Time.deltaTime * speed;
            }
        }
#endif

#if UNITY_ANDROID

        int MousePositionY = (int)Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        if (Input.touchCount > 0)
        {
            if (MousePositionY > racket_rb.position.y)
            {
                racket_rb.transform.position += Vector3.up * Time.deltaTime * speed;
            }
            else
                racket_rb.transform.position -= Vector3.up * Time.deltaTime * speed;
        }
#endif
        #endregion Movement

    }

    // Find where the ball hits the racket to set out vector
    public float hitPoint(Vector2 ballPos)
    {
        return (ballPos.y - racket_rb.position.y);
    }

    public int MinusLife()
    {
        //Disable all hearts, enable the amount we have
        lives--;
        foreach(GameObject heart in Lives_pict)
            heart.SetActive(false);

        for (int i = 0; i < lives; i++)
            Lives_pict[i].SetActive(true);

        return lives;
    }


}
