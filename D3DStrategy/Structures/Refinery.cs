using System;
using System.Collections.Generic;
using System.Text;
using GameEngine;
using System.IO;
using System.Drawing;

namespace D3DStrategy
{
	class Refinery : Structure
	{
		private static Dictionary<string, GameSprite> animations;
		
		public Refinery()
		{
			Name = "Refinery";
			
			IsSelectable = true;

			OccupationMatrix = new bool[3, 2] { 
				{ false, true },
				{ true,  true },
				{ false, true }
			};

			// * Vælg standard animation
			LoadSprites(Refinery.Animations);
			SetSprite("make");
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
			string animationRoot = GameController.imageRoot + "/refinery";

			GameSprite constructSprite = new GameSprite();
			constructSprite.RepeatCount = 0;
			for(int i = 0; i <= 9; i++)
			{
				string imagePath = animationRoot + "/make/procmake " + i + ".png";
				if(File.Exists(imagePath))
				{
					SpriteImage spriteImg = new SpriteImage(imagePath, Color.Transparent);
					SpriteFrame spriteFrame = new SpriteFrame(spriteImg, 5);
					constructSprite.AddFrame(spriteFrame);
				}
				else
					throw new FileNotFoundException("404");
			}
			Animations.Add("make", constructSprite);

		}
	}
}
