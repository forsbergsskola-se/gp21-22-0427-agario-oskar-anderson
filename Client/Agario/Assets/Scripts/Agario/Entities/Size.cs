using UnityEngine;



namespace Agario.Entities
{
    public class Size : MonoBehaviour
    {
        [SerializeField] private Transform entityTransform;

        public void SetSizeFromScore(float score)
        {
            float scale = GetRadiusFromArea(score) + 1;
            entityTransform.localScale = new Vector3(scale, scale, scale);
        }

        public static float GetRadiusFromArea(float score)
        {
            return Mathf.Sqrt(score / Mathf.PI) / 2;
        }
        
        public static float GetTrueRadius(float size)
        {
            var mathRadius = GetRadiusFromArea(size);
            var trueRadius = (mathRadius + 1f) / 2f;
            return trueRadius;
        }
    }
}
