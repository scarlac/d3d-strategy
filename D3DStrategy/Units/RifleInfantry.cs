using System;
using System.Collections.Generic;
using System.Text;
using GameEngine;
using System.IO;
using System.Drawing;

namespace D3DStrategy
{
	public class RifleInfantry : Unit
	{
		protected static Dictionary<string, GameSprite> animations;

		public RifleInfantry()
		{
			HitPoints = 100;
			AttackPoints = 15;
			AttackRange = 5;
			AttackSpeed = 75; // * Kan her tænkes som 10ms (game tick speed) * 50, dvs.: 500 ms.
			Name = "Rifle infantry";

			// * Hent (og kopier) alle sprites fra rifleinfantry klassen, så der er noget at vælge mellem
			LoadSprites(RifleInfantry.Animations);
			// * Benyt run_right som standard animation/udgangspunkt
			SetSprite("stand_right");
		}

		public override void Animate()
		{
			if(HitPoints <= 0)
			{
				SetSprite("die_right");
				DisposeWhenDone = true;

				//if(base.currentSprite.IsDonePlaying())
				//	ReadyForDispose = true;

				base.Animate();
			}
			else if(ActionState == ActionStates.Idle)
			{
				SetSprite("stand_right");

				base.Animate();
			}
			else if(walkPath != null && walkPath.Count > 0 && ActionState == ActionStates.Moving)
			{
				SetSprite("run_" + MovableEntity.GetDirection(WorldPosition, walkPath.Peek()));

				// * Benyt alm. animering
				base.Animate();
			}
			else
			{
				//SetSprite("stand_right");

				base.Animate();

				// * Evt. fiddle/kede sig/stå animation
				//base.Animate();
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
			string rifleRoot = GameController.imageRoot + "/rifle_infantry";

			List<string> directionDirs = new List<string>(8);
			directionDirs.Add("up");
			directionDirs.Add("upright");
			directionDirs.Add("right");
			directionDirs.Add("downright");
			directionDirs.Add("down");
			directionDirs.Add("downleft");
			directionDirs.Add("left");
			directionDirs.Add("upleft");

			foreach(string direction in directionDirs)
			{
				GameSprite runSprite = new GameSprite();
				List<GameSprite> sprites = new List<GameSprite>();
				for(int i = 1; i <= 6; i++)
				{
					string imagePath = rifleRoot + "/run/" + direction + "/" + i + ".png";
					if(File.Exists(imagePath))
					{
						SpriteImage spriteImg = new SpriteImage(imagePath, Color.Transparent);
						SpriteFrame spriteFrame = new SpriteFrame(spriteImg, 5);
						runSprite.AddFrame(spriteFrame);
					}
					else
						throw new FileNotFoundException("404 on " + imagePath);
				}
				Animations.Add("run_" + direction, runSprite);
			}

			GameSprite dieSprite = new GameSprite();
			// * Gentag ikke, afspil kun død én gang,
			// * hvorefter IsDonePlaying angiver om vi officielt kan slette enheden
			dieSprite.RepeatCount = 0;
			for(int i = 1; i <= 7; i++)
			{
				string imagePath = rifleRoot + "/die/right/" + i + ".png";
				if(File.Exists(imagePath))
				{
					SpriteImage spriteImg = new SpriteImage(imagePath, Color.Transparent);
					SpriteFrame spriteFrame = new SpriteFrame(spriteImg, 5);
					dieSprite.AddFrame(spriteFrame);
				}
				else
					throw new FileNotFoundException("404 on " + imagePath);
			}
			Animations.Add("die_right", dieSprite);

			GameSprite standSprite = new GameSprite();
			for(int i = 1; i <= 1; i++)
			{
				string imagePath = rifleRoot + "/stand/right.png";
				if(File.Exists(imagePath))
				{
					SpriteImage spriteImg = new SpriteImage(imagePath, Color.Transparent);
					SpriteFrame spriteFrame = new SpriteFrame(spriteImg, 5);
					standSprite.AddFrame(spriteFrame);
				}
				else
					throw new FileNotFoundException("404 on " + imagePath);
			}
			Animations.Add("stand_right", standSprite);

			foreach(string direction in directionDirs)
			{
				GameSprite attackSprite = new GameSprite();
				attackSprite.RepeatCount = 0;
				for(int i = 1; i <= 8; i++)
				{
					string imagePath = rifleRoot + "/attack/" + direction + "/" + i + ".png";
					if(File.Exists(imagePath))
					{
						SpriteImage spriteImg = new SpriteImage(imagePath, Color.Transparent);
						SpriteFrame spriteFrame = new SpriteFrame(spriteImg, 5);
						attackSprite.AddFrame(spriteFrame);
					}
					else
						throw new FileNotFoundException("404 on " + imagePath);
				}
				Animations.Add("attack_" + direction, attackSprite);
			}

			GameSprite piffSprite = new GameSprite();
			piffSprite.RepeatCount = 0;
			for(int i = 0; i <= 3; i++)
			{
				string imagePath = GameController.resourceRoot + "/images/gunfire/piffpiff " + i + ".png";
				if(File.Exists(imagePath))
				{
					SpriteImage spriteImg = new SpriteImage(imagePath, Color.Transparent);
					SpriteFrame spriteFrame = new SpriteFrame(spriteImg, 5);
					piffSprite.AddFrame(spriteFrame);
				}
				else
					throw new FileNotFoundException("404 on " + imagePath);
			}
			Animations.Add("piff", piffSprite);

		}
	}
}
