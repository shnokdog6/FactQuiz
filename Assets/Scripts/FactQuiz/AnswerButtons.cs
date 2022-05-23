using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnswerButtons : MonoBehaviour, IEnumerable<Button>
{
    private List<Button> _buttons;
    public Button this[int index]
    {
        get => _buttons[index];
        private set => _buttons[index] = value;
    }
    public int Length
    { 
        get => _buttons.Count;  
        private set => Length = value;
    }
    void Awake()
    {
        _buttons = new List<Button>(transform.childCount);
        for (int i = 0; i < transform.childCount; ++i)
            _buttons.Add(transform.GetChild(i).GetComponent<Button>());
    }
    public IEnumerator<Button> GetEnumerator()
    {
        return _buttons.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

}
