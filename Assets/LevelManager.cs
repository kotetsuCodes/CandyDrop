using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance = null;

    public GameType CandyDropGameType;

    public Transform CandyDrop;
    float nextCandyGenerationTime;

    public int Points = 0;

    public Problem CandyDropProblem;
    readonly HashSet<string> Words = new HashSet<string> { "is", "let", "dad", "mom", "like", "come", "he", "she" };
    readonly HashSet<string> Letters = new HashSet<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    readonly List<(int, int)> AdditionProblems = new List<(int, int)> { (2, 3), (3, 3) };
    readonly List<(int, int)> SubtractionProblems = new List<(int, int)> { (2, 1), (5, 4), (10, 8), (12, 7) };

    public Font DefaultFont;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        CandyDropProblem = new Problem();
        nextCandyGenerationTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Points);
        Debug.Log(CandyDropProblem.Solution);
        Debug.Log(CandyDropProblem.GetProblemString());

        // generate candy
        if (Time.time >= nextCandyGenerationTime)
        {
            var randomPositionX = Random.Range(-5, 5);

            var candyDrop = Instantiate(CandyDrop, new Vector2(transform.position.x + randomPositionX, transform.position.y), transform.rotation);
            var candyDropScript = candyDrop.GetComponent<CandyDrop>();
            candyDropScript.CandyDropAnswer = CandyDropProblem.GetRandomAnswer();

            nextCandyGenerationTime = Time.time + Random.Range(3, 6);
        }
    }

    public void AddPoints(int pointValue)
    {
        Points += pointValue;
    }

    public enum GameType
    {
        Words,
        Letters,
        Addition,
        Subtraction
    }

    public class Problem
    {
        public Problem()
        {
            switch (LevelManager.instance.CandyDropGameType)
            {
                case GameType.Words:
                    var randomWordProblem = LevelManager.instance.Words.ElementAt(Random.Range(0, LevelManager.instance.Words.Count));
                    Solution = (string)randomWordProblem;
                    Operands = (0, 0);
                    break;
                case GameType.Letters:
                    var randomLetterProblem = LevelManager.instance.Letters.ElementAt(Random.Range(0, LevelManager.instance.Letters.Count));
                    Solution = (string)randomLetterProblem;
                    Operands = (0, 0);
                    break;
                case GameType.Addition:
                    var randomAdditionProblem = LevelManager.instance.AdditionProblems.ElementAt(Random.Range(0, LevelManager.instance.AdditionProblems.Count));
                    Solution = (int)(randomAdditionProblem.Item1 + randomAdditionProblem.Item2);
                    Operands = (randomAdditionProblem.Item1, randomAdditionProblem.Item2);
                    break;
                case GameType.Subtraction:
                    var randomSubtractionProblem = LevelManager.instance.SubtractionProblems.ElementAt(Random.Range(0, LevelManager.instance.SubtractionProblems.Count));
                    Solution = (int)(randomSubtractionProblem.Item1 - randomSubtractionProblem.Item2);
                    Operands = (randomSubtractionProblem.Item1, randomSubtractionProblem.Item2);
                    break;
                default: throw new System.InvalidOperationException("AnswerType is invalid");
            }
        }

        public (int leftOperand, int rightOperand) Operands;
        public object Solution { get; set; }

        public int GetPoints(object answer)
        {
            switch (LevelManager.instance.CandyDropGameType)
            {
                case GameType.Words:
                    return (string)answer == (string)LevelManager.instance.CandyDropProblem.Solution ? ((string)Solution).Length : 0;
                case GameType.Letters:
                    return (string)answer == (string)LevelManager.instance.CandyDropProblem.Solution ? 1 : 0;
                case GameType.Addition:
                    return (int)answer == (int)LevelManager.instance.CandyDropProblem.Solution ? LevelManager.instance.CandyDropProblem.Operands.leftOperand + LevelManager.instance.CandyDropProblem.Operands.rightOperand : 0;
                case GameType.Subtraction:
                    return (int)answer == (int)LevelManager.instance.CandyDropProblem.Solution ? LevelManager.instance.CandyDropProblem.Operands.leftOperand + LevelManager.instance.CandyDropProblem.Operands.rightOperand : 0;
                default: throw new System.InvalidOperationException("Invalid GameType");
            }
        }

        public string GetProblemString()
        {
            switch (LevelManager.instance.CandyDropGameType)
            {
                case GameType.Words:
                    return string.Empty;
                case GameType.Letters:
                    return string.Empty;
                case GameType.Addition:
                    return $"{(int)LevelManager.instance.CandyDropProblem.Operands.leftOperand} + {(int)LevelManager.instance.CandyDropProblem.Operands.rightOperand} = ?";
                case GameType.Subtraction:
                    return $"{(int)LevelManager.instance.CandyDropProblem.Operands.leftOperand} - {(int)LevelManager.instance.CandyDropProblem.Operands.rightOperand} = ?";
                default: throw new System.InvalidOperationException("Invalid GameType");
            }
        }

        public object GetRandomAnswer()
        {
            switch (LevelManager.instance.CandyDropGameType)
            {
                case GameType.Words:
                    return (string)(LevelManager.instance.Words.ElementAt(Random.Range(0, LevelManager.instance.Words.Count)));
                case GameType.Letters:
                    return (string)(LevelManager.instance.Letters.ElementAt(Random.Range(0, LevelManager.instance.Letters.Count)));
                case GameType.Addition:
                    var randomAdditionProblem = LevelManager.instance.AdditionProblems.ElementAt(Random.Range(0, LevelManager.instance.AdditionProblems.Count));
                    return (int)(randomAdditionProblem.Item1 + randomAdditionProblem.Item2);
                case GameType.Subtraction:
                    var randomSubtractionProblem = LevelManager.instance.SubtractionProblems.ElementAt(Random.Range(0, LevelManager.instance.SubtractionProblems.Count));
                    return (int)(randomSubtractionProblem.Item1 - randomSubtractionProblem.Item2);

                default: throw new System.InvalidOperationException("Invalid GameType");
            }
        }

    }
}
