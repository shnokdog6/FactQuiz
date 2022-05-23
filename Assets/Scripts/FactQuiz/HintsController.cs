using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HintsController : MonoBehaviour
{
    [SerializeField] private AnswerButtons _AnswerButtons;
    [SerializeField] private TextMeshProUGUI _CoinBalanceText;
    [SerializeField] private Button[] _buttons;
    private NoRepeatingRandom _random;

    private int _coinBalance = 100;
    private bool _doubleChance = false;

    void Start()
    {
        _CoinBalanceText.text = _coinBalance.ToString();
        _random = new NoRepeatingRandom();
    }

    public bool GetDoubleChance()
    {
        if (_doubleChance)
        {
            _doubleChance = false;
            return true;
        }
        return _doubleChance;
    }

    public void DoubleChanceHint(Button btn)
    {
        if (!_doubleChance && checkUsable(5))
        {
            _coinBalance -= 5;
            _CoinBalanceText.text = _coinBalance.ToString();
            _doubleChance = true;
            btn.interactable = false;
        }
    }
    public void FiftyFiftyHint(Button btn)
    {
        if (checkUsable(10))
        {
            _coinBalance -= 10;
            _CoinBalanceText.text = _coinBalance.ToString();
            btn.interactable = false;

            Question question = QuestionLoader.GetCurrectQuestion();
            for (int i = 0; i < 2;)
            {
                int randomButton = _random.Range(0, _AnswerButtons.Length);
                var text = _AnswerButtons[randomButton].GetComponentInChildren<TextMeshProUGUI>().text;

                if (!text.Equals(question.Answer))
                {
                    _AnswerButtons[randomButton].GetComponent<Animator>().Play("Hide");
                    StartCoroutine(DoActionAfterDelay(0.3f, () => _AnswerButtons[randomButton].gameObject.SetActive(false)));
                    ++i;
                }
            }
            _random.Reset();
        }
    }
    public void MinusOneHint(Button btn)
    {
        if (checkUsable(5))
        {
            _coinBalance -= 5;
            _CoinBalanceText.text = _coinBalance.ToString();
            btn.interactable = false;

            Question question = QuestionLoader.GetCurrectQuestion();
            while (true)
            {
                int randomButton = _random.Range(0, _AnswerButtons.Length);
                var text = _AnswerButtons[randomButton].GetComponentInChildren<TextMeshProUGUI>().text;

                if (!text.Equals(question.Answer) && _AnswerButtons[randomButton].gameObject.activeSelf)
                {
                    _AnswerButtons[randomButton].GetComponent<Animator>().Play("Hide");
                    StartCoroutine(DoActionAfterDelay(0.3f, () => _AnswerButtons[randomButton].gameObject.SetActive(false)));
                    _random.Reset();
                    return;
                }
            }
        }
    }
    private bool checkUsable(int cost)
    {
        return (_coinBalance - cost >= 0);
    }

    public void ResetButtons()
    {
        foreach (var iter in _buttons)
            iter.interactable = true;
    }
    private IEnumerator DoActionAfterDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
