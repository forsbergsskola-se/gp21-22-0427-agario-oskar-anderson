using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodColoring : MonoBehaviour
{
    private static Color[] colors =
    {
        Color.blue,
        Color.red,
        Color.black,
        Color.cyan,
        Color.green,
        Color.magenta,
        Color.yellow
    };

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform transform;
        

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(transform.position.GetHashCode());
        spriteRenderer.color = colors[Random.Range(0, colors.Length)];
    }
}
