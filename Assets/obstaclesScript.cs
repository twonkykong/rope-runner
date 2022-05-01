using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaclesScript : MonoBehaviour
{
    public GameObject obstacle, sideplatform, rope, coin, star, balloon, street;
    public float pos;
    public Material[] balloonMat;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("obstacle")) Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("coin")) Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("rope")) Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("star")) Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("street")) Destroy(obj);

        pos = 2;
        while (pos < 30)
        {
            pos += Random.Range(5, 8);
            
            Instantiate(obstacle, new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(1f, 1.25f), pos), Quaternion.Euler(Vector3.zero));
            if (Random.Range(1, 3) == 1)
            {
                Instantiate(coin, new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(1f, 1.3f), pos + Random.Range(-4, 4)), Quaternion.Euler(Vector3.zero));
            }

            Instantiate(rope, new Vector3(0, 0.46f, 9), Quaternion.Euler(90,90,90));
            Instantiate(rope, new Vector3(0, 0.46f, 29), Quaternion.Euler(90, 90, 90));

            for (int i = 0; i < 2; i++)
            {
                Instantiate(street, new Vector3(0, -9, 20 + (23.015f * i)), Quaternion.identity);
            }

            float rand1 = Random.Range(0.5f, 2f);
            GameObject g1 = Instantiate(balloon, new Vector3(Random.Range(-3f, -1f), Random.Range(-2f, 4f), pos), Quaternion.Euler(Vector3.zero));
            g1.transform.localScale = new Vector3(rand1, rand1, rand1);
            g1.GetComponent<Renderer>().material = balloonMat[Random.Range(0, balloonMat.Length - 1)];

            float rand2 = Random.Range(0.5f, 2f);
            GameObject g2 = Instantiate(balloon, new Vector3(Random.Range(1f, 3f), Random.Range(-2f, 4f), pos), Quaternion.Euler(Vector3.zero));
            g2.transform.localScale = new Vector3(rand2, rand2, rand2);
            g2.GetComponent<Renderer>().material = balloonMat[Random.Range(0, balloonMat.Length - 1)];

            g1.name = "background.l";
            g2.name = "background.r";

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "obstacle")
        {
            if (other.name == "background.l") other.transform.position = new Vector3(Random.Range(-2f, -0.5f), Random.Range(-2f, 4f), other.transform.position.z + pos);
            else if (other.name == "background.r") other.transform.position = new Vector3(Random.Range(0.5f, 2f), Random.Range(-2f, 4f), other.transform.position.z + pos);
            else
            {
                other.transform.position = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(1f, 1.3f), other.transform.position.z + pos + Random.Range(-3, 3)); ;
                if (Random.Range(1, 3) == 1)
                {
                    Instantiate(coin, new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(1f, 1.25f), other.transform.position.z + Random.Range(-4, 4)), Quaternion.Euler(Vector3.zero));
                }
            }
        }

        if (other.tag == "star")
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "rope")
        {
            other.transform.position += transform.forward * 40;
        }

        if (other.tag == "street")
        {
            other.transform.position += transform.forward * (23.015f * 2);
        }
    }
}
