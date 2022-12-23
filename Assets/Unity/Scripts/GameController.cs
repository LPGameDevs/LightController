using System;
using LightController.Games;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action<int> OnSetSpecialPosition;

    private TugOfWarGame _game;
    private LightController.LightController _lightController;

    public int StripSize = 10;

    private void Start()
    {
        _game = new TugOfWarGame(StripSize);
        _lightController = _game.GetLightController();

        SetWinningSpaces();
    }

    private void SetWinningSpaces()
    {
        if (_game == null)
        {
            return;
        }

        foreach (int winningSpace in _game.GetWinningSpaces())
        {
            OnSetSpecialPosition?.Invoke(winningSpace);
        }
    }

    public void AddPlayerLeft()
    {
        try
        {
            _game.AddPlayerTeamLeft();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void AddPlayerRight()
    {
        try
        {
            _game.AddPlayerTeamRight();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnResetBoard()
    {
        SetWinningSpaces();
    }

    public void Move()
    {
        try
        {
            _game.Move();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnEnable()
    {
        LightController.LightController.OnResetBoard += OnResetBoard;
    }

    private void OnDisable()
    {
        LightController.LightController.OnResetBoard -= OnResetBoard;
    }
}
