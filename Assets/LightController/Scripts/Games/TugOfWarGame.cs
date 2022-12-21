using System;

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

            int middle = (int) Math.Floor((double) (_totalSpaces - 1) / 2);
            int position = middle - 1 - GetNumberOfPlayersLeft();
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

            int middle = (int) Math.Ceiling((double) (_totalSpaces - 1) / 2);
            int position = middle + 1 + GetNumberOfPlayersRight();
            AddPlayer();
            _lightController.TurnOnLight(position);
            _playerCountRight++;
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
