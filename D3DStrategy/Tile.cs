using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace D3DStrategy
{
	public class Tile : IComparable
	{
		public Point location;
		public Tile parent; // * Kan ikke benytte structs pga denne selv-refernece
		public int H; // * "heuristic" - et gæt på hvor langt fra målet vi er
		public int G; // * værdi der angivet hvor tæt vi er på udgangspunktet
		public Tile(int x, int y)
		{
			this.location = new Point(x, y);
		}

		public override bool Equals(object obj)
		{
			Tile other = ((Tile)obj);
			if(other.location.X == this.location.X && other.location.Y == this.location.Y)
				return true;
			else
				return false;
		}

		// * Skal overrides, ellers fremkommer warning.
		public override int GetHashCode()
		{
			// * Forsøg at lave et unikt tal ud af hvad vi har
			return Convert.ToInt32(location.X + "" + location.Y);
		}

		// * Sorter på "F" (som er G + H)
		public int CompareTo(object obj)
		{
			Tile other = ((Tile)obj);
			if((G + H) > (other.G + other.H))
				return 1;
			else if((G + H) < (other.G + other.H))
				return -1;
			else
				return 0;
		}

		public override string ToString()
		{
			return location.ToString();
		}
	}
}
