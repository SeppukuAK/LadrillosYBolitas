using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddBallsTile : Tile
{

    //Propiedades especificas del tile
    public override bool MustBeDestroyed() { return false; }
    public override bool Trigger() { return true; }

    [SerializeField] private uint nBalls;

    private Text ballsText;
    private System.Action<Tile> _onDestroyCallBack;


    /// <summary>
    /// Cuando es modificado se renderiza o no
    /// </summary>
    protected override bool visible
    {
        set
        {
            base.visible = value;
            ballsText.enabled = value;
        }
    }

    /// <summary>
    /// Cuando es modificado se actualiza el texto
    /// </summary>
    public override uint Value
    {
        get { return value; }
        set
        {
            this.value = value;
            ballsText.text = "+" + value.ToString();
        }
    }
    protected uint value;

    /// <summary>
    /// Obtiene referencias
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        ballsText = GetComponentInChildren<Text>();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (ballsText == null)
            Debug.LogError("Text no asociado");
#endif
    }

    /// <summary>
    /// Inicializa el Tile
    /// Establece su posicion y guarda el número de bolas que tiene que incrementar
    /// </summary>
    /// <param name="health"></param>
    /// <param name="onDestroyCallBack"></param>
    public override void Init(int value, System.Action<Tile> onDestroyCallBack = null)
    {
        SetPosition(new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y));
        Value = nBalls;
        _onDestroyCallBack = onDestroyCallBack;
    }

    /// <summary>
    /// Informa al level manager para eliminar el tile
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _onDestroyCallBack(this);
    }
}
