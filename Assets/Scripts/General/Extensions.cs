using UnityEngine;


public static class Extensions
{
    public static T Random<T>(this T[] array)
    {
        if (array == null || array.Length == 0) return default(T);

        int index = UnityEngine.Random.Range(0,array.Length);
        return array[index];
    }
}

public static class Utility
{
    public static GameObject CreateBoxColliderOutline(BoxCollider box,Color color, Material outlineMat)
    {
        float lineWidth = 0.02f;

        if (box == null || outlineMat == null)
            return null;

        GameObject outline = new GameObject($"{box.name}_Outline");
        outline.transform.SetParent(box.transform, false);

        Vector3 half = box.size * 0.5f;
        Vector3 centre = box.center;

        Vector3[] corners =
        {
        centre + new Vector3(-half.x, -half.y, -half.z),
        centre + new Vector3( half.x, -half.y, -half.z),
        centre + new Vector3( half.x,  half.y, -half.z),
        centre + new Vector3(-half.x,  half.y, -half.z),
        centre + new Vector3(-half.x, -half.y,  half.z),
        centre + new Vector3( half.x, -half.y,  half.z),
        centre + new Vector3( half.x,  half.y,  half.z),
        centre + new Vector3(-half.x,  half.y,  half.z)  
        };

            int[,] edges =
            {
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 },
            { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 },
            { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }
        };

        if (color == default)
            color = Color.green;

        for (int i = 0; i < 12; i++)
        {
            GameObject edge = new GameObject($"Edge_{i}");
            edge.transform.SetParent(outline.transform, false);

            LineRenderer line = edge.AddComponent<LineRenderer>();
            line.useWorldSpace = false;

            line.positionCount = 2;

            line.SetPosition(0, corners[edges[i, 0]]);
            line.SetPosition(1, corners[edges[i, 1]]);

            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.startColor = color;
            line.endColor = color;
            line.sharedMaterial = outlineMat;
        }
        return outline;
    }
}


