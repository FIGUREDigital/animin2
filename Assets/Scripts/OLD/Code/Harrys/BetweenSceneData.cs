using UnityEngine;
using System.Collections;

public class BetweenSceneData
{

    private bool m_ReturnFromMiniGame;
    public bool ReturnFromMiniGame
    {
        get
        {
            return m_ReturnFromMiniGame;
        }
        set
        {
            m_ReturnFromMiniGame = value;
        }
    }
    public enum Minigame {Collector, Guns};
    public Minigame minigame;


    private int m_Points;
    public int Points
    {
        get { return m_Points; }
        set { m_Points = value; }
    }
    public void ResetPoints() { Points = 0; }

    #region Singleton
    static BetweenSceneData m_Instance;
    public static BetweenSceneData Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new BetweenSceneData();
            return m_Instance;
        }
    }
    #endregion
}
