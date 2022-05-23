using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRepeatingRandom
{
    private List<int> _generatedNumbers;
    public int generatedNumbersCount
    {
        get => _generatedNumbers.Count;
        private set => generatedNumbersCount = value;
    }

    public NoRepeatingRandom(int length = 10)
    {
        _generatedNumbers = new List<int>(length);
    }

    public int Range(int minInclusive, int maxExclusive)
    {
        while(true)
        {
            int randomNumber = Random.Range(minInclusive, maxExclusive);
            if (!_generatedNumbers.Contains(randomNumber))
            {
                _generatedNumbers.Add(randomNumber);
                return randomNumber;
            }
        }
    }
    public List<int> GetGeneratedNumberList() => _generatedNumbers;
    public void Reset() => _generatedNumbers.Clear();
}
