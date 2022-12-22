using System;
using System.Collections.Generic;
using System.Linq;

namespace LightController.Games
{
    public class TugOfWarGame
    {
        public enum Team
        {
            Left = 0,
            Right = 1,
            None = 2
        }

        private int _totalSpaces;
        private int _playerCount;
        private int _playerCountLeft;
        private int _playerCountRight;

        LightController _lightController;

        public TugOfWarGame(int totalSpaces)
        {
            _totalSpaces = totalSpaces;
            SetupBoard();
            _lightController = new LightController();
            _lightController.SetLights(totalSpaces);
        }

        public void SetupBoard()
        {

        }

        public LightController GetLightController()
        {
            return _lightController;
        }

        public void AddPlayer()
        {
            _playerCount++;
        }

        public int GetNumberOfPlayers()
        {
            return _playerCount;
        }
        public void AddPlayerTeamLeft()
        {
            int currentNumberOfPlayers = GetNumberOfPlayersLeft();
            if (currentNumberOfPlayers == GetMaxPlayersPerTeam())
            {
                throw new TeamIsFullException();
            }

            List<int> winningSpaces = GetWinningSpaces();
            int position = winningSpaces.Min() - 1 - GetNumberOfPlayersLeft();
            AddPlayer();
            _lightController.TurnOnLight(position);
            _playerCountLeft++;
        }



        public void AddPlayerTeamRight()
        {
            int currentNumberOfPlayers = GetNumberOfPlayersRight();
            if (currentNumberOfPlayers == GetMaxPlayersPerTeam())
            {
                throw new TeamIsFullException();
            }

            List<int> winningSpaces = GetWinningSpaces();
            int position = winningSpaces.Max() + 1 + GetNumberOfPlayersRight();
            AddPlayer();
            _lightController.TurnOnLight(position);
            _playerCountRight++;
        }

        private List<int> GetWinningSpaces()
        {
            List<int> winningSpaces = new List<int>();
            if (_totalSpaces % 2 == 0)
            {
                winningSpaces.Add((int) Math.Floor((double) (_totalSpaces - 1) / 2));
                winningSpaces.Add((int) Math.Ceiling((double) (_totalSpaces - 1) / 2));
            }
            else
            {
                winningSpaces.Add((_totalSpaces - 1) / 2);
            }

            return winningSpaces;
        }

        public int GetNumberOfPlayersLeft()
        {
            return _playerCountLeft;
        }

        public int GetNumberOfPlayersRight()
        {
            return _playerCountRight;
        }

        private int GetMaxPlayersPerTeam()
        {
            return GetTeamAreaSize() - 1;
        }

        private int GetTeamAreaSize()
        {
            return (int) Math.Ceiling((double) (_totalSpaces - 1) / 2);
        }

        public Team GetWinner()
        {
            if (GetNumberOfPlayersLeft() > GetNumberOfPlayersRight())
            {
                return Team.Left;
            }

            if (GetNumberOfPlayersLeft() == GetNumberOfPlayersRight())
            {
                return Team.None;
            }

            return Team.Right;
        }

        public void Move()
        {
            if (GetNumberOfPlayers() < 2)
            {
                throw new NotEnoughPlayersException();
            }

            if (GetNumberOfPlayersLeft() == 0 || GetNumberOfPlayersRight() == 0)
            {
                throw new NotEnoughTeamsException();
            }

            Team winner = GetWinner();
            if (winner == Team.None)
            {
                return;
                // throw new GameIsTiedException();
            }

            // Get players.
            List<int> players = new List<int>();
            for (int i = 0; i < _lightController.GetNumberOfLights(); i++)
            {
                if (_lightController.IsLightOn(i))
                {
                    players.Add(i);
                }
            }

            if (winner == Team.Left)
            {
                foreach (int player in players)
                {
                    _lightController.TurnOffLight(player);
                }

                foreach (int player in players)
                {
                    _lightController.TurnOnLight(player - 1);
                }
            }
            else
            {
                foreach (int player in players)
                {
                    _lightController.TurnOffLight(player);
                }

                foreach (int player in players)
                {
                    _lightController.TurnOnLight(player + 1);
                }
            }
        }

        public bool IsGameOver()
        {
            List<int> winningSpaces = GetWinningSpaces();
            foreach (int winningSpace in winningSpaces)
            {
                if (_lightController.IsLightOn(winningSpace))
                {
                    return true;
                }
            }

            return false;
        }

        public class NotEnoughPlayersException : Exception
        {
        }

        public class NotEnoughTeamsException : Exception
        {
        }

        public class TeamIsFullException : Exception
        {
        }
    }

}
