using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace FactQuiz
{
    public class SceneContoller : MonoBehaviour
    {
        [Header("Текстовые поля")]
        [SerializeField] private TextMeshProUGUI _QuestionText;
        [SerializeField] private TextMeshProUGUI _ScoreText;
        [SerializeField] private TextMeshProUGUI _QuestionNumber;

        [Header("Картинки")]
        [SerializeField] private Image _QuestionImage;

        [Header("Другие объекты")]
        [SerializeField] private Timer _Timer;
        [SerializeField] private GameObject _Blocker;
        [SerializeField] private GameObject _PauseMenu;
        [SerializeField] private AnswerButtons _AnswerButtons;
        [SerializeField] private Animator _QuestionLayoutAnimator;
        [SerializeField] private GameObject _QuestionEndTitle;
        [SerializeField] private HintsController _hintsController;

        private Question _currentQuestion;
        private int _currentScore = 0;

        private Button _rightButton;
        private Button _firstButtonForDoubleChance;


        void Start()
        {
            _Timer.timeOut += () => 
            { 
                _QuestionLayoutAnimator.Play("Hide");
                StartCoroutine(DoActionAfterDelay(1.0f, ResetScene));
            };
            QuestionLoader.ResetQuestion();
            ResetScene();
        }

        public void CheckAnswer(Button btn)
        {
            _Blocker.SetActive(true);

            var answer = btn.GetComponentInChildren<TextMeshProUGUI>().text;
            var color = btn.GetComponent<Image>().color;

            if (btn == _rightButton)
            {
                btn.GetComponent<Image>().color = Color.green;
                _currentScore += Random.Range(3, 8);
            }
            else
            {
                btn.GetComponent<Image>().color = Color.red;
                if (_hintsController.GetDoubleChance())
                {
                    _Blocker.SetActive(false);
                    _firstButtonForDoubleChance = btn;
                    return;
                }
                _rightButton.GetComponent<Image>().color = Color.green;
            }

            _ScoreText.text = _currentScore.ToString();
            _QuestionLayoutAnimator.Play("Hide");

            StartCoroutine(DoActionAfterDelay(1.0f, () =>
            {
                if (_firstButtonForDoubleChance != null)
                {
                    _firstButtonForDoubleChance.GetComponent<Image>().color = color;
                    _firstButtonForDoubleChance = null;
                }
                btn.GetComponent<Image>().color = color;
                _rightButton.GetComponent<Image>().color = color;
                ResetScene();
            }));
        }

        private void UpdateQuestion()
        {
            _currentQuestion = QuestionLoader.GetNewQuestion();
            if (_currentQuestion == null)
            {
                GameEnd();
                return;
            }
            _QuestionText.text = _currentQuestion.Text;
            _QuestionImage.sprite = Resources.Load<Sprite>("Images/" + _currentQuestion.Image);
            _QuestionNumber.text = QuestionLoader.GetUsedQuestionAmount() 
                    + "/" + QuestionLoader.GetAmountOfQuestion();

            int randomButton = Random.Range(0, _AnswerButtons.Length);
            _rightButton = _AnswerButtons[randomButton];
            _rightButton.GetComponentInChildren<TextMeshProUGUI>().text = _currentQuestion.Answer;
            
            for (int i = 0; i < _AnswerButtons.Length; ++i)
            {
                if (i == randomButton) continue;
                _AnswerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = Country.getCountry(_currentQuestion.Answer);
            }

            _hintsController.ResetButtons();
        }
        private void ResetScene()
        {
            foreach (var iter in _AnswerButtons)
            {
                iter.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                iter.gameObject.SetActive(true);
            }

            UpdateQuestion();
            _QuestionLayoutAnimator.Play("Show");
            _Timer.restart();
            StartCoroutine(DoActionAfterDelay(0.1f, () => _Blocker.SetActive(false)));
        }

        private void GameEnd()
        {
            _QuestionLayoutAnimator.gameObject.SetActive(false);
            _Blocker.SetActive(true);
            _QuestionEndTitle.SetActive(true);
            StartCoroutine(DoActionAfterDelay(1.5f, Exit));
        }

        public void GameContinue()
        {
            _Timer.start();
            _Blocker.SetActive(false);
            _PauseMenu.SetActive(false);
        }
        public void GamePause()
        {
            _Timer.stop();
            _Blocker.SetActive(true);
            _PauseMenu.SetActive(true);
        }
        public void Exit()
        {
            SceneManager.LoadScene(0);
        }

        private IEnumerator DoActionAfterDelay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}