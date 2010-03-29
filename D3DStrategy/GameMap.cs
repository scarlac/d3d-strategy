using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GameEngine;

namespace D3DStrategy
{
	public class GameMap
	{
		private Size tileSize;
		private Size mapSize;
		private Point viewportPosition; // * Nuværende synlige områdes position ifht. mapkoordinat (0, 0)
		private GameEntity[,] occupiedMap;

		public GameMap(Size mapSize, Size tileSize)
		{
			this.tileSize = tileSize;
			this.mapSize = mapSize;
			this.occupiedMap = new GameEntity[mapSize.Height, mapSize.Width];
		}

		public Size GetSize()
		{
			return mapSize;
		}

		public Size GetTileSize()
		{
			return tileSize;
		}

		public Size MapSize
		{
			get { return mapSize; }
		}

		public Point ViewportPosition
		{
			get { return viewportPosition; }
			set { this.viewportPosition = value; }
		}

		public Point GetWorldCoordinate(PointF graphicCoordinate)
		{
			Point worldCoordinate;
			// * graphicX = worldX * tileWidth
			// * worldX = graphicX / tileWidth

			worldCoordinate = new Point(
				((int)graphicCoordinate.X / tileSize.Width) - viewportPosition.X,
				((int)graphicCoordinate.Y / tileSize.Height) - viewportPosition.Y
			);

			return worldCoordinate;
		}

		public PointF GetGraphicCoordinate(Point worldCoordinate)
		{
			PointF graphicCoordinate;

			graphicCoordinate = new PointF(
				(worldCoordinate.X + viewportPosition.X) * tileSize.Width,
				(worldCoordinate.Y + viewportPosition.Y) * tileSize.Height
			);

			return graphicCoordinate;
		}

		public void RemoveEntity(GameEntity occupant)
		{
			bool[,] matrix = occupant.OccupationMatrix;
			for(int y = 0; y < matrix.GetLength(1); y++)
			{
				for(int x = 0; x < matrix.GetLength(0); x++)
				{
					// * Todo: Hvis man lægger en bygning oven på en mand (og omvendt) forsvinder han så
					occupiedMap[occupant.WorldPosition.X + x, occupant.WorldPosition.Y + y] = null;
				}
			}
		}

		public void PlaceEntity(Point position, GameEntity occupant)
		{
			bool[,] matrix = occupant.OccupationMatrix;
			for(int y = 0; y < matrix.GetLength(1); y++)
			{
				for(int x = 0; x < matrix.GetLength(0); x++)
				{
					// * Todo: Hvis man lægger en bygning oven på en mand (og omvendt) forsvinder han så
					if(matrix[x, y] == true)
					{
						// * Tjek lige at matricen ligger inden for kortet
						if(position.X + x < mapSize.Width && position.Y + y < mapSize.Height)
							occupiedMap[position.X + x, position.Y + y] = occupant;
					}
				}
			}
		}

		public bool HasEntity(Point position)
		{
			return occupiedMap[position.X, position.Y] != null;
		}

		public GameEntity GetOccupant(Point position)
		{
			if(position.X < mapSize.Width && position.Y < mapSize.Height)
				return occupiedMap[position.X, position.Y];
			else
				return null;
		}

		public double GetDistance(Point p1, Point p2)
		{
			// * For at finde afstanden benyttes pythagoras læresætning (igen ;)):
			// * I en retvinklet trekant er kateten (c) lig: c^2 = a^2 + b^2
			//   Her er de to sider nemme at finde ved at trække koordinaterne fra hinanden
			int distanceX = p1.X - p2.X;
			int distanceY = p1.Y - p2.Y;

			// * c = kvadratroden af(a^2 + b^2)
			return Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
		}

		// * A-star algoritme til at finde vej
		public Stack<Point> GetPath(Point origin, Point destination)
		{
			// * Åbne muligheder: En liste over felter der er mulige veje til målet
			List<Tile> openList = new List<Tile>();
			// * Lukkede muligheder: En liste over felter der ikke kan betale sig at benytte
			List<Tile> closedList = new List<Tile>();
			Tile destinationTile = null;

			// * Vi begynder i udgangspunktet.
			openList.Add(new Tile(origin.X, origin.Y));

			Tile current = null;
			// * Tjek alle åbne muligheder
			while(openList.Count > 0)
			{
				// * Kig på det felt med lavest F(=G+H) værdi
				openList.Sort();

				current = openList[0];
				openList.RemoveAt(0);
				closedList.Add(current);

				if(current.location == destination)
					break;

				// * Vi har 8 retninger, dette kunne udvides til at gennemløbe naboer i en graf
				for(int lookY = -1; lookY <= 1; lookY++)
				{
					for(int lookX = -1; lookX <= 1; lookX++)
					{
						// * Vi kigger ikke på samme tile, det vil give en uendelig løkke
						if(lookX == 0 && lookY == 0)
							continue;

						Tile neighbour = new Tile(
							current.location.X + lookX,
							current.location.Y + lookY);

						// * Vi har fundet vores destination - gå nu tilbage gennem stien
						if(neighbour.location == destination)
						{
							destinationTile = neighbour;
						}

						// * Kig kun inden for kortets grænser
						if(neighbour.location.X < 0 || neighbour.location.X > mapSize.Width - 1)
							continue;
						if(neighbour.location.Y < 0 || neighbour.location.Y > mapSize.Height - 1)
							continue;

						// * Hvis der ikke er en væg eller andre ting (og hvis den ikke er der i forvejen)
						//   så tilføjes feltet til åbne muligheder-listen og forældren sættes til nuværende felt
						if(HasEntity(neighbour.location) == false &&
							closedList.Contains(neighbour) == false &&
							openList.Contains(neighbour) == false)
						{
							// * Uh! _mulig_, den ryger i åbne muligheder-listen
							openList.Add(neighbour);

							// * Vi skal huske hvor vi kom fra
							neighbour.parent = current;

							// * Hvad koster det at gå til dette nabofelt?
							// * Det koster 10 for lige-nabo felter og 14 for diagonal-nabo felter
							// * 14 kommer af pythagaros læresætning; diagonalen i en retvinklet trekant har længden:
							// * c^2 = a^2 + b^2, hvor b og c er de rette sider og a er diagonalen
							//   c = sqrt(a^2 + b^2)
							// * dvs. at hvis den normale pris er 10, så er den diagonale pris: sqrt(100 + 100) ~= 14,14
							//   og her arbejder vi med heltal for at få bedre ydeevne, så det er 14.
							// * Generelt kan man gange lige felters pris med kvadratroden af 2 (1,4142135623730950488016887242097)
							//   for at få den diagonale pris

							int directionCost;
							// * Hvis begge retninger er modificeret,
							//   så kigges der diagonalt i en eller anden retning
							if(lookX != 0 && lookY != 0)
								directionCost = 14; // * diagonal pris
							else
								directionCost = 10; // * lige (el. "retvinklet") pris

							neighbour.G = directionCost + current.G;

							// * Jeg bruger "Manhattan" gæt-algoritmen til H, da den er simpel og hurtig
							// * Absolutte værdier, da det ikke på nogen måde kan blive billigere at flytte sig ;)
							neighbour.H = (
								Math.Abs(neighbour.location.X - destination.X) +
								Math.Abs(neighbour.location.Y - destination.Y)
							);
						}
						else if(openList.Contains(neighbour) == true)
						{
							// * Allerede på åbne muligheder-listen, 
							//   så vi tjekker om den vej i stedet er billigere?
							if(neighbour.G < current.G)
							{
								// * Vi har fundet en hurtigere vej til det punkt
								neighbour.parent = current;
							}
						}
					}
				}

			}

			Stack<Point> path = new Stack<Point>();
			if(destinationTile != null)
			{
				current = destinationTile;
				while(current != null && current.location != origin)
				{
					path.Push(current.location);
					current = current.parent;
				}
			}

			return path;
		}
	}
}
