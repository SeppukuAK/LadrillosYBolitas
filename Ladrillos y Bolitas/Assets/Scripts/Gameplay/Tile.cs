using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

    public int X { get; set; }
    public int Y { get; set; }

    LevelManager _levelManager;

    public Text HealthText;

    private uint health;
    public uint Health
    {
        get { return health; }
        set
        {
            health = value;
            HealthText.text = health.ToString();
        }
    }

    public void Init(LevelManager levelManager, uint pendingTouches, int x, int y)
    {
        X = x;
        Y = y;
        Health = pendingTouches;
        _levelManager = levelManager;
    }

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;

        transform.localPosition = new Vector3(X, Y, 0);
        if (Y == 0)
            _levelManager.LevelEnd();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health--;
        if (health == 0)
            _levelManager.TileDestroyed(this);
    }

}
