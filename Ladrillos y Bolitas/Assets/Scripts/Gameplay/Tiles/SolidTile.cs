using UnityEngine;
using UnityEngine.UI;

public class SolidTile : Tile
{
    private Text healthText;
    private System.Action<Tile> _onDestroyCallBack;

    /// <summary>
    /// Cuando es modificado se actualiza el texto
    /// </summary>
    protected int health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthText.text = _health.ToString();
        }
    }
    private int _health;

    /// <summary>
    /// Cuando es modificado se renderiza o no
    /// </summary>
    protected override bool visible
    {
        set
        {
            base.visible = value;
            healthText.enabled = _visible;
        }
    }

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        healthText = GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Inicializa el Tile
    /// </summary>
    /// <param name="health"></param>
    /// <param name="onDestroyCallBack"></param>
    public override void Init(int value, System.Action<Tile> onDestroyCallBack = null)
    {
        base.Init(value, onDestroyCallBack);
        health = value;
        _onDestroyCallBack = onDestroyCallBack;
    }

    /// <summary>
    /// Si colisiona la bola, le resta vida. Comprueba si ha llegado a 0 e informa al Callback en tal caso y se destruye
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        health--;
        if (health == 0)
            if (_onDestroyCallBack != null)
            {
                _onDestroyCallBack(this);
                Destroy(gameObject);
            }
    }

}
