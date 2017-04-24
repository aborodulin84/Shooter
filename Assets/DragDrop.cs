using UnityEngine;
using System.Collections.Generic;

public class DragDrop : MonoBehaviour
{
    LineRenderer lineRenderer;
    LineRenderer disp1;
    LineRenderer disp2;
    public GameObject Ball;
    // GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    Vector3[] Points = null;
    Vector3 dist;
    Vector3 spawn=new Vector3(0, -4, 1);
    Vector3 Dash;
    Vector3 StopTransform;
    Color s;
   bool dispON=false;
    float Impulse;
    float posX;
    float posY;
    int i = 0;

   class Line //класс линия, возможно стоит убрать.
    {
        public float a; //угловой коэффициент
        public float b; // подъем над горизонтальной осью

       public Line (Vector3 st, Vector3 pu)
        {
            a = (pu.y - st.y) / (pu.x - st.x);
            b = st.y - a * st.x;
        }

        public float moveLine(float x)
        {
            float y = a * x + b;
            return y;
        }
    } 

    void PosInit()  //подбор координат шарика
    {
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
    }

    void CarryBall() //перенос шарика
    {
        Vector3 curPos =
                  new Vector3(Input.mousePosition.x - posX,
                  Input.mousePosition.y - posY, dist.z);

       Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
    }

    void RenderLine()
    {
        lineRenderer.SetPosition(0, transform.position);
        Debug.Log("Basic line P0");
        if (spawn.x == transform.position.x) lineRenderer.SetPosition(1, new Vector3(transform.position.x, 6, 1));
        else
        {
            lineRenderer.SetPosition(1, new Vector3((transform.position.x > 0 ? -10.5f : 10.5f), nextPoint(transform.position), 1));
            Debug.Log("Basic line P1");
            lineRenderer.SetPosition(2, new Vector3((transform.position.x < 0 ? -10.5f : 10.5f), -nextPoint(lineRenderer.GetPosition(1)), 1));
        }
    }

    float nextPoint(Vector3 end) //поиск следующей точки для отрисовки траектории
    {
        float a = (end.y + 4) / end.x;
        return (end.x < 0 ? 10 : -10) * a - 4;
    }

    void NeightborFunc(GameObject g, GameObject[] A, List<GameObject> list, Color color)
    {
        list.Add(g);
        foreach (GameObject o in A)
        {
            float dx = g.transform.position.x - o.transform.position.x;
            float dy = g.transform.position.y - o.transform.position.y;
            if (Mathf.Abs(dx) < 1 && Mathf.Abs(dy) < 1)
            {
                if (o.gameObject.GetComponent<SpriteRenderer>().color == color)
                {
                    foreach(GameObject c in list) { if (!o.Equals(c))
                        {
                            list.Add(o);
                            NeightborFunc(o, A, list, color);
                        }
                    }
                    
                }
            }
        }
    }

    void Func(GameObject a)
    {
        GameObject[] All = GameObject.FindGameObjectsWithTag("Circle");
        List<GameObject> mach = new List<GameObject>();
        Color col = a.gameObject.GetComponent<SpriteRenderer>().color;
            NeightborFunc(a,All,mach,col);
        if (mach.Count >= 3) foreach (GameObject o in mach) {
                Debug.Log(o.transform.position);
                Destroy(o); }
    }

    void Spawn(GameObject O, Vector3 V)
    {
        Instantiate(O, V, Quaternion.identity);
        Func(O);
    }

    void OnMouseDown()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        PosInit();
        lineRenderer.enabled = true;
        

    }

    void OnMouseDrag()
    {
        CarryBall();
        RenderLine();
        if (dispON==true)
        {
             disp1 = GameObject.FindGameObjectWithTag("wall1").GetComponent<LineRenderer>();
             disp2 = GameObject.FindGameObjectWithTag("wall2").GetComponent<LineRenderer>();
            disp1.enabled = true;
            disp2.enabled = true;
            disp1.SetPosition(0, lineRenderer.GetPosition(0));
            
            disp2.SetPosition(0, lineRenderer.GetPosition(0));
            
            Vector3 disp1Point1 = lineRenderer.GetPosition(1); ++disp1Point1.y;
            Debug.Log(lineRenderer.GetPosition(1)); Debug.Log(disp1Point1);
            Vector3 disp2Point1 = lineRenderer.GetPosition(1); --disp1Point1.y;
            disp1.SetPosition(1, disp1Point1);
            disp2.SetPosition(1, disp2Point1);
        }


    }
    private void OnMouseUp()
    {
        Dash = transform.position;
        Impulse = Mathf.Sqrt(Mathf.Pow((Dash.x - spawn.x), 2) + Mathf.Pow((Dash.y -spawn.y), 2));

       /* Line traectory = new Line(Start,transform.position);  //перенести в update возможно
        while(transform.position.x!=(Dash.x<0?-10:10))
        {
           transform.position=new Vector3(transform.position.x+0.01f,traectory.moveLine(transform.position.x+0.01f),1);
        }*/

        Points=new Vector3 [3];
        lineRenderer.GetPositions(Points);
       lineRenderer.enabled = false;
        if (dispON == true)
        {
            disp1.enabled = false;
            disp2.enabled = false;
        }
        
        Debug.Log(Impulse);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InstansBlock>().CueBall();
        //transform.position=Vector3.Lerp(Points[i],Points[i+1],Time.deltaTime*Impulse); }
        /*  GetComponent<Rigidbody>().isKinematic = false;
         GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * 1000, new Vector3(0, -4, 1));
     */
    }

   

    private void FixedUpdate()
    {
        if (i == 2) { Points = null; i = 0; }
        if (Points != null)
        {
            Vector3 dir= (Points[i + 1] - transform.position).normalized *Impulse*Time.deltaTime*10;
            transform.Translate(dir); // Vector3.Lerp(transform.position, Points[i+1], Vector3.Distance(Start,Dash)*Time.deltaTime);

            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "wall"|| collision.gameObject.tag=="wall1"|| collision.gameObject.tag == "wall2")
        {
            i++;
        }
        if (collision.gameObject.tag == "trig") dispON = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "trig") dispON = false;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.tag=="Circle")
        {
            if (Impulse >= 2)
            {
                StopTransform = collision.transform.position;
                Destroy(collision.gameObject);
            }
            else
            StopTransform = transform.position;
            s = this.gameObject.GetComponent<SpriteRenderer>().color;
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Ball.GetComponent<SpriteRenderer>().color = s;
        Spawn(Ball, StopTransform);
       // Func(Ball);
    }
}