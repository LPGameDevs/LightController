using System.Collections.Generic;
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

        [TestCase(13, 3, 9)]
        [TestCase(10, 2, 7)]
        [TestCase(9, 2, 6)]
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

        private static IEnumerable<TestTowStartingPositionsFourPlayersCase> TestTowStartingPositionsFourPlayersCases
        {
            get
            {
                yield return new TestTowStartingPositionsFourPlayersCase(13, new List<LightChecks>()
                {
                    new LightChecks(new List<LightCheck>()
                    {
                        new LightCheck(3,true),
                        new LightCheck(4,false),
                        new LightCheck(8,false),
                        new LightCheck(9,false),
                    }),
                    new LightChecks(new List<LightCheck>()
                    {
                        new LightCheck(3,true),
                        new LightCheck(4,false),
                        new LightCheck(8,false),
                        new LightCheck(9,true),
                    }),
                    new LightChecks(new List<LightCheck>()
                    {
                    new LightCheck(3,true),
                    new LightCheck(4,true),
                    new LightCheck(8,true),
                    new LightCheck(9,false),
                    }),
                    new LightChecks(new List<LightCheck>()
                    {
                        new LightCheck(3,true),
                        new LightCheck(4,true),
                        new LightCheck(8,true),
                        new LightCheck(9,true),
                    })
                }, true);
                yield return new TestTowStartingPositionsFourPlayersCase(10, new List<LightChecks>()
                {
                    new LightChecks(new List<LightCheck>()
                    {
                        new LightCheck(2,true),
                        new LightCheck(3,false),
                        new LightCheck(6,false),
                        new LightCheck(7,false),
                    }),
                    new LightChecks(new List<LightCheck>()
                    {

                        new LightCheck(2,true),
                        new LightCheck(3,false),
                        new LightCheck(6,false),
                        new LightCheck(7,true),
                    }),
                    new LightChecks(new List<LightCheck>()
                    {

                        new LightCheck(2,true),
                        new LightCheck(3,true),
                        new LightCheck(6,true),
                        new LightCheck(7,false),
                    }),
                    new LightChecks(new List<LightCheck>()
                    {

                        new LightCheck(2,true),
                        new LightCheck(3,true),
                        new LightCheck(6,true),
                        new LightCheck(7,true),
                    })
                }, true);
                yield return new TestTowStartingPositionsFourPlayersCase(9, new List<LightChecks>()
                {
                    new LightChecks(new List<LightCheck>()
                    {
                        new LightCheck(2,true),
                        new LightCheck(3,false),
                        new LightCheck(5,false),
                        new LightCheck(6,false),
                    }),
                    new LightChecks(new List<LightCheck>()
                    {
                        new LightCheck(2,true),
                        new LightCheck(3,false),
                        new LightCheck(5,false),
                        new LightCheck(6,true),
                    }),
                    new LightChecks(new List<LightCheck>()
                    {
                        new LightCheck(2,true),
                        new LightCheck(3,true),
                        new LightCheck(5,true),
                        new LightCheck(6,false),
                    }),
                    new LightChecks(new List<LightCheck>()
                    {
                        new LightCheck(2,true),
                        new LightCheck(3,true),
                        new LightCheck(5,true),
                        new LightCheck(6,true),
                    }),
                }, true);
                yield return new TestTowStartingPositionsFourPlayersCase(5, new List<LightChecks>()
                    {
                        new LightChecks(new List<LightCheck>()
                        {
                            new LightCheck(1,true),
                            new LightCheck(3,false),
                        }),
                        new LightChecks(new List<LightCheck>()
                        {
                            new LightCheck(1,true),
                            new LightCheck(3,true),
                        }),
                    }, false);
            }
        }

        [Test, TestCaseSource(nameof(TestTowStartingPositionsFourPlayersCases))]
        public void TestTowStartingPositionsFourPlayers(TestTowStartingPositionsFourPlayersCase testCase)
        {
            TugOfWarGame game = new TugOfWarGame(testCase.totalLights);
            LightController lightController = game.GetLightController();

            game.AddPlayerTeamLeft();
            foreach (LightCheck lightCheck in testCase.lightChecks[0].lightChecks)
            {
                Assert.AreEqual(lightCheck.IsOn, lightController.IsLightOn(lightCheck.Position));
            }


            game.AddPlayerTeamRight();
            foreach (LightCheck lightCheck in testCase.lightChecks[1].lightChecks)
            {
                Assert.AreEqual(lightCheck.IsOn, lightController.IsLightOn(lightCheck.Position));
            }

            if (testCase.playersWillFit)
            {
                game.AddPlayerTeamLeft();
                foreach (LightCheck lightCheck in testCase.lightChecks[2].lightChecks)
                {
                    Assert.AreEqual(lightCheck.IsOn, lightController.IsLightOn(lightCheck.Position));
                }

                game.AddPlayerTeamRight();
                foreach (LightCheck lightCheck in testCase.lightChecks[3].lightChecks)
                {
                    Assert.AreEqual(lightCheck.IsOn, lightController.IsLightOn(lightCheck.Position));
                }
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



        public struct TestTowStartingPositionsFourPlayersCase
        {
            public int totalLights;
            public List<LightChecks> lightChecks;
            public bool playersWillFit;

            public TestTowStartingPositionsFourPlayersCase(int totalLights, List<LightChecks> lightChecks, bool playersWillFit)
            {
                this.totalLights = totalLights;
                this.lightChecks = lightChecks;
                this.playersWillFit = playersWillFit;
            }
        }

        public struct LightChecks
        {
            public List<LightCheck> lightChecks;

            public LightChecks(List<LightCheck> lightChecks)
            {
                this.lightChecks = lightChecks;
            }
        }

        public struct LightCheck
        {
            public int Position;
            public bool IsOn;

            public LightCheck(int position, bool isOn)
            {
                Position = position;
                IsOn = isOn;
            }
        }
    }
}
