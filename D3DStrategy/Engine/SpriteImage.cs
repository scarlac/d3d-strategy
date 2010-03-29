using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace GameEngine
{
	public class SpriteImage : IDisposable
	{
		private Texture texture;
		private Size size;

		public SpriteImage(string fileName, Color alfaColor)
		{
			ImageInformation imageInfo = TextureLoader.ImageInformationFromFile(fileName);
			
			this.size = new Size(imageInfo.Width, imageInfo.Height);

			this.texture = TextureLoader.FromFile(
				Game.renderDevice, fileName,
				0, 0, 1, 
				Usage.None, Format.Unknown, Pool.Managed, 
				Filter.None, Filter.None, alfaColor.ToArgb()
			);
		}

		public Texture GetTexture()
		{
			return texture;
		}

		public void Dispose()
		{
			// * Ryd op
			this.texture.Dispose();
		}
	}
}
