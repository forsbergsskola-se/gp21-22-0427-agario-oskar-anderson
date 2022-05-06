using UnityEngine;



namespace Agario.Entities
{
    public class Size : MonoBehaviour
    {
        [SerializeField] private Transform entityTransform;

        public void SetSizeFromScore(float score)
        {
            float diameter = Mathf.Sqrt(score / Mathf.PI);
            float scale = diameter + 1;
            entityTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
