using System;
using System.Collections.Generic;
using System.Text;
using GameEngine;
using System.IO;
using System.Drawing;

namespace D3DStrategy
{
	public class Barracks : Structure
	{
		private List<Structure> structureOptions;
		private List<Unit> trainingOptions;
		private static Dictionary<string, GameSprite> animations;

		public Barracks()
		{
			// * Barakker giver ikke mulighed for andre bygninger
			structureOptions = new List<Structure>();
			trainingOptions = new List<Unit>();

			Name = "Barracks";
			
			// * Barakker kan bygge rifle-men.
			// * Man kan eventuelt sætte nogle specielle indstillinger her,
			//   som en slags template for nybyggede enheder
			RifleInfantry templateRifleman = new RifleInfantry();
			templateRifleman.SetSprite("run_down");
			templateRifleman.MoveSpeed = 75; // * Dem vi laver her er altså ultra hurtige ;)
			templateRifleman.AttackPoints = 50;
			templateRifleman.Name = "Rifle infantry";
			templateRifleman.Buildable = true;
			templateRifleman.BuildCost = 150;
			templateRifleman.BuildSpeed = 20; // * tælles 1 ned per gametick
			trainingOptions.Add(templateRifleman);

			IsSelectable = true;

			OccupationMatrix = new bool[2, 2] { 
				{ false, true },
				{ false, true }
			};

			// * Højere z-index end units da units skal animere at de kommer ud af bygninger
			ZIndex = 500;

			// * Vælg standard animation
			LoadSprites(Barracks.Animations);
			SetSprite("make");
		}

		public override List<Structure> GetContructionOptions()
		{
			return structureOptions;
		}

		public override List<Unit> GetTrainingOptions()
		{
			return trainingOptions;
		}

		public override void Animate()
		{
			base.Animate();
		}

		public override Point RallyPoint
		{
			get
			{
				// * Hvis der ikke er sat et rallypoint, så er det bare 2 tiles under positionen
				if(rallyPoint == Point.Empty)
					return new Point(WorldPosition.X, WorldPosition.Y + 2);
				else
					return rallyPoint;
			}
			set { this.rallyPoint = value; }
		}

		public override Point RallyPointStart
		{
			get
			{
				// * Hvis der ikke er sat et rallypoint, så er det bare 2 tiles under positionen
				if(rallyPointStart == Point.Empty)
					return new Point(WorldPosition.X, WorldPosition.Y + 2);
				else
					return rallyPoint;
			}
			set { this.rallyPointStart = value; }
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
			string barracksRoot = GameController.imageRoot + "/barracks";

			GameSprite constructSprite = new GameSprite();
			constructSprite.RepeatCount = 0;
			for(int i = 0; i <= 12; i++)
			{
				string imagePath = barracksRoot + "/make/barrmake " + i + ".png";
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
