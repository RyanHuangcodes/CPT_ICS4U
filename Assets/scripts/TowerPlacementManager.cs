using UnityEngine;
//gpt
public class TowerPlacementManager : MonoBehaviour
{
    public static TowerPlacementManager Instance;

    public bool IsBasePlaced { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetBasePlaced(bool value)
    {
        IsBasePlaced = value;
    }
}
