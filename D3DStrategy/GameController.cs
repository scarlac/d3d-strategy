using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GameEngine;
using System.Windows.Forms;

namespace D3DStrategy
{
	class GameController
	{
		public Game game;
		public GameSounds sounds;
		public GameMap map;
		public static string imageRoot = "./resources/images";
		public static string resourceRoot = "./resources";
		private List<GameEntity> selectedEntities = new List<GameEntity>();
		private ZIndexComparer zIndexComparer = new ZIndexComparer();
		private Structure placingStructure = null;
		private int credits = 1000;
		private LinkedList<Unit> trainingUnits = new LinkedList<Unit>();
		private LinkedList<GameEntity> disposeEntities = new LinkedList<GameEntity>();

		public GameController(Game engine, GameMap map)
		{
			this.game = engine;
			this.map = map;
			this.sounds = new GameSounds();

			// * Indl�s animationer for diverse enheder
			RifleInfantry.InitializeGraphics();
			Barracks.InitializeGraphics();
			Refinery.InitializeGraphics();
			Jeep.InitializeGraphics();

			game.AddEntity(new MapBackground(map.GetSize(), map.GetTileSize()));
		}

		public Point GetWorldCoordinate(Point graphicCoordinate)
		{
			return map.GetWorldCoordinate(graphicCoordinate);
		}

		public void MoveViewport(int x, int y)
		{
			map.ViewportPosition = new Point(map.ViewportPosition.X + x, map.ViewportPosition.Y + y);
		}

		public void PlaceEntity(GameEntity entity)
		{
			game.AddEntity(entity);
			map.PlaceEntity(entity.WorldPosition, entity);
		}

		public bool CanMoveSelected()
		{
			if(selectedEntities.Exists(new Predicate<GameEntity>(IsMoveable)))
				return true;
			else
				return false;
		}

		private bool IsUnit(GameEntity e)
		{
			return (e is Unit);
		}

		private bool IsMoveable(GameEntity e)
		{
			return (e is MovableEntity);
		}

		public void StartPlacing(string structureName)
		{
			// * Todo: skal kun kunne v�lges hvis man har nok penge, mv.
			Structure newStructure = null;

			switch(structureName)
			{
				case "Barracks":
					newStructure = new Barracks();
					newStructure.Opacity = 128; // * g�r den alphatransperant, da den ikke er placeret endnu
					placingStructure = newStructure;
					PlaceEntity(placingStructure);
					break;

				case "Refinery":
					newStructure = new Refinery();
					newStructure.Opacity = 128; // * g�r den alphatransperant, da den ikke er placeret endnu
					placingStructure = newStructure;
					PlaceEntity(placingStructure);
					break;
			}
		}

		public void MovePlacingStructure(Point worldPosition)
		{
			if(IsPlacingStructure())
			{
				placingStructure.WorldPosition = worldPosition;
			}
		}

		public bool IsPlacingStructure()
		{
			return (placingStructure != null);
		}

		// * Konstru�r bygningen som brugeren har i 'h�nden'.
		public bool PlaceStructure(Point worldPosition)
		{
			if(map.HasEntity(worldPosition) == false)
			{
				placingStructure.SetSprite("make", true);
				placingStructure.Opacity = 255;
				map.PlaceEntity(worldPosition, placingStructure);
				placingStructure = null;
				PlaySound("placebuilding");
				PlaySound("build5");
				return true;
			}
			else
			{
				return false;
			}
		}

		public void PrimaryActivate(Point worldCoordinate)
		{
			// * Nulstil markeringen i tilf�lde af brugeren ikke rammer en enhed
			selectedEntities.Clear();

			/*
			// * Fors�g at tage et unit
			foreach(GameEntity e in game.entities)
			{
				if(e.WorldPosition == worldCoordinate && e.IsSelectable == true)
				{
					selectedEntities.Add(e);
				}
			}
			*/

			GameEntity e = map.GetOccupant(worldCoordinate);

			if(IsPlacingStructure())
			{
				// * brugeren er ved at placere en bygning
				PlaceStructure(worldCoordinate);
			}
			else if(e != null)
			{
				// * marker enheden
				selectedEntities.Add(e);

				if(e != null)
				{
					// * Afspil ikke lyd for bygninger
					if(selectedEntities[0] is Unit)
						PlaySound("awaiting" + (new Random().Next(1, 4)));
					else if(selectedEntities[0] is Structure)
						PlaySound("sfx4");
				}
			}
		}

		public void SecondaryActivate(Point worldCoordinate)
		{
			// * Hvis der er markeret en enhed f�r, ryk den
			if(selectedEntities.Count > 0)
			{
				if(map.HasEntity(worldCoordinate) == true && selectedEntities.Exists(new Predicate<GameEntity>(IsUnit)))
				{
					GameEntity victim = map.GetOccupant(worldCoordinate);

					double distance = map.GetDistance(selectedEntities[0].WorldPosition, victim.WorldPosition);

					if(selectedEntities[0] is Unit)
					{
						Unit attacker = (Unit)selectedEntities[0];

						// * En enhed kan ikke angribe sig selv
						if(attacker != victim)
						{
							if(distance <= attacker.AttackRange && victim is Unit)
							{
								PlaySound("affirmative" + (new Random().Next(1, 4)));
								attacker.AttackTarget = (Unit)victim;
								attacker.ActionState = Unit.ActionStates.Attacking;
							}
							else
							{
								if(victim is Unit)
									MessageBox.Show("You are too far away. Current/Max distance: " + distance + "/" + attacker.AttackRange);
								else
									MessageBox.Show("Target not attackable");
							}
						}
					}
				}
				else
				{
					PlaySound("affirmative" + (new Random().Next(1, 4)));
					foreach(GameEntity e in selectedEntities)
					{
						// * Husk kun 1 unit pr felt, men flere enheder flyttes her til samme felt :-|
						// * Fors�g at finde vej.
						Stack<Point> path = map.GetPath(e.WorldPosition, worldCoordinate);
						if(path.Count > 0)
						{
							if(e is MovableEntity)
							{
								if(e is Unit)
								{
									((Unit)e).ActionState = Unit.ActionStates.Moving;
									((Unit)e).AttackTarget = null; // * Angrib ikke l�ngere
								}

								MovableEntity te = ((MovableEntity)e);
								te.walkPath = path;
								te.walkDestination = worldCoordinate;
								te.NextPosition = path.Peek();
								//string foo = "";
								//foreach(Point p in path)
								//	foo += " -> " + p;
								//MessageBox.Show(foo);
							}
						}
					}
				}
			}
		}

		public void Render()
		{
			game.Render();
		}

		public int SelectCount
		{
			get { return selectedEntities.Count; }
		}

		public bool IsBarracks(GameEntity e)
		{
			return (e is Barracks);
		}

		public int Credits
		{
			get { return credits; }
			set
			{
				if(value < credits)
					PlaySound("creditsdown");
				else
					PlaySound("creditsup");

				credits = value;
			}

		}

		public void Tick()
		{
			game.entities.Sort(zIndexComparer); // * Sorter efter z-order

			try
			{
				foreach(Unit unit in trainingUnits)
				{
					if(unit.BuildCostLeft == 0)
					{
						// * Todo: udv�lg en primary bygning noget smartere - feks den brugeren byggede fra
						Barracks primaryBarrack = (Barracks)game.entities.Find(new Predicate<GameEntity>(IsBarracks));
						if(primaryBarrack == null)
							continue;

						// * Fors�g at placere enheden i midten af bygningen,
						// * Og f� ham til at l�be (animere) ud ad d�ren - HARDCODED indtil videre
						unit.WorldPosition = new Point(primaryBarrack.WorldPosition.X + 1, primaryBarrack.WorldPosition.Y + 1);
						unit.walkPath = map.GetPath(unit.WorldPosition, primaryBarrack.RallyPoint);

						// * Angiv n�ste position, s� den ved hvor den skal animere hen ad
						unit.NextPosition = unit.walkPath.Peek();
						unit.ActionState = Unit.ActionStates.Moving;

						PlaySound("unitready");
						PlaceEntity(unit);

						trainingUnits.Remove(unit);
					}
					else
					{
						// * Tr�k en delm�ngde af prisen fra credits hvert build (ikke n�dvendigvis game-) tick
						int tickCost = unit.BuildCost / unit.BuildSpeed;

						// * Ofte er sidste tick billigere, men m� ikke betyde at der tr�kkes for meget ifht. hvad der er tilbage
						if(unit.BuildCostLeft - tickCost < 0)
							tickCost = unit.BuildCostLeft; // * Tr�k kun det n�dvendige

						// * Hvis der er r�d
						if(Credits - tickCost >= 0)
						{
							// * Formindsk "prisen tilbage" med den rigtige hastighed
							if(unit.BuildSpeedTicks == 0)
							{
								unit.BuildSpeedTicks = unit.BuildSpeed;
								unit.BuildCostLeft -= tickCost;
								Credits -= tickCost;
							}
							else
							{
								unit.BuildSpeedTicks--;
							}
						}
						else
						{
							// * Todo: Afspil kun lyden �n gang, og s� f�rst 
							PlaySound("insufficientfunds");
						}
					}
				}

				// * Flyt alle enheder
				foreach(GameEntity e in game.entities)
				{
					// * Ryd op i alle entiteter - feks hvis de er f�rdige med en d�-animation
					if(e.IsAnimationDone() == true && e.DisposeWhenDone)
					{
						disposeEntities.AddLast(e);
					}

					PointF graphicsPosition = map.GetGraphicCoordinate(e.WorldPosition);

					if(e is Unit)
					{
						e.GraphicPosition = graphicsPosition;

						// * Hvis enheder er ved at angribe noget/nogen, s� t�l attackSpeedTicks ned og skyd eventuelt
						Unit unit = (Unit)e;
						if(unit.AttackTarget != null &&
							map.GetDistance(unit.WorldPosition, unit.AttackTarget.WorldPosition) <= unit.AttackRange)
						{
							unit.AttackSpeedTicks--;

							// * Nu er den, efter overholdelse af hastigheden for skydning, klar til at skyde!
							//
							// * *BANG*  -   -  - - - - ---> >-x-O
							//
							if(unit.AttackSpeedTicks == 0)
							{
								unit.AttackSpeedTicks = unit.AttackSpeed;

								unit.AttackTarget.HitPoints -= unit.AttackPoints;

								GameEntity gunfire = new GameEntity();
								gunfire.WorldPosition = unit.AttackTarget.WorldPosition;
								// * Her skal den graphicsposition s�ttes manuelt, da det normalt g�res tidligere,
								//   ellers kommer f�rste 
								gunfire.GraphicPosition = map.GetGraphicCoordinate(gunfire.WorldPosition);
								gunfire.AddSprite("piff", RifleInfantry.Animations["piff"]);
								gunfire.SetSprite("piff");
								gunfire.DisposeWhenDone = true;
								game.AddEntity(gunfire);

								PlaySound("minigunner shot");

								unit.SetSprite("attack_" + MovableEntity.GetDirection(unit.WorldPosition, unit.AttackTarget.WorldPosition), true);
								
								if(unit.AttackTarget.HitPoints <= 0)
								{
									PlaySound("man die #" + new Random().Next(1, 10));
									PlaySound("unitlost");
									unit.AttackTarget = null;
									unit.ActionState = Unit.ActionStates.Idle;
								}
							}
							continue;
						}
						else
						{
							// * Hvis ikke en r�kke attack-krav opfyldes, 
							//   jamen s� sikrer vi os lige at enheden ikke angriber af sig selv igen
							unit.AttackTarget = null;
						}

						if(unit.WorldPosition == unit.NextPosition)
						{
							e.GraphicPosition = graphicsPosition;
							continue;
						}

						if(unit.walkPath.Count > 0)
						{
							// * Hastighedsimplementering: Flyt kun enhed hvis t�ller er nul
							unit.MoveTicks--;

							// * 3 scenarier:
							// * Enheden er p� et nyt felt, og worldpos skal �ndres
							// * Enheden er i fart ('mellem to felter') og graphicspos skal �ndres dynamisk
							// * Enheden st�r stille, men dens graphics pos skal s� stadig liiige opdateres, da viewporten kan �ndre sig
							if(unit.MoveTicks <= 0)
							{
								unit.MoveTicks = unit.MoveSpeed;

								Point nextPoint = unit.walkPath.Pop();

								// * Sammenst�d aka. Collision-detection
								if(map.HasEntity(nextPoint))
								{
									// * Sammenst�d! Fors�g at finde ny vej
									unit.walkPath = map.GetPath(unit.WorldPosition, unit.walkDestination);
									// * Todo: Problemer med animation ved dynamisk rutefinding
									continue;
								}

								// * Pessimistisk flytning/"l�sning":
								// * Vi holder p� begge felter s� vi p� aldrig f�r 2 units p� samme felt
								map.RemoveEntity(unit);
								map.PlaceEntity(nextPoint, unit);

								// * Enheden skal flyttes f�r den animerer
								unit.LastPosition = unit.WorldPosition;
								unit.WorldPosition = nextPoint;
								if(unit.walkPath.Count > 0)
								{
									unit.NextPosition = unit.walkPath.Peek();
								}
								else
								{
									// * Enheden er stoppet (ikke n�dvendigvis n�et frem til det, af brugeren, �nskede m�l)
									unit.ActionState = Unit.ActionStates.Idle;
								}

								// * Dette er ikke det gamle koordinat, s� vi genudregner det
								e.GraphicPosition = map.GetGraphicCoordinate(unit.WorldPosition);
							}
							else if(unit.MoveTicks != unit.MoveSpeed) // enheden er ikke helt i "m�l", og skal kun flyttes en del af �t felt
							{
								unit.ActionState = Unit.ActionStates.Moving;
								// * Her er moveSpeedTics implicit st�rre end nul, da f�rste if-blok ikke blev udf�rt
								float offsetX = 0, offsetY = 0;
								if(unit.WorldPosition.X - unit.NextPosition.X != 0)
									offsetX = ((unit.NextPosition.X - unit.WorldPosition.X) / (float)unit.MoveSpeed) * (float)unit.MoveTicks;
								if(unit.WorldPosition.Y - unit.NextPosition.Y != 0)
									offsetY = ((unit.NextPosition.Y - unit.WorldPosition.Y) / (float)unit.MoveSpeed) * (float)unit.MoveTicks;

								// * Udgangspunktet er at vi renderer entiteten i n�ste pos,
								//   men flytter den grafisk tilbage alt efter hvor langt den er n�et (moveSpeedTicks)
								graphicsPosition = map.GetGraphicCoordinate(unit.NextPosition);
								e.GraphicPosition = new PointF(
									graphicsPosition.X - (offsetX * map.GetTileSize().Width),
									graphicsPosition.Y - (offsetY * map.GetTileSize().Height)
								);
							}
							else
							{
								e.GraphicPosition = graphicsPosition;
							}
						}
						else
						{
							e.GraphicPosition = graphicsPosition;
						}
					}
					else
					{
						// * Hvis ikke det er en flytbar enhed, s� skal vi jo bare genoptegne dens position,
						//   da viewport kan �ndre sig
						e.GraphicPosition = graphicsPosition;
					}

					if(e.IsAnimationDone() && e.DisposeWhenDone)
					{
						disposeEntities.AddLast(e);
					}
				}

			}
			catch(InvalidOperationException)
			{
				// * Ignorer denne exception.
				// * Den opst�r fordi jeg v�lger ikke at bruge foreach og samtidigt sletter fra collection'en.
				// * I .NET er samtlige generics /stadig/ ikke thread-safe, desv�rre.
				;
			}

			foreach(GameEntity e in disposeEntities)
			{
				// * Fjern kun hvis han i forvejen optager plads
				if(map.GetOccupant(e.WorldPosition) == e)
					map.RemoveEntity(e);
				game.entities.Remove(e);
				e.Dispose();
			}

			disposeEntities.Clear();
		}

		/// <summary>
		/// Stopper tr�ning af samtlige enheder.
		/// TODO: Mulighed for kun at stoppe tr�ning af specifik enhed
		/// </summary>
		public void CancelTraining()
		{
			PlaySound("cancelled");
			foreach(Unit unit in trainingUnits)
			{
				// * Tilbagebetal hvad det har kostet
				if(unit.BuildCostLeft > 0)
				{
					Credits += unit.BuildCost - unit.BuildCostLeft;
				}
			}
			trainingUnits.Clear();
		}

		/// <summary>
		/// Afspil en lyd udfra dens registrerede navn.
		/// </summary>
		/// <param name="soundName">navn p� lyden</param>
		public void PlaySound(string soundName)
		{
			sounds.PlaySound(soundName);
		}

		/// <summary>
		/// Returnerer en liste af enheder man kan tr�ne p� den nuv�rende markering.
		/// Kan evt. laves om til at v�re global fremfor markeringen.
		/// </summary>
		public List<Unit> GetTrainingOptions()
		{
			if(SelectCount > 0 && selectedEntities[0] is Structure)
				return ((Structure)selectedEntities[0]).GetTrainingOptions();
			else
				return new List<Unit>();
		}

		// * Tr�n enheder ud fra den markerede bygning
		public void TrainUnit(string unitName)
		{
			if(selectedEntities[0] is Structure)
			{
				Structure building = (Structure)selectedEntities[0];
				List<Unit> options = building.GetTrainingOptions();
				foreach(Unit unit in options)
				{
					// * Der matches p� navne af enhederne
					if(unit.Name == unitName)
					{
						// * Om der er nok penge tjekkes f�rst senere ved hvert game tick

						PlaySound("training");

						Unit newUnit = unit.Clone();

						trainingUnits.AddLast(newUnit);
					}
				}
			}
		}

	}
}
