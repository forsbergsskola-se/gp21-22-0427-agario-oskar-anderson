using UnityEngine;



namespace Agario.Entities
{
    public class Size : MonoBehaviour
    {
        [SerializeField] private Transform entityTransform;

        public void SetSizeFromScore(float score)
        {
            float radius = Mathf.Sqrt(score / Mathf.PI) / 2;
            float scale = radius + 1;
            entityTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
