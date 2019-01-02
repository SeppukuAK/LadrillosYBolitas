using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tile que puede ser destruido
/// </summary>
public class SolidTile : Tile
{
    private System.Action<Tile> _onDestroyCallBack;
    private Text healthText;

    /// <summary>
    /// Cuando es modificado se actualiza el texto
    /// </summary>
    protected uint health
    {
        get { return uint.Parse(healthText.text); }
        set
        {
            healthText.text = value.ToString();
        }
    }

    /// <summary>
    /// Cuando es modificado se renderiza o no
    /// </summary>
    protected override bool visible
    {
        set
        {
            base.visible = value;
            healthText.enabled = value;
        }
    }

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        healthText = GetComponentInChildren<Text>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (healthText == null)
            Debug.LogError("Text no asociado");
#endif
    }

    /// <summary>
    /// Inicializa el Tile
    /// Establece su posicion y guarda la vida y callback
    /// </summary>
    /// <param name="health"></param>
    /// <param name="onDestroyCallBack"></param>
    public override void Init(int value, System.Action<Tile> onDestroyCallBack = null)
    {
        base.Init(value, onDestroyCallBack);
        health = (uint)value;
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
            _onDestroyCallBack(this);

    }

}
