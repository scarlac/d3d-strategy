using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GameEngine;
using System.Windows.Forms;

namespace D3DStrategy
{
	public abstract class MovableEntity : GameEntity
	{
		private Point p = new Point(3, 3);
		public Stack<Point> walkPath;
		public Point walkDestination;
		private Point lastPosition, nextPosition;
		private int moveSpeed; // * Kunne ogs� kedde moveDelay, da det faktisk er angivelse af et delay
		private int moveSpeedTicks; // * Tick ned-t�ller. N�r den rammer 0 flyttes enheden
		
		public MovableEntity()
		{
			walkPath = new Stack<Point>();
			moveSpeed = 10; // * Hurtigst (lavest tick delay) - faktisk hasighed h�nger sammen med gameTicks per sekundt
			moveSpeedTicks = moveSpeed; // * 
			lastPosition = Point.Empty;
			nextPosition = Point.Empty;
			walkDestination = Point.Empty;
		}

		public Point LastPosition
		{
			get { return this.lastPosition; }
			set { this.lastPosition = value; }
		}

		public Point NextPosition
		{
			get { return this.nextPosition; }
			set { this.nextPosition = value; }
		}
		
		public int MoveSpeed
		{
			get { return this.moveSpeed; }
			set {
				this.moveSpeed = value;
				this.moveSpeedTicks = this.moveSpeed;
			}
		}

		public int MoveTicks
		{
			get { return this.moveSpeedTicks; }
			set { this.moveSpeedTicks = value; }
		}

		public override void Tick()
		{
			base.Animate();
		}

		public static string GetDirection(Point relativeTo, Point target)
		{
			if(target.Y < relativeTo.Y && target.X == relativeTo.X)
				return "up"; // * Op
			else if(target.Y < relativeTo.Y && target.X > relativeTo.X)
				return "upright"; // * Op+h�jre
			else if(target.Y == relativeTo.Y && target.X > relativeTo.X)
				return "right"; // * H�jre
			else if(target.Y > relativeTo.Y && target.X > relativeTo.X)
				return "downright"; // * Ned+h�jre
			else if(target.Y > relativeTo.Y && target.X == relativeTo.X)
				return "down"; // * Ned
			else if(target.Y > relativeTo.Y && target.X < relativeTo.X)
				return "downleft"; // * Ned+venstre
			else if(target.Y == relativeTo.Y && target.X < relativeTo.X)
				return "left"; // * Venstre
			else if(target.Y < relativeTo.Y && target.X < relativeTo.X)
				return "upleft"; // * Op+venstre
			else if(target.Y == relativeTo.Y && target.X == relativeTo.X)
				return "center"; // * Op+venstre
			return null; // * Burde ikke ske
		}
	}
}
