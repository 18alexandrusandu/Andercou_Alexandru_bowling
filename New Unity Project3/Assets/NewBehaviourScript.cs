using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;





public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update

    public class Player
    {

        public double score;
        public int rounds, nr_collision;

        public Player(int round)
        {
            rounds = round;
            nr_collision = 0;
            score = 0;


        }

    }











    public float force, angle = 0;
    public int numb_rounds = 3, current = 0;
    private Text text;
    private List<Vector3> pinPosition;
    private List<Quaternion> pinRotations;
    private Vector3 ballPosition;
    private Player PLAY1, PLAY2;
    private int turn = 0;

    void Start()
    {

        PLAY1 = new Player(numb_rounds);
        PLAY2 = new Player(numb_rounds);
        var txte = GameObject.FindGameObjectWithTag("texty");
        txte.GetComponent<Text>().text = "Player1  score:"+ PLAY1.score ;

        var pins = GameObject.FindGameObjectsWithTag("piny");

        pinPosition = new List<Vector3>();
        pinRotations = new List<Quaternion>();
        foreach (var pin in pins)
        {
            pinPosition.Add(pin.transform.position);
            pinRotations.Add(pin.transform.rotation);



        }



        ballPosition = GameObject.FindGameObjectWithTag("ball").transform.position;







    }

    public void SetBallBegin()
    {

        var ball = GameObject.FindGameObjectWithTag("ball");
        ball.transform.position = ballPosition;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


    }



    private int uis = 0;
   public float slerp(float value,float bm,float bmx,float Bm,float Bmx)
    {
        if (value <= bm)
            return Bm;
        if (value >= bmx)
            return Bmx;

        return (value - bm) * (Bmx - Bm) / (bmx - bm) + Bm;


    }








    // Update is called once per frame
    void Update()
    {
        var ball = GameObject.FindGameObjectWithTag("ball");
        var cam = GameObject.FindGameObjectWithTag("MainCamera");
        var txte = GameObject.FindGameObjectWithTag("texty");
        var txte2 = GameObject.FindGameObjectWithTag("texty2");
        var txte3 = GameObject.FindGameObjectWithTag("texty3");
        var line= GameObject.FindGameObjectWithTag("angle_line");
        var line2= GameObject.FindGameObjectWithTag("force_line");
        var act_line = line.GetComponent<LineRenderer>();
        var act_line2 = line2.GetComponent<LineRenderer>();


        act_line.SetPosition(0, new Vector3(-50*Mathf.Cos(Mathf.PI-angle * Mathf.Deg2Rad), 1,-50* Mathf.Cos(Mathf.PI-angle * Mathf.Deg2Rad)));

        print(slerp(force, 100, 9000, 5, 25));

        //act_line2.SetPosition(0, new Vector3(5, slerp(force, 100, 9000, 5, 25), -50));

        txte2.GetComponent<Text>().text = "Angle:" + angle;
        txte3.GetComponent<Text>().text = "Force:" + force;
        var current = 0;
        var pins = GameObject.FindGameObjectsWithTag("piny");


            if (Input.GetKeyUp(KeyCode.A))
            {
                angle += 5;
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                angle -= 5;
            }



            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }





            if (turn == 0)
            {
                txte.GetComponent<Text>().text = "Player1 score:" + (PLAY1.score + current);
                current = 0;
            }
            else
            {

                txte.GetComponent<Text>().text = "Player2 score:" + (PLAY2.score + current);
                current = 0;

            }

            force = force % 9000;

        if (force <= 9000 / 4)

        { act_line2.material.color = Color.yellow;


        }

        else
        if (force > 9000 / 4 && force <= 2 * 9000 / 4)
            act_line2.material.color =new Color(0.0f,0.50f,0f);
        else
        if (force > 2 * 9000 / 4 && force <= 3 * 9000 / 4)
            act_line2.material.color =new Color(1.0f, 0.20f,0.0f);
        else
        act_line2.material.color = Color.red;


            if (force < 100) force = 100;
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                ball.GetComponent<Rigidbody>().AddForce(
                    new Vector3(force * Mathf.Cos(angle * Mathf.Deg2Rad), 0,
                    force * Mathf.Sin(angle * Mathf.Deg2Rad)
                    ));

            


                // cam.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, force));


            }
            //cam.transform.position = ball.transform.position;

            if (Input.GetKeyUp(KeyCode.RightArrow))
                ball.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0), ForceMode.Impulse);


            if (Input.GetKeyUp(KeyCode.LeftArrow))
                ball.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0), ForceMode.Impulse);

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                force += 500;
                // GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, (float)0.5 ), ForceMode.Impulse);

            }





            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                //GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0), ForceMode.Impulse);
                force -= 500;
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                if (turn == 0)
                {
                    turn = 1;
                    PLAY1.score += current;
                    txte.GetComponent<Text>().text = "Player1 score:" + PLAY1.score.ToString();
                    current = 0;
                }
                else
                {
                    turn = 0;
                    PLAY1.score += current;
                    txte.GetComponent<Text>().text = "Player2 score:" + PLAY2.score.ToString();
                    current = 0;
                }






                for (int i = 0; i < pins.Length; i++)
                {
                    var pinPhisics = pins[i].GetComponent<Rigidbody>();
                    pinPhisics.velocity = Vector3.zero;
                    pinPhisics.position = pinPosition[i];
                    pinPhisics.rotation = pinRotations[i];
                    pinPhisics.velocity = Vector3.zero;
                    pinPhisics.angularVelocity = Vector3.zero;



                }
                SetBallBegin();

            }
            if (Input.GetKeyUp(KeyCode.B))
                SetBallBegin();


        }
   

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "piny")
        {

           GetComponent<AudioSource>().Play();
            if (turn == 0)
                PLAY1.score += 1;
            else
                PLAY2.score += 1;




        }
           
       

    }










}
