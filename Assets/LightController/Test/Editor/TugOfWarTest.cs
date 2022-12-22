using LightController.Games;
using NUnit.Framework;

namespace LightController.Test
{
    public class TugOfWarTest
    {

        /*
         * 1. Create a new game
         * 2. Add 1 player to the game.
         * 3. Check that player is in correct team.
         * 4. Add another player to the same team.
         * 5. Check that both players are in the same team.
         * 6. Add another player to the other team.
         * 7. Check that both teams have the right number of players.
         * 8. Start the game
         * 9. Check who wins the game.
         */

        [Test]
        public void TestTowPickSides() {
            // Start the game - no winner.
            TugOfWarGame game = new TugOfWarGame(10);
            LightController lightController = game.GetLightController();

            TugOfWarGame.Team winningTeam = game.GetWinner();
            Assert.AreEqual(TugOfWarGame.Team.None, winningTeam);

            // Add player left - left wins.
            game.AddPlayerTeamLeft();

            int numberOfPlayers = game.GetNumberOfPlayers();
            Assert.AreEqual(1, numberOfPlayers);

            winningTeam = game.GetWinner();
            Assert.AreEqual(TugOfWarGame.Team.Left, winningTeam);

            // Add player right - no winner.
            game.AddPlayerTeamRight();

            numberOfPlayers = game.GetNumberOfPlayers();
            Assert.AreEqual(2, numberOfPlayers);

            winningTeam = game.GetWinner();
            Assert.AreEqual(TugOfWarGame.Team.None, winningTeam);

            // Add second player right - right wins.
            game.AddPlayerTeamRight();

            numberOfPlayers = game.GetNumberOfPlayers();
            Assert.AreEqual(3, numberOfPlayers);

            int numberOfPlayersRight = game.GetNumberOfPlayersRight();
            Assert.AreEqual(2, numberOfPlayersRight);
            int numberOfPlayersLeft = game.GetNumberOfPlayersLeft();
            Assert.AreEqual(1, numberOfPlayersLeft);

            winningTeam = game.GetWinner();
            Assert.AreEqual(TugOfWarGame.Team.Right, winningTeam);

            game.AddPlayerTeamLeft();
        }

        [TestCase(10, 3, 6)]
        [TestCase(9, 3, 5)]
        [TestCase(5, 1, 3)]
        public void TestTowStartingPositionsTwoPlayers(int totalLights, int player1pos, int player2pos)
        {
            TugOfWarGame game = new TugOfWarGame(totalLights);
            LightController lightController = game.GetLightController();

            Assert.AreEqual(false, lightController.IsLightOn(player1pos));
            Assert.AreEqual(false, lightController.IsLightOn(player2pos));

            game.AddPlayerTeamLeft();
            Assert.AreEqual(true, lightController.IsLightOn(player1pos));
            game.AddPlayerTeamRight();
            Assert.AreEqual(true, lightController.IsLightOn(player2pos));
        }

        [TestCase(10, 3, 6, 2,7,true)]
        [TestCase(9, 3, 5,2,6, true)]
        [TestCase(5, 1, 3, 0, 0, false)]
        public void TestTowStartingPositionsFourPlayers(int totalLights, int player1pos, int player2pos, int player3pos, int player4pos, bool playersWillFit)
        {
            TugOfWarGame game = new TugOfWarGame(totalLights);
            LightController lightController = game.GetLightController();

            Assert.AreEqual(false, lightController.IsLightOn(player1pos));
            Assert.AreEqual(false, lightController.IsLightOn(player2pos));
            Assert.AreEqual(false, lightController.IsLightOn(player3pos));
            Assert.AreEqual(false, lightController.IsLightOn(player4pos));

            game.AddPlayerTeamLeft();
            Assert.AreEqual(true, lightController.IsLightOn(player1pos));
            game.AddPlayerTeamRight();
            Assert.AreEqual(true, lightController.IsLightOn(player2pos));

            if (playersWillFit)
            {
                game.AddPlayerTeamLeft();
                Assert.AreEqual(true, lightController.IsLightOn(player3pos));
                game.AddPlayerTeamRight();
                Assert.AreEqual(true, lightController.IsLightOn(player4pos));

            }
            else
            {
                Assert.Throws<TugOfWarGame.TeamIsFullException>(
                    () => { game.AddPlayerTeamLeft(); });
                Assert.Throws<TugOfWarGame.TeamIsFullException>(
                    () => { game.AddPlayerTeamRight(); });

            }
        }


        [Test]
        public void TestTowMove()
        {
            // Start the game.
            TugOfWarGame game = new TugOfWarGame(10);
            LightController lightController = game.GetLightController();

            Assert.Throws<TugOfWarGame.NotEnoughPlayersException>(
                () => { game.Move(); });

            // Add player left - Not enough players.
            game.AddPlayerTeamLeft();
            Assert.Throws<TugOfWarGame.NotEnoughPlayersException>(
                () => { game.Move(); });

            // Add player left - Not enough teams (all players on left).
            game.AddPlayerTeamLeft();
            Assert.Throws<TugOfWarGame.NotEnoughTeamsException>(
                () => { game.Move(); });


            // Add player right.
            game.AddPlayerTeamRight();

            // Check player positions.
            Assert.AreEqual(false, lightController.IsLightOn(1));
            Assert.AreEqual(true, lightController.IsLightOn(2));
            Assert.AreEqual(true, lightController.IsLightOn(3));
            Assert.AreEqual(false, lightController.IsLightOn(4));
            Assert.AreEqual(false, lightController.IsLightOn(5));
            Assert.AreEqual(true, lightController.IsLightOn(6));
            Assert.AreEqual(false, lightController.IsLightOn(7));

            game.Move();

            // Check players have moved to the left.
            Assert.AreEqual(true, lightController.IsLightOn(1));
            Assert.AreEqual(true, lightController.IsLightOn(2));
            Assert.AreEqual(false, lightController.IsLightOn(3));
            Assert.AreEqual(false, lightController.IsLightOn(4));
            Assert.AreEqual(true, lightController.IsLightOn(5));
            Assert.AreEqual(false, lightController.IsLightOn(6));
            Assert.AreEqual(false, lightController.IsLightOn(7));

            // Setup new game with draw outcome.
            game = new TugOfWarGame(10);
            lightController = game.GetLightController();
            game.AddPlayerTeamLeft();
            game.AddPlayerTeamLeft();
            game.AddPlayerTeamRight();
            game.AddPlayerTeamRight();

            // Check starting positions.
            Assert.AreEqual(false, lightController.IsLightOn(1));
            Assert.AreEqual(true, lightController.IsLightOn(2));
            Assert.AreEqual(true, lightController.IsLightOn(3));
            Assert.AreEqual(false, lightController.IsLightOn(4));
            Assert.AreEqual(false, lightController.IsLightOn(5));
            Assert.AreEqual(true, lightController.IsLightOn(6));
            Assert.AreEqual(true, lightController.IsLightOn(7));
            Assert.AreEqual(false, lightController.IsLightOn(8));

            game.Move();

            // Check no movement
            Assert.AreEqual(false, lightController.IsLightOn(1));
            Assert.AreEqual(true, lightController.IsLightOn(2));
            Assert.AreEqual(true, lightController.IsLightOn(3));
            Assert.AreEqual(false, lightController.IsLightOn(4));
            Assert.AreEqual(false, lightController.IsLightOn(5));
            Assert.AreEqual(true, lightController.IsLightOn(6));
            Assert.AreEqual(true, lightController.IsLightOn(7));
            Assert.AreEqual(false, lightController.IsLightOn(8));

            // Setup new game with right team win outcome.
            game = new TugOfWarGame(10);
            lightController = game.GetLightController();
            game.AddPlayerTeamLeft();
            game.AddPlayerTeamLeft();
            game.AddPlayerTeamRight();
            game.AddPlayerTeamRight();
            game.AddPlayerTeamRight();

            // Check starting positions.
            Assert.AreEqual(false, lightController.IsLightOn(1));
            Assert.AreEqual(true, lightController.IsLightOn(2));
            Assert.AreEqual(true, lightController.IsLightOn(3));
            Assert.AreEqual(false, lightController.IsLightOn(4));
            Assert.AreEqual(false, lightController.IsLightOn(5));
            Assert.AreEqual(true, lightController.IsLightOn(6));
            Assert.AreEqual(true, lightController.IsLightOn(7));
            Assert.AreEqual(true, lightController.IsLightOn(8));
            Assert.AreEqual(false, lightController.IsLightOn(9));

            game.Move();

            // Check movement to the right.
            Assert.AreEqual(false, lightController.IsLightOn(1));
            Assert.AreEqual(false, lightController.IsLightOn(2));
            Assert.AreEqual(true, lightController.IsLightOn(3));
            Assert.AreEqual(true, lightController.IsLightOn(4));
            Assert.AreEqual(false, lightController.IsLightOn(5));
            Assert.AreEqual(false, lightController.IsLightOn(6));
            Assert.AreEqual(true, lightController.IsLightOn(7));
            Assert.AreEqual(true, lightController.IsLightOn(8));
            Assert.AreEqual(true, lightController.IsLightOn(9));
        }

        [Test]
        public void TestTowWin()
        {
            // Start the game.
            TugOfWarGame game = new TugOfWarGame(10);

            game.AddPlayerTeamLeft();
            game.AddPlayerTeamLeft();
            game.AddPlayerTeamRight();

            Assert.AreEqual(false, game.IsGameOver());

            game.Move();

            Assert.AreEqual(true, game.IsGameOver());

            Assert.Throws<TugOfWarGame.GameOverException>(
                () => { game.Move(); });
        }
    }
}
