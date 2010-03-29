using System;
using System.Collections.Generic;
using System.Text;
using GameEngine;
using System.Drawing;
using System.IO;

namespace D3DStrategy
{
	class MapBackground : GameEntity
	{
		public MapBackground(Size mapSize, Size tileSize)
		{
			IsSelectable = false;

			// * Opret en statisk baggrund baseret på de forskellige tiles der er til rådighed
			string tempFile = GameController.imageRoot + "/bg.png";
			if(File.Exists(tempFile) == false)
			{
				Bitmap bgBitmap = new Bitmap(mapSize.Width * tileSize.Width, mapSize.Height * tileSize.Height);
				Graphics graphic = Graphics.FromImage(bgBitmap);

				// * Lav ét stort billede istedet for mange små -> meget bedre performance
				Random rand = new Random();
				int bitmapTileWidth = 24, bitmapTileHeight = 24;
				for(int y = 0; y < mapSize.Height; y++)
				{
					for(int x = 0; x < mapSize.Width; x++)
					{
						graphic.DrawImage(
							new Bitmap(GameController.imageRoot + "/tiles/green/" + rand.Next(1, 16) + ".png"),
							x * tileSize.Width, y * tileSize.Height, bitmapTileWidth, bitmapTileHeight
						);
					
						// * Lav lidt huller i jorden, så det ser federe ud, 1/20 chance per tile
						if(rand.Next(1, 20) == 1)
						{
							string path = String.Format(GameController.imageRoot + "/craters/cr1 {0:000}.png", rand.Next(0, 4));
							graphic.DrawImage(
								new Bitmap(path),
								x * tileSize.Width, y * tileSize.Height, bitmapTileWidth, bitmapTileHeight
							);
						}
					}
				}

				bgBitmap.Save(tempFile, System.Drawing.Imaging.ImageFormat.Png);
			}

			SpriteFrame bgFrame = new SpriteFrame(
				new SpriteImage(tempFile,
				Color.FromArgb(255, 255, 255)),
			1);

			GameSprite bgSprite = new GameSprite();
			bgSprite.AddFrame(bgFrame);

			AddSprite("background", bgSprite);

			SetSprite("background");

			// * Som udgangs er starter et map selvf. i (0, 0)
			this.WorldPosition = new Point(0, 0);

			this.ZIndex = Int32.MinValue; // * Forsøg altid at ligge bagerst
		}

	}
}
