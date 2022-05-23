public class Question
{
    public string Text { get; private set; }
    public string Answer { get; private set; }
    public string Image { get; private set; }

    public Question(string text, string answer, string image)
    {
        Text = text;
        Answer = answer;
        Image = image;
    }
}
