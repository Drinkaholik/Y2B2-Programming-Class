using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("UI/Effects/Gradient")]
[ExecuteAlways] // Ensures updates happen in Edit Mode
public class UIGradient : BaseMeshEffect
{
    public bool UseCorners = false;
    public bool UseWorldSpace = false;
    public bool MatchAspectRatio = true;
    public bool UpdateEveryFrame = false;

    // Linear
    public Gradient LinearGradient = new Gradient();
    [Range(-180f, 180f)] public float Angle = 0f;
    [Range(0f, 1f)] public float Offset = 0.5f;

    // Corners
    public Color TopLeft = Color.white;
    public Color TopRight = Color.white;
    public Color BottomLeft = Color.black;
    public Color BottomRight = Color.black;

    protected override void Start()
    {
        base.Start();
        if (graphic != null) graphic.SetVerticesDirty();
    }

    private void Update()
    {
        if (UpdateEveryFrame && Application.isPlaying)
        {
            if (graphic != null) graphic.SetVerticesDirty();
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || graphic == null) return;

        List<UIVertex> vertices = new List<UIVertex>();
        vh.GetUIVertexStream(vertices);
        if (vertices.Count == 0) return;

        Canvas canvas = graphic.canvas;
        if (canvas == null) return;

        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        Rect worldReferenceRect = canvasRectTransform.rect;
        
        Rect localRect = graphic.rectTransform.rect;
        Vector2 dir = GetDirection(Angle, UseWorldSpace ? worldReferenceRect : localRect);

        for (int i = 0; i < vertices.Count; i++)
        {
            UIVertex v = vertices[i];
            
            // Coordinate mapping
            Vector3 worldPos = transform.TransformPoint(v.position);
            Vector3 canvasLocalPos = canvasRectTransform.InverseTransformPoint(worldPos);
            
            Vector2 processPos = UseWorldSpace ? (Vector2)canvasLocalPos : (Vector2)v.position;
            Rect processRect = UseWorldSpace ? worldReferenceRect : localRect;

            if (UseCorners)
            {
                float nx = Mathf.InverseLerp(processRect.xMin, processRect.xMax, processPos.x);
                float ny = Mathf.InverseLerp(processRect.yMin, processRect.yMax, processPos.y);
                
                Color top = Color.Lerp(TopLeft, TopRight, nx);
                Color bottom = Color.Lerp(BottomLeft, BottomRight, nx);
                v.color *= Color.Lerp(bottom, top, ny);
            }
            else
            {
                float dot = Vector2.Dot(processPos, dir);
                GetRectBounds(processRect, dir, out float min, out float max);

                float t = Mathf.InverseLerp(min, max, dot);
                float adjustedT = Mathf.Clamp01(t - (Offset - 0.5f));
                v.color *= LinearGradient.Evaluate(adjustedT);
            }
            vertices[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(vertices);
    }

    private void GetRectBounds(Rect rect, Vector2 dir, out float min, out float max)
    {
        Vector2[] corners = {
            new Vector2(rect.xMin, rect.yMin),
            new Vector2(rect.xMax, rect.yMin),
            new Vector2(rect.xMin, rect.yMax),
            new Vector2(rect.xMax, rect.yMax)
        };

        min = float.MaxValue; max = float.MinValue;
        foreach (var corner in corners)
        {
            float dot = Vector2.Dot(corner, dir);
            min = Mathf.Min(min, dot);
            max = Mathf.Max(max, dot);
        }
    }

    private Vector2 GetDirection(float angle, Rect rect)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        if (MatchAspectRatio && rect.width > 0 && rect.height > 0)
        {
            dir.x *= rect.height / rect.width;
            dir = dir.normalized;
        }
        return dir;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIGradient)), CanEditMultipleObjects]
public class UIGradientEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        UIGradient script = (UIGradient)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("UseCorners"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("UseWorldSpace"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("UpdateEveryFrame"));
        
        if (!script.UseCorners)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MatchAspectRatio"));
        
        EditorGUILayout.Space();

        if (!script.UseCorners)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LinearGradient"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Angle"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Offset"));
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TopLeft"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TopRight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("BottomLeft"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("BottomRight"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif