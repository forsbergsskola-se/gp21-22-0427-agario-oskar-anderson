using System.Numerics;
using AgarioServer.Data;

namespace AgarioServer;

public class FoodControl
{
    private HashSet<Vector2> foodPositions = new();
    private Random random = new();
    private readonly int maxFoodCount;

    public Vector2[] CurrentFoodPositions => foodPositions.ToArray();


    public FoodControl(int maxFood)
    {
        
    }

    
    
    public bool TrySpawnFood(out NetworkPackage<Vector2[]>? foodPackage ,int amount = 1)
    {
        if (foodPositions.Count > maxFoodCount)
        {
            foodPackage = null;
            return false;
        }
        
        Vector2[] positions = new Vector2[amount];
        lock (foodPositions)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2 position = new(random.NextSingle() * 100 - 50, random.NextSingle() * 100 - 50);
                foodPositions.Add(position);
                positions[i] = position;
            }
        }

        foodPackage = CreateFoodPackage(positions);
        return true;
    }

    
    
    private NetworkPackage<Vector2[]> GetCurrentFoodPositionsPackage()
    {
        return CreateFoodPackage(CurrentFoodPositions);
    }
    
    
    
    private NetworkPackage<Vector2[]> CreateFoodPackage(Vector2[] positions)
    {
        return new NetworkPackage<Vector2[]>(PackageType.FoodSpawning, positions);
    }
    
    

    public void RemoveFood(Vector2[] positions)
    {
        foreach (var position in positions)
        {
            foodPositions.Remove(position);
        }
    }
}