using System;
using System.Collections.Generic;
using System.Text;
using GameEngine;
using System.Drawing;

namespace D3DStrategy
{
	// * Abstrakt klasse. En struktur SKAL arves inden den instantsieres.
	// * Der er ikke brug for en "konkret" generisk bygning i selve spillet.
	public abstract class Structure : GameEntity
	{
		protected Point rallyPoint = Point.Empty, rallyPointStart = Point.Empty;
		private string name = null;

		// * Dumme indhold til abstrakt klasse
		public virtual List<Structure> GetContructionOptions()
		{
			return new List<Structure>();
		}

		public virtual List<Unit> GetTrainingOptions()
		{
			return new List<Unit>();
		}

		public virtual string Name
		{
			get { return name; }
			set { this.name = value; }
		}
		
		public virtual Point RallyPoint
		{
			get { 
				// * Hvis der ikke er sat et rallypoint, så er det bare 2 tiles under positionen
				if(rallyPoint == Point.Empty)
					return new Point(WorldPosition.X, WorldPosition.Y + 2);
				else
					return rallyPoint;
			}
			set { this.rallyPoint = value; }
		}

		public virtual Point RallyPointStart
		{
			get { return this.rallyPointStart;  }
			set { this.rallyPointStart = value; }
		}
	}
}
