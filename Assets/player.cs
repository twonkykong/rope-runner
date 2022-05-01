using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class player : MonoBehaviour
{
    public GameObject spine, death, cam, ragdoll, body, obsttrig, retryMenu, asd;
    public GameObject[] menu;
    public Text score, best, coins, timerText;
    public float velocityY, speed = 0.025f, timer, zpos;
    public bool play, dead, starPower, continueShow, extraLife;
    public int coinsGoal, coinsCollected, deaths;
    float start1;

    private void Start()
    {
        MobileAds.Initialize(initStatus => { });

        Color color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1);
        body.GetComponent<SkinnedMeshRenderer>().material.color = color;
    }

    private void FixedUpdate()
    {
        death.transform.position = new Vector3(transform.position.x, -2, transform.position.z);
        obsttrig.transform.position = new Vector3(0, 1, transform.position.z - 3.57f);
        if (play)
        {
            foreach (GameObject obj in menu) obj.SetActive(false);
            if (!dead)
            {
                if (starPower)
                {
                    if (start1 == 0)
                    {
                        GetComponent<Rigidbody>().isKinematic = true;
                        Color color = body.GetComponent<SkinnedMeshRenderer>().material.color;
                        color.a -= 0.6f; 
                        body.GetComponent<SkinnedMeshRenderer>().material.color = color;
                        speed += 0.005f;
                    }
                    start1 += 0.1f;
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("coin")) obj.SetActive(false);
                    if (start1 >= 50)
                    {
                        speed -= 0.005f;
                        start1 = 0;
                        gameObject.AddComponent<Rigidbody>();
                        GetComponent<Rigidbody>().isKinematic = false;
                        Color color = body.GetComponent<SkinnedMeshRenderer>().material.color;
                        color.a = 1;
                        body.GetComponent<SkinnedMeshRenderer>().material.color = color;
                        starPower = false;
                    }
                }
                coins.text = coinsCollected + "/" + coinsGoal;
                speed += 0.000001f;
                obsttrig.SetActive(true);
                score.text = "score: " + Math.Round(transform.position.z) + "m";
                if (GetComponent<Rigidbody>().velocity.y > -2)
                {
                    if (coinsCollected >= coinsGoal)
                    {
                        coinsCollected = 0;
                        coinsGoal = UnityEngine.Random.Range(5, 15);
                        starPower = true;
                    }
                    transform.position = new Vector3(0, transform.position.y, transform.position.z);
                    if (!starPower) GetComponent<Rigidbody>().angularVelocity *= 1.1f;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -Input.acceleration.x * 60), 0.1f);
                    spine.transform.rotation = Quaternion.Slerp(spine.transform.rotation, Quaternion.Euler(0, 0, -Input.acceleration.x * 100), 0.1f);
                    transform.Translate(0, 0, speed);
                    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z - 3);
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            spine.transform.rotation = Quaternion.Euler(Vector3.zero);
            if (transform.position.z != 0)
            {
                if (!extraLife) transform.position = new Vector3(0, -5, transform.position.z);
                /*if (deaths == 4)
                {
                    timer -= 0.1f;
                    timerText.text = "" + Mathf.Round(timer / 5);
                    if (timer == 24.9f)
                    {
                        continueShow = true;
                        retryMenu.SetActive(true);
                        zpos = transform.position.z;
                    }
                    else if (timer <= 0)
                    {
                        continueShow = false;
                        deaths = 0;
                        timer = 25f;
                        retryMenu.SetActive(false); 
                    }
                }*/
            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Animator>().SetBool("dead", false);
            if (continueShow == false)
            {
                coins.gameObject.SetActive(false);
                foreach (GameObject obj in menu) obj.SetActive(true);
                obsttrig.SetActive(false);
                best.gameObject.SetActive(true);
                best.text = "best: " + PlayerPrefs.GetInt("best") + "m";

                if (Input.GetMouseButtonDown(0))
                {
                    if (transform.position.z == 0 || extraLife == true) GetComponent<Animator>().enabled = true;
                    death.GetComponent<deathTrigger>().banner.SetActive(false);
                    
                    best.gameObject.SetActive(false);
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                    if (transform.position.z != 0)
                    {
                        if (!extraLife)
                        {
                            obsttrig.GetComponent<obstaclesScript>().Generate();
                            Color color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1);
                            body.GetComponent<SkinnedMeshRenderer>().material.color = color;
                        } 
                    }
                    if (!extraLife)
                    {
                        transform.position = new Vector3(0, 0.5f, 0); coins.gameObject.SetActive(true);
                        coinsGoal = UnityEngine.Random.Range(5, 15);
                        coinsCollected = 0;
                        speed = 0.025f;
                        asd.transform.position = new Vector3(0, 1, 0);
                    }
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    play = true;
                    dead = false;
                    extraLife = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "obstacle")
        {
            dead = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Animator>().SetBool("dead", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "coin")
        {
            coins.gameObject.GetComponent<Animation>().Play();
            other.GetComponent<Animation>().Play();
            other.GetComponent<Collider>().enabled = false;
            coinsCollected += 1;
        }
    }

    public void noThanks()
    {
        continueShow = false;
        deaths = 0;
        timer = 0;
        retryMenu.SetActive(false);
    }

    public void watchAd()
    {
        RewardedAd ad = new RewardedAd("ca-app-pub-8300493683714143/8365862994");
        AdRequest req = new AdRequest.Builder().Build();
        ad.LoadAd(req);
        ad.Show();

        ad.OnUserEarnedReward += HandleUserEarnedReward;
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Debug.Log("yes here's your extra");
        noThanks();
        deaths = -1;
        extraLife = true;
        asd.transform.position = new Vector3(0, 1, zpos);
        transform.position = new Vector3(0, 0.5f, zpos);
        GetComponent<Animator>().enabled = false;
    }
}
