using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.DirectSound;
//using Microsoft.DirectX.DirectSound.Device;

namespace GameEngine
{
	class GameSound
	{
		private SecondaryBuffer secondaryBuffer;
		string fileName;

		public GameSound(string file)
		{
			this.fileName = file;
			BufferDescription description = new BufferDescription();
			description.StaticBuffer = true;
			secondaryBuffer = new SecondaryBuffer(fileName, description, Game.soundDevice);
		}

		public void Play()
		{
			secondaryBuffer.Play(0, BufferPlayFlags.Default);
		}

	}
}
