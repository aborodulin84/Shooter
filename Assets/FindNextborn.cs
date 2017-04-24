using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNextborn : MonoBehaviour {
    List<Vector3> Elem=new List<Vector3>();
   
    

    void GetNeighbor(GameObject O)
    {
        Vector3 P = O.transform.position;
        Vector3[] N = new Vector3[4];
        N[0] = P; N[0].x++; Elem.Add(N[0]);
        N[1] = P; N[1].x--; Elem.Add(N[1]);
        N[2] = P; N[2].y++; Elem.Add(N[2]);
        N[3] = P; N[3].y--; Elem.Add(N[3]);
    }

    void FindNeighborn(GameObject A)
    {
        GameObject[] All = GameObject.FindGameObjectsWithTag("Circle");
        Color[] Colors = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InstansBlock>().Colors;
        GetNeighbor(A);
        foreach(GameObject O in All)
        {
            foreach(Vector3 V in Elem)
            {
                if(O.transform.position==V)
                {
                    if (O.GetComponent<SpriteRenderer>().color == A.GetComponent<SpriteRenderer>().color)
                    {
                        O.GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, Colors.Length)];
                        
                    }
                }
            }
        }
    }

   

    void Start () {
        FindNeighborn(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
