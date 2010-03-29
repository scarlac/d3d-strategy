using System;
using System.Collections.Generic;
using System.Text;
using GameEngine;

namespace D3DStrategy
{
	class GameSounds
	{
		public Dictionary<string, GameSound> soundList;

		public GameSounds()
		{
			soundList = new Dictionary<string, GameSound>();

			string soundRoot = "resources/sounds";

			soundList.Add("awaiting1", new GameSound(soundRoot + "/awaiting1.wav"));
			soundList.Add("awaiting2", new GameSound(soundRoot + "/awaiting2.wav"));
			soundList.Add("awaiting3", new GameSound(soundRoot + "/awaiting3.wav"));
			soundList.Add("awaiting4", new GameSound(soundRoot + "/awaiting4.wav"));

			soundList.Add("affirmative1", new GameSound(soundRoot + "/affirmative1.wav"));
			soundList.Add("affirmative2", new GameSound(soundRoot + "/affirmative2.wav"));
			soundList.Add("affirmative3", new GameSound(soundRoot + "/affirmative3.wav"));
			soundList.Add("affirmative4", new GameSound(soundRoot + "/affirmative4.wav"));

			soundList.Add("minigunner shot", new GameSound(soundRoot + "/minigunner shot.wav"));

			soundList.Add("training", new GameSound(soundRoot + "/train1.wav"));
			soundList.Add("cancelled", new GameSound(soundRoot + "/cancld1.wav"));

			soundList.Add("creditsup", new GameSound(soundRoot + "/cashup1.wav"));
			soundList.Add("creditsdown", new GameSound(soundRoot + "/cashdn1.wav"));
			soundList.Add("insufficientfunds", new GameSound(soundRoot + "/nofunds1.wav"));
			
			soundList.Add("unitlost", new GameSound(soundRoot + "/unitlst1.wav"));
			soundList.Add("unitready", new GameSound(soundRoot + "/unitrdy1.wav"));

			soundList.Add("jeepfire", new GameSound(soundRoot + "/jeepfire.wav"));

			soundList.Add("sfx4", new GameSound(soundRoot + "/sfx4.wav"));
			soundList.Add("build5", new GameSound(soundRoot + "/build5.wav"));
			soundList.Add("placebuilding", new GameSound(soundRoot + "/placbldg.wav"));

			for(int i = 1; i <= 9; i++)
			{
				soundList.Add("man die #" + i, new GameSound(soundRoot + "/man die #" + i + ".wav"));
			}
			
			// c:\source\c#\D3DStrategy\D3DStrategy\resources\sounds

		}

		public void PlaySound(string keyName)
		{
			if(soundList.ContainsKey(keyName))
			{
				soundList[keyName].Play();
			}
			else
			{
				; // * Gør ingen ting eller lav advarsel?
				throw new SoundNotFoundException(keyName);
			}
		}
	}

	class SoundNotFoundException : Exception
	{
		public SoundNotFoundException(string soundName) : base(soundName)
		{

		}
	}
}
