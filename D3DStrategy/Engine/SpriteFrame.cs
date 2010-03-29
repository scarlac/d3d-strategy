using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GameEngine
{
	public struct SpriteFrame
	{
		public SpriteImage spriteImage;
		public int delay;

		public SpriteFrame(SpriteImage image, int delay)
		{
			this.spriteImage = image;
			this.delay = delay;
		}
	}
}
