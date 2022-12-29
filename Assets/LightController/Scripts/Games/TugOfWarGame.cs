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
        private int _playerMovesToLoseLeft;
        private int _playerMovesToLoseRight;

        LightController _lightController;
        private bool _hasGameStarted;

        public TugOfWarGame(int totalSpaces)
        {
            _totalSpaces = totalSpaces;
            _lightController = new LightController();
            SetupBoard();
        }

        public void SetupBoard()
        {
            _lightController.SetLights(_totalSpaces);

            if (!_hasGameStarted)
            {
                SetStartingPositions();
            }
            else
            {
                SetGamePositions();
            }

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

        private void SetStartingPositions()
        {
            int maxTeamSize = Math.Max(GetNumberOfPlayersLeft(), GetNumberOfPlayersRight());
            int maxDistanceToWinningSpace = GetMaxDistanceToWinningSpace(maxTeamSize);

            int stepsToWin = _playerMovesToLoseLeft = _playerMovesToLoseRight = Math.Min(3, maxDistanceToWinningSpace);

            for (int i = 0; i < GetNumberOfPlayersLeft(); i++)
            {
                List<int> winningSpaces = GetWinningSpaces();
                int winningSpace = winningSpaces.Min();
                _lightController.TurnOnLight(winningSpace - stepsToWin - i);
            }

            for (int i = 0; i < GetNumberOfPlayersRight(); i++)
            {
                List<int> winningSpaces = GetWinningSpaces();
                int winningSpace = winningSpaces.Max();
                _lightController.TurnOnLight(winningSpace + stepsToWin + i);
            }
        }

        private void SetGamePositions()
        {
            for (int i = 0; i < GetNumberOfPlayersLeft(); i++)
            {
                List<int> winningSpaces = GetWinningSpaces();
                int winningSpace = winningSpaces.Min();
                _lightController.TurnOnLight(winningSpace - _playerMovesToLoseLeft - i);
            }

            for (int i = 0; i < GetNumberOfPlayersRight(); i++)
            {
                List<int> winningSpaces = GetWinningSpaces();
                int winningSpace = winningSpaces.Max();
                _lightController.TurnOnLight(winningSpace + _playerMovesToLoseRight + i);
            }
        }

        public void AddPlayerTeamLeft()
        {
            int currentNumberOfPlayers = GetNumberOfPlayersLeft();
            if (currentNumberOfPlayers == GetMaxPlayersPerTeam())
            {
                throw new TeamIsFullException();
            }

            AddPlayer();
            _playerCountLeft++;
            SetupBoard();
        }


        public void AddPlayerTeamRight()
        {
            int currentNumberOfPlayers = GetNumberOfPlayersRight();
            if (currentNumberOfPlayers == GetMaxPlayersPerTeam())
            {
                throw new TeamIsFullException();
            }

            AddPlayer();
            _playerCountRight++;
            SetupBoard();
        }

        private int GetMaxDistanceToWinningSpace(int teamSize)
        {
            int playableSpaces = (_totalSpaces - GetWinningSpaces().Count) / 2;
            return (int) Math.Ceiling((double) (playableSpaces - teamSize) / 2);
        }

        public List<int> GetWinningSpaces()
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

            if (IsGameOver())
            {
                throw new GameOverException();
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
                _playerMovesToLoseLeft++;
                _playerMovesToLoseRight--;
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
                _playerMovesToLoseLeft--;
                _playerMovesToLoseRight++;
                foreach (int player in players)
                {
                    _lightController.TurnOffLight(player);
                }

                foreach (int player in players)
                {
                    _lightController.TurnOnLight(player + 1);
                }
            }

            _hasGameStarted = true;
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

        public class GameOverException : Exception
        {
        }
    }


}
