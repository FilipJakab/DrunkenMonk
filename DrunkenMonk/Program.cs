using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DrunkenMonk.ConsoleHelpers;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Base;
using DrunkenMonk.Data.Enums;
using DrunkenMonk.Providers;
using NLog;

namespace DrunkenMonk
{
	public class Program
	{
		public static void Main()
		{
			Logger logger = LogManager.GetCurrentClassLogger();

			Configurations configurations = new Configurations();

			NPCProvider npcProvider = new NPCProvider();

			GameContext context = new GameContext
			{
				Player = new Player(),
				ScoreBoard = new TextCanvas(),
				Square = new Canvas(),
				MovementLog = new TextCanvas()
			};

			PaintBrush brush = new PaintBrush();

			Init(context, logger).Wait();

			int amountOfPeopleOnSquare = (int)GetAmountOfPeople(context.Square, context.Player.DifficultyLevel, logger);

			context.Enemies = new List<Enemy>();
			context.Enemies = npcProvider.GenerateEnemies(
				context.Square,
				context.Player,
				amountOfPeopleOnSquare);

			brush.Render(context.Square, context.Enemies.Select(x => x.Position), Enemy.BodyCharacter);

			Timer timer = new Timer(state =>
			{
				context.ScoreBoard.Clear();
				context.ScoreBoard.WriteLine($"Current Time: {DateTime.Now: hh:mm:ss}");
				context.ScoreBoard.WriteLine($"Position: [{context.Player.Position.X},{context.Player.Position.Y}]");
			}, null, 0, 1000);

			Task task = null;
			CancellationTokenSource cts = null;
			do
			{
				// Wait for GUI to finish
				if (!task?.IsCompleted ?? false) continue;

				// Shutdown Game
				if (cts?.IsCancellationRequested ?? false) return;

				// To empty Console buffer
				while (Console.KeyAvailable)
				{
					Console.ReadKey(true);
				}

				ConsoleKey key = Console.ReadKey(true).Key;

				cts = new CancellationTokenSource();
				task = Task.Run(() =>
				{
					UserAction action = GetUserAction(key, logger);

					// Copy Players Position and Direction
					Position newPlayerPosition = Position.Copy(context.Player.Position);
					Position oldPosition = Position.Copy(context.Player.Position);

					Direction newPlayerDirection = context.Player.Direction;

					#region Handle User Actions

					switch (action)
					{
						case UserAction.DirectionUp:
							{
								newPlayerPosition.Y--;
								newPlayerDirection = Direction.Up;
								break;
							}
						case UserAction.DirectionDown:
							{
								newPlayerPosition.Y++;
								newPlayerDirection = Direction.Down;
								break;
							}
						case UserAction.DirectionLeft:
							{
								newPlayerPosition.X--;
								newPlayerDirection = Direction.Left;
								break;
							}
						case UserAction.DirectionRight:
							{
								newPlayerPosition.X++;
								newPlayerDirection = Direction.Right;
								break;
							}
						case UserAction.Reload:
							{
								Reload(context, timer, logger);
								return;
							}
						case UserAction.ShowPath:
							{
								/**
									 * TODO: Call PathFinder
									 * Use .To2DBinaryArray extension on Canvas
									 */
								break;
							}
						case UserAction.QuitGame:
							{
								cts.Cancel();
								break;
							}
					}

					#endregion

					// TODO: Handle player's tripping

					TripAndCollisionLogic(context, oldPosition, newPlayerPosition, newPlayerDirection, brush);

					Thread.Sleep(configurations.GetMainDelay);
				}, cts.Token);
			} while (true);
		}

		/// <summary>
		/// Checks if next position will be collision and handles it
		/// </summary>
		/// <param name="context"></param>
		/// <param name="lastPosition"></param>
		/// <param name="newPlayerPosition"></param>
		/// <param name="newPlayerDirection"></param>
		/// <param name="brush"></param>
		private static void TripAndCollisionLogic(
			GameContext context,
			Position lastPosition,
			Position newPlayerPosition,
			Direction newPlayerDirection,
			PaintBrush brush)
		{
			Random random = new Random(DateTime.Now.Millisecond);

			bool collided = newPlayerPosition.PredictCollision(context.Enemies.Select(enemy => enemy.Position));

			bool tripped = false;

			if (!collided)
			{
				tripped = random.Next(1, 5) == 1; // 25% chance
			}

			if (collided)
			{
				SimulationResult simulationResult = null;

				do
				{
					// TODO: Move constnat numbres to App.config
					Simulation simulation = (simulationResult?.Obstacle ?? newPlayerPosition)
						.SimulateCollision(
							newPlayerPosition,
							newPlayerDirection,
							3, 4);

					newPlayerDirection = newPlayerDirection.Reverse();

					simulationResult = context.Square.ExecuteSimulation(simulation, newPosition =>
					{
						return !newPosition.PredictCollision(context.Enemies.Select(enemy => enemy.Position));
					});

					newPlayerPosition = simulationResult.LastSafePosition;
				} while (!simulationResult.HasSuccessfulyFinished);

				context.Player.Position = simulationResult.LastSafePosition;
			}
			else if (tripped)
			{
				SimulationResult simulationResult = null;

				do
				{
					Simulation simulation;

					// TODO: Move constnat numbres to App.config
					if (!simulationResult?.HasSuccessfulyFinished ?? false)
					{
						simulation = newPlayerPosition
							.SimulateCollision(
								simulationResult.LastSafePosition,
								newPlayerDirection,
								3, 4);

						newPlayerDirection = newPlayerDirection.Reverse();
					}
					else
					{
						simulation = newPlayerPosition
							.SimulateTrip(
								simulationResult?.LastSafePosition ?? context.Player.Position,
								newPlayerDirection,
								2, 4);
					}

					simulationResult = context.Square.ExecuteSimulation(simulation, newPosition =>
					{
						return !newPosition.PredictCollision(context.Enemies.Select(enemy => enemy.Position))
							|| newPosition.Y < 0 || newPosition.X < 0; // TODO: Finish validation
					}, true);

					newPlayerPosition = simulationResult.Obstacle;
				} while (!simulationResult.HasSuccessfulyFinished);

				context.Player.Position = simulationResult.LastSafePosition;
			}
			else
			{
				brush.Derender(context.Square, context.Player.Position);

				// Update position and direction
				context.Player.Direction = newPlayerDirection;
				context.Player.Position = newPlayerPosition;

				brush.Render(context.Square, context.Player.Position, Player.BodyCharacter);
			}
		}

		private static void Reload(GameContext ctx, Timer timeTimer, Logger logger)
		{
			timeTimer.Change(UInt32.MaxValue, 0);

			Init(ctx, logger).Wait();

			int amountOfPeopleOnSquare = (int)GetAmountOfPeople(ctx.Square, ctx.Player.DifficultyLevel, logger);

			NPCProvider npcProvider = new NPCProvider();

			ctx.Enemies = npcProvider.GenerateEnemies(
				ctx.Square,
				ctx.Player,
				amountOfPeopleOnSquare);

			PaintBrush brush = new PaintBrush();

			brush.Render(ctx.Square, ctx.Enemies.Select(x => x.Position), Enemy.BodyCharacter);

			ctx.Player.Position.X = 0;
			ctx.Player.Position.Y = 0;

			timeTimer.Change(0, 1000);
		}

		/// <summary>
		/// Checks againts Arrow key (updating direction) and Escape key (meaning exit)
		/// </summary>
		/// <param name="key"></param>
		/// <param name="logger"></param>
		private static UserAction GetUserAction(ConsoleKey key, Logger logger)
		{
			logger.Trace($"{nameof(GetUserAction)} method called");

			logger.Debug($"Users key was {key}");

			// TODO: Use context.MovementLog to show on console what was pressed by user

			switch (key)
			{
				case ConsoleKey.W:
				case ConsoleKey.UpArrow:
					{
						return UserAction.DirectionUp;
					}
				case ConsoleKey.S:
				case ConsoleKey.DownArrow:
					{
						return UserAction.DirectionDown;
					}
				case ConsoleKey.A:
				case ConsoleKey.LeftArrow:
					{
						return UserAction.DirectionLeft;
					}
				case ConsoleKey.D:
				case ConsoleKey.RightArrow:
					{
						return UserAction.DirectionRight;
					}
				case ConsoleKey.R:
					{
						return UserAction.Reload;
					}
				case ConsoleKey.Spacebar:
					{
						return UserAction.ShowPath;
					}
				case ConsoleKey.Escape:
					{
						return UserAction.QuitGame;
					}
				default:
					{
						return UserAction.NoAction;
					}
			}
		}

		/// <summary>
		/// retruns amount of ppl bassed on difficulty
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="difficulty"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
		private static float GetAmountOfPeople(Canvas canvas, DifficultyLevel difficulty, Logger logger)
		{
			logger.Trace($"{nameof(GetAmountOfPeople)} method called");

			Random random = new Random(DateTime.Now.Millisecond);

			// Get 1% of screen size
			float amount = (canvas.Width * canvas.Height) * (1 / 100f);

			logger.Debug($"1% of canvas {canvas.Title} is {amount}");

			// multiply base 1% based on Chosen level
			switch (difficulty)
			{
				case DifficultyLevel.Easy:
					{
						amount *= random.Next(25, 35);
						break;
					}
				case DifficultyLevel.Medium:
					{
						amount *= random.Next(36, 50);
						break;
					}
				case DifficultyLevel.Hard:
					{
						amount *= random.Next(55, 65);
						break;
					}
			}

			logger.Debug($"The final amount of people of canvas {canvas.Title} is {amount}");

			return amount;
		}

		public static async Task Init(GameContext ctx, Logger logger)
		{
			logger.Trace("Init method called");

			Configurations config = new Configurations();

			Console.Title = config.GetConsoleTitle;

			Console.OutputEncoding = System.Text.Encoding.UTF8;

			// App.config doesnt have to be valid
			try
			{
				logger.Info("Setting up Console size");
				Console.BufferWidth = Console.WindowWidth = config.GetConsoleWidth;
				Console.BufferHeight = Console.WindowHeight = config.GetConsoleHeight;

				Console.WindowLeft = Console.WindowTop = 0;

				logger.Info("Setting up Square");
				// Console's grid is indexed from zero => doesnt need to add 1 to startX of ctx.Square
				ctx.Square.StartX = ctx.Square.StartY = config.GetComponentMargin;
				ctx.Square.Width = config.GetSquareWidth;
				ctx.Square.Height = config.GetSquareHeight;
				// TODO: Move Title to AppConfig
				ctx.Square.Title = "Main field - Square";

				logger.Info("Setting up ScoreBoard");
				ctx.ScoreBoard.StartX = (config.GetComponentMargin * 3) + ctx.Square.Width;
				ctx.ScoreBoard.StartY = ctx.Square.StartY;
				ctx.ScoreBoard.Width = config.GetScoreBoardWidth;
				ctx.ScoreBoard.Height = (int)Math.Ceiling(config.GetScoreBoardHeight / 2.0) - config.GetComponentMargin;
				ctx.ScoreBoard.Title = "Info board";

				ctx.MovementLog.StartX = ctx.ScoreBoard.StartX;
				ctx.MovementLog.StartY = ctx.ScoreBoard.Height + (config.GetComponentMargin * 3);
				ctx.MovementLog.Width = ctx.ScoreBoard.Width;
				ctx.MovementLog.Height = ctx.ScoreBoard.Height - 1;
				ctx.MovementLog.Title = "Movement History";
			}
			catch (FormatException ex)
			{
				logger.Error($"Some of App.config parameters were incorrect. Exception message: {ex.Message}");
			}
			catch (ArgumentNullException ex)
			{
				logger.Error($"Some of App.config parameters were null. Exception message: {ex.Message}");
			}

			Console.CursorVisible = false;

			DialogProvider dialogProvider = new DialogProvider();

			logger.Info("Setting players base position");
			ctx.Player.Position = new Position
			{
				X = 0,
				Y = 0
			};

			logger.Info("User asked to choose difficulty");
			// Ask for difficulty
			ctx.Player.DifficultyLevel = await dialogProvider.AskUser(new Menu<DifficultyLevel>
			{
				Question = "Select difficulty:",
				Options = new Dictionary<DifficultyLevel, string>
				{
					{
						DifficultyLevel.Easy,
						"Sir, let me have some drink"
					},
					{
						DifficultyLevel.Medium,
						"Alcoholic (professional)"
					},
					{
						DifficultyLevel.Hard,
						"I alcohol therefor I am"
					}
				},
				Position = RenderMenuPosition.Center,
				Margin = 1
			});

			logger.Debug($"User picked {ctx.Player.DifficultyLevel.ToString()}");

			PaintBrush brush = new PaintBrush();

			// Draws Walls for game field
			brush.RenderCanvas(ctx.Square);
			logger.Info($"canvas {ctx.Square.Title} rendered");

			// Draws Walls for score board
			brush.RenderCanvas(ctx.ScoreBoard);
			logger.Info($"canvas {ctx.ScoreBoard.Title} rendered");

			brush.RenderCanvas(ctx.MovementLog);

			// Draws player
			brush.Render(ctx.Square, ctx.Player.Position, Player.BodyCharacter);
			logger.Info($"Player rendered in canvas {ctx.Square.Title}");
		}
	}
}
