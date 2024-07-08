using UnityEngine;

public class EquationManager : MonoBehaviour
{
    public static EquationManager instance; 

    
    public EquationData[] levels;

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public EquationData GetLevelData(int level)
    {
        if (level >= 0 && level < levels.Length)
        {
            return levels[level];
        }
        else
        {
            Debug.LogWarning("Level index out of range.");
            return null;
        }
    }
}

[System.Serializable]
public class EquationData
{
    public int level;
    public int[] numbers; 
    public char operator1; 
    public char operator2; 
}
