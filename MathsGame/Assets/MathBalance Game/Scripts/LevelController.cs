using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Linq;


public class ControlGame : MonoBehaviour
{
    public GameObject Groups;
    public GameObject[] rowNumbers;
    public int currentLevel = 1;
    private Dictionary<int, int> droppedNumbers = new Dictionary<int, int>();
    private  MainMenu mainMenu;
    public int uniqueNumbers;
    public TimeHandler TimeHandler;


    void Start()
    {
        GenerateQue();

    }

    private void Update()
    {
        float bend = Mathf.Sin(Time.time * 2f);
        float rotationAngle = bend * 5f; 

        
        if (EquationManager.instance.GetLevelData(currentLevel) != null && droppedNumbers.Count == 4)
        {
            int a = droppedNumbers[0];
            int b = droppedNumbers[1];
            int c = droppedNumbers[2];
            int d = droppedNumbers[3];

            int leftHandSide = CalculateSideValue(a, b, EquationManager.instance.GetLevelData(currentLevel).operator1);
            int rightHandSide = CalculateSideValue(c, d, EquationManager.instance.GetLevelData(currentLevel).operator2);

            if (leftHandSide < rightHandSide)
            {
                rotationAngle -= 15f; 
            }
            else if (leftHandSide > rightHandSide)
            {
                rotationAngle += 15f; 
            }
        }
        else if (droppedNumbers.Count < 4)
        {
            int lhsSum = 0;
            int rhsSum = 0;

            foreach (var pair in droppedNumbers)
            {
                int index = pair.Key;
                int number = pair.Value;

                
                if (index == 0 || index == 1)
                {
                    lhsSum += number;
                }
                else if (index == 2 || index == 3)
                {
                    rhsSum += number;
                }
            }

            
            if (lhsSum < rhsSum)
            {
                rotationAngle -= 15f; 
            }
            else if (lhsSum > rhsSum)
            {
                rotationAngle += 15f; 
            }
        }

        
        Groups.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }



    void GenerateQue()
    {
        // Get level data from Manager
        EquationData levelData = EquationManager.instance.GetLevelData(currentLevel);

        if (levelData != null)
        {
            Groups.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            Groups.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "";
            Groups.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "";
            Groups.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "";
            Groups.transform.GetChild(4).GetComponent<Text>().text = levelData.operator1.ToString();
            Groups.transform.GetChild(5).GetComponent<Text>().text = "=";
            Groups.transform.GetChild(6).GetComponent<Text>().text = levelData.operator2.ToString();

            // Generate a list of unique numbers
            List<int> uniqueNumbers = new List<int>();
            for (int i = 1; i <= 20; i++)
            {
                uniqueNumbers.Add(i);
            }
            uniqueNumbers.Remove(levelData.numbers[0]);
            uniqueNumbers.Remove(levelData.numbers[1]);
            uniqueNumbers.Remove(levelData.numbers[2]);
            uniqueNumbers.Remove(levelData.numbers[3]);

            // Shuffle the list of unique numbers
            ShuffleList(uniqueNumbers);

            // Populate the rowNumbers array with unique numbers followed by equation numbers
            for (int i = 0; i < rowNumbers.Length; i++)
            {
                if (i < levelData.numbers.Length)
                {
                    rowNumbers[i].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = levelData.numbers[i].ToString();
                }
                else
                {
                    rowNumbers[i].transform.GetChild(i).GetChild(0).GetComponent<Text>().text = uniqueNumbers[i - levelData.numbers.Length].ToString();
                }
            }
        }
        else
        {
            Debug.LogWarning("No level data found for level " + currentLevel);
        }
    }

    // Function to shuffle a list
    void ShuffleList<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void AddDroppedNumber(int number, int index)
    {
        if (index >= 0 && index < 4)
        {
            // If the number already exists at another index, replace it and update the previous index
            if (droppedNumbers.ContainsValue(number))
            {
                // Find the index of the existing number
                int existingIndex = droppedNumbers.FirstOrDefault(x => x.Value == number).Key;

                // Update the existing index to 0
                droppedNumbers[existingIndex] = 0;
            }

            // Add or update the number at the specified index
            droppedNumbers[index] = number;
            Debug.Log("Added number: " + number + " at index: " + index);

            // Reset rotation of the row object at index to zero
            foreach (Transform child in rowNumbers[index].transform)
            {
                child.localRotation = Quaternion.identity;

                // Check if the child is a text element hide it
                Text textComponent = child.GetComponent<Text>();
                if (textComponent != null)
                {
                    textComponent.gameObject.SetActive(false);  // Deactivating the text component when dropped        
                }
            }

            TimeHandler.ResetTimer();  // After every drop the time is reset


            // Check the equation after adding or updating a number
            CheckEquation();
        }
        else
        {
            Debug.LogWarning("Invalid index for dropped number: " + index);
        }
    }

    // Function to check if the equation is satisfied
    public void CheckEquation()
    {
        EquationData levelData = EquationManager.instance.GetLevelData(currentLevel);

        if (levelData != null)
        {
            if (droppedNumbers.Count == 4)
            {
                int a = droppedNumbers[0];
                int b = droppedNumbers[1];
                int c = droppedNumbers[2];
                int d = droppedNumbers[3];
                Debug.Log("a= " + a + " b= " + b + " c= " + c + " d= " + d);
                int leftHandSide = CalculateSideValue(a, b, levelData.operator1);
                int rightHandSide = CalculateSideValue(c, d, levelData.operator2);
                Debug.Log("LHS : " + leftHandSide + " RHS : " + rightHandSide);


                // Check if the equation is satisfied
                bool equationSatisfied = leftHandSide == rightHandSide;
                if (equationSatisfied)
                {
                    Debug.Log("Level completed!");
                    int levelNumber = levelData.level + 1;
                    if (levelNumber <= 5)
                    {
                        SceneManager.LoadScene("Level_" + levelNumber);
                    }
                    else
                    {
                        SceneManager.LoadScene("MainMenu");
                    }
                }
                else
                {
                    Debug.Log("Equation not satisfied. Try replacing numbers.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No level data found for level " + currentLevel);
        }
    }

    private int CalculateSideValue(int operand1, int operand2, char operation)
    {
        int sideValue = 0;
        switch (operation)
        {
            case '+':
                sideValue = operand1 + operand2;
                break;
            case '-':
                sideValue = operand1 - operand2;
                break;
            case '*':
                sideValue = operand1 * operand2;
                break;
            case '/':
                if (operand2 == 0)
                {
                    sideValue = 0;
                }
                else
                {
                    sideValue = operand1 / operand2;
                }
                break;
        }
        return sideValue;
    }

}