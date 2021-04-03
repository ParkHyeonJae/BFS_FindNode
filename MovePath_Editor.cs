using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(MovePath))]
public class MovePath_Editor : Editor
{
    MovePath movePath;

    GUIStyle stringStyle = new GUIStyle();
    GUIStyle stringStyle2 = new GUIStyle();
    GUIStyle stringStyle3 = new GUIStyle();

    float buttonXOffset = 5.0f;

    private void OnEnable()
    {
        movePath = (MovePath)target;

        stringStyle.normal.textColor = Color.green;
        stringStyle.fontStyle = FontStyle.Bold;
        stringStyle.fontSize = 30;
        stringStyle.alignment = TextAnchor.MiddleCenter;

        stringStyle2.normal.textColor = Color.red;
        stringStyle2.fontStyle = FontStyle.Bold;
        stringStyle2.fontSize = 10;
        stringStyle2.alignment = TextAnchor.MiddleCenter;


        stringStyle3.fontSize = 10;
        stringStyle3.alignment = TextAnchor.MiddleCenter;
        attachCnt = 0;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }

    Node[] selectedNode = new Node[2];
    int attachCnt = 0;

    int nodeInfo = 0;

    private void OnSceneGUI()
    {
        movePath = (MovePath)target;

        var buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.fontSize = 15;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        Vector3[] graphVertexPosition = new Vector3[movePath.movePaths.Count + 1];
        for (int i = 0; i < movePath.movePaths.Count; i++)
        {
            if (movePath.ShowNodeHandles)
            {
                buttonXOffset = Handles.ScaleValueHandle(buttonXOffset, movePath.movePaths[i][0].position, Quaternion.identity, buttonXOffset, Handles.ArrowHandleCap, 0.5f);
                movePath.movePaths[i][0].position = Handles.PositionHandle(movePath.movePaths[i][0].position, Quaternion.identity);
            }
            Vector3 position = movePath.movePaths[i][0].position;
            Handles.Label(position, i.ToString(), stringStyle);

            Vector3[] graphEdgePosition = new Vector3[movePath.movePaths[i].Count + 1];

            if (movePath.ShowNodeButtons)
            {
                Handles.BeginGUI();

                GUI.backgroundColor = new Color(0.3f, 0.5f, 2);
                Rect buttonRect = new Rect(HandleUtility.WorldToGUIPoint(position).x + buttonXOffset, HandleUtility.WorldToGUIPoint(position).y - 65, 100, 20);
                if (GUI.Button(buttonRect, "Attach", buttonStyle))
                {
                    selectedNode[attachCnt] = new Node(i, position);
                    attachCnt++;

                    if (attachCnt == 2)
                    {
                        movePath.AttachEdge(selectedNode[0], selectedNode[1]);
                        attachCnt = 0;
                    }
                }

                GUI.backgroundColor = Color.red;
                Rect buttonRect2 = new Rect(HandleUtility.WorldToGUIPoint(position).x + buttonXOffset, HandleUtility.WorldToGUIPoint(position).y - 95, 100, 20);
                if (GUI.Button(buttonRect2, "AddVertex", buttonStyle))
                {
                    movePath.AddVertex(position);
                }
                Handles.EndGUI();
            }

            //Handles.color = new Color(2, 0.5f, 0.2f, 5);
            Handles.color = Color.white;
            for (int j = 1; j < movePath.movePaths[i].Count; j++)
            {
                Handles.DrawAAPolyLine(10, movePath.movePaths[i][0].position, movePath.movePaths[i][j].position);
                //Handles.DrawLine(movePath.movePaths[i][0].position, movePath.movePaths[i][j].position);
            }
        }

        for (int i = 0; i < movePath.points.Count - 1; i++)
        {
            Handles.color = new Color(2, 0.5f, 0.2f, 5);
            Handles.DrawAAPolyLine(10, movePath.points[i].position, movePath.points[i + 1].position);
        }

        GUI.backgroundColor = Color.black;
        Handles.BeginGUI();

        GUILayout.BeginVertical();

        GUILayout.BeginArea(new Rect(20, 20, 60, 500));

        //GUILayout.Label("Test");

        EditorGUILayout.LabelField("StartNode", stringStyle2);
        movePath.start = EditorGUILayout.IntField(movePath.start, GUILayout.MinHeight(60));

        EditorGUILayout.LabelField("DestNode", stringStyle2);
        movePath.destination = EditorGUILayout.IntField(movePath.destination, GUILayout.MinHeight(60));

        if (GUILayout.Button("Search", GUILayout.MinHeight(50)))
        {

            movePath.StartBFSSearch(null);
        }
        
        EditorGUILayout.LabelField("PrintNode", stringStyle2);
        nodeInfo = EditorGUILayout.IntField(nodeInfo, GUILayout.MinHeight(60));
        if (GUILayout.Button("NodeInfo", GUILayout.MinHeight(50)))
        {
            movePath.PrintNodeByIndex(nodeInfo);
        }

        GUILayout.EndArea();

        GUILayout.EndVertical();

        Handles.EndGUI();

    }

    
}
