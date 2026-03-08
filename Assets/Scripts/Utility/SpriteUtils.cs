using UnityEngine;

public static class SpriteUtils
{
    public static void SetFlipX(this Transform target, bool flipX)
    {
        Vector3 scale = target.localScale;
        float baseScale = Mathf.Abs(scale.x); 
        scale.x = flipX ? -baseScale : baseScale;
        target.localScale = scale;
    }
}