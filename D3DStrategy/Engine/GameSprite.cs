using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace GameEngine
{
	public class GameSprite
	{
		private List<SpriteFrame> frames;
		private int currentFrameIndex;
		private bool shouldPlay;
		private int delay;
		private int repeatCount;
		private int repeatInfinite = -1;
		private bool donePlaying;

		public GameSprite()
		{
			RepeatCount = repeatInfinite; // * Uendelig gentagelse
			shouldPlay = true;
			donePlaying = false;
			currentFrameIndex = 0;
			frames = new List<SpriteFrame>();
		}

		public GameSprite(SpriteFrame initialFrame)
		{
			RepeatCount = repeatInfinite; // * Uendelig gentagelse
			shouldPlay = true;
			donePlaying = false;
			currentFrameIndex = 0;
			frames = new List<SpriteFrame>();
			AddFrame(initialFrame);
		}

		public GameSprite Clone()
		{
			return (GameSprite)this.MemberwiseClone();
		}

		public void AddFrame(SpriteFrame frame)
		{
			this.frames.Add(frame);
		}

		public void Animate()
		{
			if(shouldPlay)
			{
				SpriteFrame currentFrame = frames[currentFrameIndex];
				
				if(delay > 0)
					delay--;
				
				// * Hvis tiden er udløbet for framet, så skift til næste
				if(delay == 0)
				{
					if(currentFrameIndex == frames.Count - 1)
					{
						if(donePlaying == false && (RepeatCount > 0 || RepeatCount == repeatInfinite))
						{
							if(RepeatCount != repeatInfinite)
								RepeatCount--;
							currentFrameIndex = 0;
						}
						else
							donePlaying = true;
					}
					else
					{
						delay = currentFrame.delay;
						currentFrameIndex++;
					}
				}
			}
		}

		public Texture GetCurrentTexture()
		{
			return frames[currentFrameIndex].spriteImage.GetTexture();
		}

		public void Play()
		{
			shouldPlay = true;
		}

		public void Pause()
		{
			shouldPlay = false;
		}

		public void Stop()
		{
			shouldPlay = false;
			currentFrameIndex = 0; // * Nulstil position
		}

		public int RepeatCount
		{
			get { return this.repeatCount; }
			set { this.repeatCount = value; }
		}

		public bool IsDonePlaying()
		{
			return (donePlaying);
		}
	}
}
