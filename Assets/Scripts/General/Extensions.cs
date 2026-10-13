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
