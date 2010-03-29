using System;
using System.Collections.Generic;
using System.Text;
using GameEngine;
using System.Drawing;
using System.Windows.Forms;

namespace D3DStrategy
{
	public class StrategyGame : Game
	{
		public override void InitializeResources()
		{
		}

		public List<GameEntity> GetEntities()
		{
			return entities;
		}
	}
}
