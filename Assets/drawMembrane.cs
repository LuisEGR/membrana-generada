using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delaunay;
using Delaunay.Geo;
using UnityEditor;
using Gizmos = Popcron.Gizmos;


//[ExecuteAlways]
public class drawMembrane : MonoBehaviour {
    public float scaleSphere = 1;
    public Material material = null;
    public Camera mainCam = null;

    private List<Vector2> m_points;
    private float m_mapWidth = 100;
    private float m_mapHeight = 50;
    private List<LineSegment> m_edges = null;
    private List<LineSegment> m_spanningTree;
    private List<LineSegment> m_delaunayTriangulation;
    List<uint> colors = new List<uint>();
    int totalPasos = 0;
    int pasoActual = 0;
    int pasoActualDelaunay = 0;

    // Use this for initialization
    void Start() {
       
        m_points = new List<Vector2>();

        var r = 10;
        for (int i = 0; i < r; i++)
        {
            createCircle(i);
        }
        Delaunay.Voronoi v = new Delaunay.Voronoi(m_points, colors, new Rect(0, 0, m_mapWidth, m_mapHeight));
        m_edges = v.VoronoiDiagram();

        //m_spanningTree = v.SpanningTree(KruskalType.MINIMUM);
        m_delaunayTriangulation = v.DelaunayTriangulation();
        totalPasos = m_points.Count;

        InvokeRepeating("UpdateSteps", 1f, 0.01f);
    }

    void createCircle(float r)
    {

        //d1 = 15;% dimension x de la malla
        //d2 = 2;% dimensi ? n y
        //d3 = 2;% dimensi ? n z
        //k = 1;
        //        for z = 1:d3
        //           for y = 1:d2
        //      for x = 1:d1
        //         R(k,:) =[x, y, z];% posici ? n inicial de las part ? culas
        //         k = k + 1;
        //        end % x
        //   end % y
        //end % z
        int d1 = 15;
        int d2 = 2;
        int d3 = 2;
        for(int z = 0; z <= d3; z++)
        {
            for(int y = 0; y <= d2; y++)
            {
                for(int x = 0; x <= d1; x++)
                {
                    m_points.Add(new Vector2(x, y));
                }
            }
        }




        //var inc = Mathf.PI / (6 + r * 2);
        //for (float i = 0; i <= 2 * Mathf.PI; i += inc)
        //{
        //    //Debug.LogWarning(i);
        //    var x = r * Mathf.Cos(i);
        //    var y = r * Mathf.Sin(i);
        //    var z = 0;
        //    colors.Add(0);
        //    m_points.Add(new Vector2(x, y));                
        //}
    }

    
    void drawLines()
    {
        if (m_delaunayTriangulation != null)
        {
            for (int i = 0; i < m_delaunayTriangulation.Count; i++)
            {

                //Vector2 left = (Vector2)m_delaunayTriangulation[i].p0;
                // Vector2 right = (Vector2)m_delaunayTriangulation[i].p1;
                var t = m_delaunayTriangulation[i];
                var vp0 = (Vector2)t.p0;
                var vp1 = (Vector2)t.p1;
                var left = new Vector3(vp0.x, vp0.y, 0);
                var right = new Vector3(vp1.x, vp1.y, 0);

                Gizmos.Line(left, right, Color.green);

            }
        }
    }


    private void UpdateSteps()
    {
         pasoActual++;
         if(pasoActual > m_points.Count)
        {
            pasoActualDelaunay++;
        }
    }

    // Update is called once per frame
    void Update() {


        if(pasoActual <= m_points.Count)
        {
            if (m_points != null)
            {
                Debug.Log("TOTAL:" + m_points.Count);
                Debug.Log("Exte:" + (m_points.Count - 53));



                for (int i = 0; i < pasoActual; i++)
                {
                    //Gizmos.color = Color.blue;
                    var color = Color.blue;
                    if (i < m_points.Count - 48)
                    {
                        color = Color.magenta;
                    }
                    Vector3 pos = new Vector3(m_points[i].x, m_points[i].y, 0);
                    Gizmos.Sphere(pos, 0.15f, color);
                    //Gizmos.Circle(pos, 0.15f, mainCam, color);
                    //DrawString("("+i+")", pos, Color.white);


                }
            }
        } else
        {
            for (int i = 0; i < m_points.Count; i++)
            {
                //Gizmos.color = Color.blue;
                var color = Color.blue;
                if (i < m_points.Count - 48)
                {
                    color = Color.magenta;
                }
                Vector3 pos = new Vector3(m_points[i].x, m_points[i].y, 0);
                Gizmos.Sphere(pos, 0.15f, color);
            }
        }
        

        if(pasoActualDelaunay <= m_delaunayTriangulation.Count)
        {

            if (m_delaunayTriangulation != null)
            {
                for (int i = 0; i < pasoActualDelaunay; i++)
                {
                    var t = m_delaunayTriangulation[i];
                    var vp0 = (Vector2)t.p0;
                    var vp1 = (Vector2)t.p1;
                    var left = new Vector3(vp0.x, vp0.y, 0);
                    var right = new Vector3(vp1.x, vp1.y, 0);

                    Gizmos.Line(left, right, Color.black);
                }
            }
        }
        //gizmos.color = color.yellow;
        

    }



    void OnDrawGizmos()
    {
        //Gizmos.color = Color.magenta;
        //if (m_points != null)
        //{
        //    //Debug.Log("TOTAL:"+ m_points.Count);
        //    //Debug.Log("Exte:" + (m_points.Count - 53));
        //    for (int i = 0; i < m_points.Count; i++)
        //    {
        //        //Gizmos.color = Color.blue;
        //        //if (i < m_points.Count - 48) {
        //        //    Gizmos.color = Color.magenta;
        //        //}
        //        Vector3 pos = new Vector3(m_points[i].x, m_points[i].y, 0);
        //        Gizmos.Sphere(pos, 0.15f);
        //        //DrawString("("+i+")", pos, Color.white);


        //    }
        //}

      
        ////Gizmos.color = Color.yellow;
        //if (m_delaunayTriangulation != null)
        //{
        //    for (int i = 0; i < m_delaunayTriangulation.Count; i++)
        //    {
        //        Vector2 left = (Vector2)m_delaunayTriangulation[i].p0;
        //        Vector2 right = (Vector2)m_delaunayTriangulation[i].p1;
        //        //Gizmos.DrawLine((Vector3)left, (Vector3)right);
        //        Gizmos.Line((Vector3)left, (Vector3)right);
        //    }
        //}

        //if (m_spanningTree != null)
        //{
        //    Gizmos.color = Color.green;
        //    for (int i = 0; i < m_spanningTree.Count; i++)
        //    {
        //        LineSegment seg = m_spanningTree[i];
        //        Vector2 left = (Vector2)seg.p0;
        //        Vector2 right = (Vector2)seg.p1;
        //        Gizmos.DrawLine((Vector3)left, (Vector3)right);
        //    }
        //}

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(new Vector2(0, 0), new Vector2(0, m_mapHeight));
        //Gizmos.DrawLine(new Vector2(0, 0), new Vector2(m_mapWidth, 0));
        //Gizmos.DrawLine(new Vector2(m_mapWidth, 0), new Vector2(m_mapWidth, m_mapHeight));
        //Gizmos.DrawLine(new Vector2(0, m_mapHeight), new Vector2(m_mapWidth, m_mapHeight));
    }

    /*
    public static void DrawString(string text, Vector3 worldPos, Color? textColor = null, Color? backColor = null)
    {
        Handles.BeginGUI();
        var restoreTextColor = GUI.color;
        var restoreBackColor = GUI.backgroundColor;

        GUI.color = textColor ?? Color.white;
        GUI.backgroundColor = backColor ?? Color.black;

        var view = SceneView.currentDrawingSceneView;
        if (view != null && view.camera != null)
        {
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                GUI.color = restoreTextColor;
                Handles.EndGUI();
                return;
            }
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            var r = new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y);
            GUI.Box(r, text, EditorStyles.numberField);
            GUI.Label(r, text);
            GUI.color = restoreTextColor;
            GUI.backgroundColor = restoreBackColor;
        }
        Handles.EndGUI();
    }
    */
}
