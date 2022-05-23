using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
public class QuestionLoader
{
    private static XDocument _document;
    private static List<Question> _questions;
    private static NoRepeatingRandom _random;

    static QuestionLoader()
    {
        _document = XDocument.Parse(Resources.Load<TextAsset>("Data/Questions").text);
        _questions = _document.Element("Question")
            .Elements("Question")
            .Select(p => new Question(p.Element("Text").Value,
            p.Element("Answer").Value,
            p.Element("Image").Value))
            .ToArray().OfType<Question>().ToList();
        _random = new NoRepeatingRandom(_questions.Count);
    }

    public static int GetUsedQuestionAmount()
    {
        return _random.generatedNumbersCount;
    }

    public static int GetAmountOfQuestion()
    {
        return _questions.Count;
    }
    public static void ResetQuestion()
    {
        _random.Reset();
    }

    public static Question GetNewQuestion()
    {
        if (_random.generatedNumbersCount == _questions.Count)
            return null;

        int randomQuestion = _random.Range(0, _questions.Count);
        return _questions[randomQuestion];
    }

    public static Question GetCurrectQuestion()
    {
        return _questions[_random.GetGeneratedNumberList()[_random.generatedNumbersCount - 1]];
    }
}
