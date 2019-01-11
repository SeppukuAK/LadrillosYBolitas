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
    public override uint Value
    {
        get { return value; }
        set
        {
            this.value = value;
            healthText.text = value.ToString();
        }
    }
    protected uint value;

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
        _onDestroyCallBack = onDestroyCallBack;
    }

    /// <summary>
    /// Si colisiona la bola, le resta vida. Comprueba si ha llegado a 0 e informa al Callback en tal caso y se destruye
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Value--;
        if (Value == 0)
            _onDestroyCallBack(this);

    }

}
