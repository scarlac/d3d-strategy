using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GameEngine;

namespace D3DStrategy
{
	public class MapEntity : GameEntity
	{
		private Point mapPosition;

		public MapEntity()
		{
			;
		}

		public MapEntity(Point initialPosition)
		{
			// * Gør noget!
			MapPosition = initialPosition;
		}

		public Point MapPosition
		{
			get { return mapPosition; }
			set { this.mapPosition = value; }
		}
	}
}
