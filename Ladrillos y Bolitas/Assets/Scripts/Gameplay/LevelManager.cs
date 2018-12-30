﻿using UnityEngine;

/// <summary>
/// Manager de un nivel de juego
/// Gestiona el flujo de la partida, las rondas de tiro, los bloques, etc.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Game Attributes")]
    [SerializeField] private uint ballSpawnTickRate;
    [SerializeField] private float ballToSinkTime;
    [SerializeField] private float ballVelocity;

    [Header("References")]
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private BallSink ballSink;
    [SerializeField] private DeathZone deathZone;
    [SerializeField] private Board board;
    [SerializeField] private AimController aimController;

    public uint CurrentNumBalls { get; set; }

    public delegate void OnRoundStart();
    public OnRoundStart OnRoundStartCallback;

    public delegate void OnRoundEnd();
    public OnRoundStart OnRoundEndCallback;

    /// <summary>
    /// Inicializa todos los componentes, pasandole los atributos que necesitan para su funcionamiento en la escena
    /// </summary>
    private void Start()
    {
        CurrentNumBalls = uint.Parse(GameManager.Instance.GameData.text.Split('\n')[GameManager.Instance.MapLevel].Split(' ', ',')[1]);

        aimController.Init(this, ballSpawner, ballVelocity);
        ballSpawner.Init(ballPrefab, ballSpawnTickRate);
        deathZone.Init(this, ballSink, ballToSinkTime);
        ballSink.Init(this);
        board.Init(this, GameManager.Instance.MapData[GameManager.Instance.MapLevel]);
    }

    /// <summary>
    /// Informa a los componentes subscritos
    /// Es llamado cuando se inicia el disparo
    /// </summary>
    public void RoundStart()
    {
        if (OnRoundStartCallback != null)
            OnRoundStartCallback.Invoke();
    }

    /// <summary>
    /// Establece la posición del ballSpawner a a la del ballSink e informa a todos los subscritos que ha acabado la ronda
    /// Es llamado cuando todas las pelotas han caido
    /// </summary>
    public void RoundEnd()
    {
        ballSpawner.transform.position = ballSink.transform.position;

        if (OnRoundEndCallback != null)
            OnRoundEndCallback.Invoke();
    }

    public void LevelEnd()
    {
        Debug.Log("has ganado:" + (board.PendingTiles == 0));
    }
}
