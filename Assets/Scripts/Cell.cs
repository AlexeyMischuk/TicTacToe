using Enums;
using UnityEngine;
using Zenject;

public class Cell : MonoBehaviour, IInitializable
{
    [SerializeField] private Sprite _crossSprite;
    [SerializeField] private Sprite _circleSprite;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private Color _highlightColor;

    private GameManager _gameManager;
    private EventService _eventService;
    public SymbolType Symbol { get; private set; }

    [Inject]
    public void Construct(GameManager gameManger, EventService eventService)
    {
        _gameManager = gameManger;
        _eventService = eventService;
        _eventService.AddListener(EventName.GameOver, BlockMouseClick);
    }

    public void Initialize()
    {
        
    }

    public void SetSymbol(SymbolType symbol)
    {
        Symbol = symbol;
        switch (symbol)
        {
            case SymbolType.Circle:
                gameObject.GetComponent<SpriteRenderer>().sprite = _circleSprite;
                break;
            case SymbolType.Cross:
                gameObject.GetComponent<SpriteRenderer>().sprite = _crossSprite;
                break;
            case SymbolType.Empty:
            default:
                gameObject.GetComponent<SpriteRenderer>().sprite = null;
                break;
        }
    }

    public void Clear()
    {
        Symbol = SymbolType.Empty;
        SetSymbol(SymbolType.Empty);
    }

    public void Highlight()
    {
        _background.color = _highlightColor;
    }

    private void OnMouseDown()
    {
        if (Symbol is SymbolType.Circle or SymbolType.Cross)
        {
            return;
        }
        
        SetSymbol(_gameManager.ActivePlayer.Symbol);
        _eventService.Invoke(EventName.MoveMade);
    }

    private void BlockMouseClick()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}