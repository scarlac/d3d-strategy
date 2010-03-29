using System;
using System.Collections.Generic;
using System.Text;
using GameEngine;
using System.Drawing;

namespace D3DStrategy
{
	class MapTile : GameEntity
	{
		public static GameSprite tilesprite = new GameSprite(new SpriteFrame(new SpriteImage("resources/images/grass_1.bmp", Color.White), 0));

		public MapTile()
		{
			this.AddSprite("tile", MapTile.tilesprite);
		}

		public override void Render()
		{
			base.Render();
		}

		public override void Animate()
		{
			//base.Animate(); // * ingen animation

		}
	}
}
