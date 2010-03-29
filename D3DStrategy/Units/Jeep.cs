using System;
using System.Collections.Generic;
using System.Text;
using GameEngine;
using System.IO;
using System.Drawing;

namespace D3DStrategy
{
	class Jeep : Unit
	{
		protected static Dictionary<string, GameSprite> animations;

		public Jeep()
		{
			HitPoints = 300;
			AttackPoints = 40;
			AttackRange = 5;
			AttackSpeed = 40; // * Kan her tænkes som 10ms (game tick speed) * 50, dvs.: 500 ms.
			Name = "Jeep";

			// * Hent (og kopier) alle sprites fra rifleinfantry klassen, så der er noget at vælge mellem
			LoadSprites(Jeep.Animations);
			// * Benyt run_right som standard animation/udgangspunkt
			SetSprite("drive_up-upright");
		}
		
		public override void Animate()
		{
			if(walkPath != null && walkPath.Count > 0)
			{
				string originDirection = MovableEntity.GetDirection(WorldPosition, walkPath.Peek());
				string destinationDirection = MovableEntity.GetDirection(LastPosition, walkPath.Peek());

				//SetSprite("drive_" + originDirection + "-" + destinationDirection);

				// * Benyt alm. animering
				base.Animate();
			}
			else
			{
				SetSprite("drive_up-upright");

				base.Animate();
			}
		}

		public static Dictionary<string, GameSprite> Animations
		{
			get { return animations; }
			set { animations = value; }
		}

		// * Indlæser diverse animationer STATISK (kaldes af GameController) så de ikke genindlæses i hver instans
		public static void InitializeGraphics()
		{
			Animations = new Dictionary<string, GameSprite>();
			string imageRoot = GameController.imageRoot + "/jeep";

			// * Flydende bevægelsesanimationer for en jeep
			List<string> directionDirs = new List<string>(8);
			directionDirs.Add("up-upright");
			directionDirs.Add("upright-right");
			directionDirs.Add("right-downright");
			directionDirs.Add("downright-down");
			directionDirs.Add("down-downleft");
			directionDirs.Add("downleft-left");
			directionDirs.Add("left-upleft");
			directionDirs.Add("upleft-up");

			foreach(string direction in directionDirs)
			{
				GameSprite moveSprite = new GameSprite();
				List<GameSprite> sprites = new List<GameSprite>();
				for(int i = 1; i <= 5; i++)
				{
					string imagePath = imageRoot + "/" + direction + "/jeep " + i + ".png";
					if(File.Exists(imagePath))
					{
						SpriteImage spriteImg = new SpriteImage(imagePath, Color.Transparent);
						SpriteFrame spriteFrame = new SpriteFrame(spriteImg, 5);
						moveSprite.AddFrame(spriteFrame);
					}
					else
						throw new FileNotFoundException("404 on " + imagePath);
				}
				Animations.Add("drive_" + direction, moveSprite);
			}
		}

	}
}
