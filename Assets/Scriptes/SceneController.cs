using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    public int gridRows = 2;
    public int gridCols = 2;
    public float offsetX = 2.5f;
    public float offsetY = 3.5f;

    private int _score = 0;

    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;

    private List<MemoryCard> cards = new List<MemoryCard>();
    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        CloseAllCards();

        foreach (var card in cards)
        {
            if (card != originalCard)
            {
                Destroy(card.gameObject);
            }
        }
        cards.Clear();

        Vector3 startPos = originalCard.transform.position;

        int totalCards = gridRows * gridCols;
        int[] numbers = new int[totalCards];
        for (int i = 0; i < totalCards; i += 2)
        {
            int pairId = i / 2;
            numbers[i] = pairId % images.Length;
            numbers[i + 1] = pairId % images.Length;
        }
        numbers = ShuffleArray(numbers);

        Vector2 offsets = GetOffsetsForGrid();

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = j * gridCols + i;
                int id = numbers[index];

                card.SetCard(id, images[id]);

                float posX = (offsets.x * i) + startPos.x;
                float posY = -(offsets.y * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);

                cards.Add(card);
            }
        }

        _score = 0;
        scoreLabel.text = "Score: " + _score;
        _firstRevealed = null;
        _secondRevealed = null;
    }

    private void CloseAllCards()
    {
        foreach (var card in cards)
        {
            if (card != null)
            {
                card.Unreveal();
            }
        }
    }

    private Vector2 GetOffsetsForGrid()
    {
        int totalCards = gridRows * gridCols;

        switch (totalCards)
        {
            case 4: 
                return new Vector2(2f, 2f);
            case 6: 
                return new Vector2(2f, 2f);
            case 8: 
                return new Vector2(2f, 2f);
            case 10: 
                return new Vector2(2f, 2f);
            default: 
                return new Vector2(offsetX, offsetY);
        }
    }

    public void IncreaseCards()
    {
        int totalCards = gridRows * gridCols;

        switch (totalCards)
        {
            case 4:
                gridRows = 2;
                gridCols = 3;
                break;
            case 6: 
                gridRows = 2;
                gridCols = 4;
                break;
            case 8: 
                gridRows = 2;
                gridCols = 5;
                break;
            case 10: 
                gridRows = 2;
                gridCols = 2;
                break;
            default:
                gridRows = 2;
                gridCols = 2;
                break;
        }

        CreateGrid();
    }

    public void DecreaseCards()
    {
        int totalCards = gridRows * gridCols;

        switch (totalCards)
        {
            case 4: 
                gridRows = 2;
                gridCols = 5;
                break;
            case 6: 
                gridRows = 2;
                gridCols = 2;
                break;
            case 8: 
                gridRows = 2;
                gridCols = 3;
                break;
            case 10:
                gridRows = 2;
                gridCols = 4;
                break;
            default:
                gridRows = 2;
                gridCols = 2;
                break;
        }

        CreateGrid();
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public bool canReveal
    {
        get { return _secondRevealed == null; }
    }

    public void CardRevealed(MemoryCard card)
    {
        if (_firstRevealed == null)
        {
            _firstRevealed = card;
        }
        else
        {
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }
        _firstRevealed = null;
        _secondRevealed = null;
    }

    public void Restart()
    {
        Application.LoadLevel("SampleScene");
    }
}