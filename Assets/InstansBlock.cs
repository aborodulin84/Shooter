using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstansBlock : MonoBehaviour
{

    public GameObject Circle;
    public GameObject Bubble;
    //Color color;
    public Color[] Colors; // { Color.blue, Color.cyan, Color.green, Color.red, Color.magenta, Color.yellow };
    //List<Vector3> Elem=new List<Vector3>();
   // List<GameObject> Accord=new List<GameObject>();


    public void CueBall()
    {

        Bubble.GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, Colors.Length-1)];
        Instantiate(Bubble, new Vector3(0, -4, 1), Quaternion.identity);
    }

    void BildPole()
    {
        for (int i = -10; i <= 10; i++)
        {
            for (int j = 0; j <= 6; j++)
            {
                Circle.GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, Colors.Length)];
                Instantiate(Circle, new Vector3(i, j, 1), Quaternion.identity);
            }
        }
    }

  /*  void Func(GameObject a)
    {
        GameObject[] All = GameObject.FindGameObjectsWithTag("Circle");
        List<GameObject> mach = new List<GameObject>();
        mach.Add(a);
        Color col = a.gameObject.GetComponent<SpriteRenderer>().color;
        foreach (GameObject g in All)
        {
            float dx = transform.position.x - g.transform.position.x;
            float dy = transform.position.y - g.transform.position.y;
            if (Mathf.Abs(dx) < 1 && Mathf.Abs(dy) < 1)
            {
                if (g.gameObject.GetComponent<SpriteRenderer>().color == col)
                    mach.Add(g);
            }
        }
        if (mach.Count >= 3) foreach (GameObject o in mach) { Destroy(o); }
    } */

   
    
    void Start()
    {
        CueBall();
        BildPole();

    }

    // Update is called once per frame
    void Update()
    {
        /* GameObject[] All = GameObject.FindGameObjectsWithTag("Circle");
         Debug.Log("Найдены");
         for (int i=0; i<All.Length;i++)
         {
             Accord.Add(All[i]);
             Debug.Log("Объект {0} добавлен",All[i]);
            color=All[i].GetComponent<SpriteRenderer>().color;
             GetNeighbor(All[i]);
             foreach (GameObject A in All)
             {
                 foreach (Vector3 e in Elem)
                 {
                     if (A.transform.position == e)
                     {
                         if (A.GetComponent<SpriteRenderer>().color == color) Accord.Add(A);
                     }
                 }
             }
             if (Accord.Count >= 3) { foreach (GameObject D in Accord) { Destroy(D); } }
             Accord.Clear();
         }*/
    }
}