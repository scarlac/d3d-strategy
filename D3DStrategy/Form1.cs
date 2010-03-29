using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GameEngine;
using System.Threading;

namespace D3DStrategy
{
	// * Delegate (en slags call-back) til rendering af spillet, da det foregår på et GUI element
	public delegate void RenderDelegate();

	public partial class Form1 : Form
	{
		private bool shouldRun;
		private Game game;
		private GameController control;

		public Form1()
		{
			InitializeComponent();

		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			shouldRun = false;
			game.Dispose();
		}

		GameEntity hoverTile;
		private void Form1_Load(object sender, EventArgs e)
		{
			game = new Game();

			game.Initialize(panel1, panel1.DisplayRectangle.Size);
			game.InitializeGraphics();
			game.InitializeSound();
			game.InitializeResources();
			game.InternalUpdate();
			
			this.Show();
			gameTickTimer.Enabled = true;
			animationTimer.Enabled = true;

			control = new GameController(game, new GameMap(new Size(32, 32), new Size(24, 24)));

			hoverTile = new GameEntity();
			hoverTile.AddSprite("hover", new GameSprite(new SpriteFrame(new SpriteImage("resources/images/hover_tile.png", Color.Transparent), 1)));
			hoverTile.SetSprite("hover");
			hoverTile.ZIndex = 200; // * Hold den nær jorden ift. infantry som er på 100
			hoverTile.IsSelectable = false;

			RifleInfantry testUnit1 = new RifleInfantry();
			RifleInfantry testUnit2 = new RifleInfantry();
			RifleInfantry testUnit3 = new RifleInfantry();
			RifleInfantry testUnit4 = new RifleInfantry();

			testUnit1.WorldPosition = new Point(2, 2);
			testUnit2.WorldPosition = new Point(4, 1);
			testUnit3.WorldPosition = new Point(4, 2);
			testUnit4.WorldPosition = new Point(4, 3);

			testUnit1.MoveSpeed = 20;
			testUnit2.MoveSpeed = 50;
			testUnit3.MoveSpeed = 50;
			testUnit4.MoveSpeed = 40;

			control.PlaceEntity(testUnit1);
			control.PlaceEntity(testUnit2);
			control.PlaceEntity(testUnit3);
			control.PlaceEntity(testUnit4);

			control.PlaceEntity(hoverTile);

			// * Lav lidt skov
			Random rand = new Random();
			for(int i = 0; i < 15; i++)
			{
				GameEntity tree = new GameEntity();
				
				tree.AddSprite("idle", new GameSprite(new SpriteFrame(new SpriteImage(
					GameController.resourceRoot + "/images/trees/" + rand.Next(1, 6) + ".png", Color.Transparent), 1))
				);
				tree.OccupationMatrix = new bool[2, 2] {
					{ false, true },
					{ false,  false }
				};
				tree.IsSelectable = false;
				tree.SetSprite("idle");
				tree.WorldPosition = new Point(
					rand.Next(0, control.map.GetSize().Width),
					rand.Next(0, control.map.GetSize().Height)
				);

				control.PlaceEntity(tree);
			}
			
			shouldRun = true;
			
			/*
			RenderThread renderThread = new RenderThread(new RenderDelegate(this.RenderGame));
			ThreadStart threadStart = new ThreadStart(renderThread.Start);
			new Thread(threadStart).Start(); // * Start renderingstråden
			*/
		}

		// * Til delegates, hvis ellers det virker
		public void RenderGame()
		{
			control.game.InternalUpdate();
			control.Render();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if(shouldRun)
			{
				control.Tick();
				game.InternalUpdate();
				game.Render();

				lblCredits.Text = "Credits: " + control.Credits;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			game.TickAll();
		}

		private void animationTimer_Tick(object sender, EventArgs e)
		{
			game.AnimateAll();
		}

		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			Point worldCoord = control.GetWorldCoordinate(e.Location);

			if(worldCoord.X < 0 || worldCoord.Y < 0)
				return;
			else if(control.IsPlacingStructure() == true)
			{
				control.PlaceStructure(worldCoord);
			}
			else if(e.Button == MouseButtons.Left)
				control.PrimaryActivate(worldCoord);
			else if(e.Button == MouseButtons.Right)
				control.SecondaryActivate(worldCoord);

			btnRifle.Enabled = control.GetTrainingOptions().Exists(new Predicate<Unit>(IsRifleInfantry));
		}

		private bool IsRifleInfantry(Unit unit)
		{
			return (unit is RifleInfantry);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			control.MoveViewport(-1, 0);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			control.MoveViewport(0, -1);
		}

		private void button5_Click(object sender, EventArgs e)
		{
			control.MoveViewport(0, 1);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			control.MoveViewport(1, 0);
		}

		private void panel1_MouseMove(object sender, MouseEventArgs e)
		{
			Point worldCoord = control.GetWorldCoordinate(e.Location);

			int nudgeSize = 16;

			if(chkNudge.Checked)
			{
				if(e.X < nudgeSize)
					control.MoveViewport(1, 0);
				if(e.Y < nudgeSize)
					control.MoveViewport(0, 1);
				if(panel1.Width - e.X < nudgeSize)
					control.MoveViewport(-1, 0);
				if(panel1.Height - e.Y < nudgeSize)
					control.MoveViewport(0, -1);
			}

			lblCoord.Text = worldCoord.ToString();

			if(	worldCoord.X < 0 || worldCoord.Y < 0 || 
				worldCoord.X > control.map.MapSize.Width - 1 || worldCoord.Y > control.map.MapSize.Height - 1)
			{
				panel1.Cursor = Cursors.No;
				hoverTile.IsVisible = false;
				return;
			}
			else
			{
				hoverTile.IsVisible = true;
			}

			if(control.SelectCount > 0 && control.CanMoveSelected())
			{
				hoverTile.IsVisible = true;
				hoverTile.WorldPosition = worldCoord;
			}
			else
				hoverTile.IsVisible = false;

			bool occupied = control.map.HasEntity(worldCoord);
			GameEntity occupant = control.map.GetOccupant(worldCoord);

			if(occupied == true && occupant.IsSelectable) // * Ingen før-markerede enheder
				panel1.Cursor = Cursors.Hand;
			else if(occupied == true && occupant.IsSelectable == false && control.CanMoveSelected())
			{
				panel1.Cursor = Cursors.No;
				hoverTile.IsVisible = false;
			}
			else if(control.CanMoveSelected() == false)
				panel1.Cursor = Cursors.Arrow;
			else if(occupied == false && control.CanMoveSelected())
				panel1.Cursor = Cursors.UpArrow; // * Flyt enhed
			else if(occupied == true && occupant.IsSelectable && control.CanMoveSelected())
				panel1.Cursor = Cursors.Hand; // * Glem nuværende markering og Marker ny enhed

			if(control.IsPlacingStructure() == true)
			{
				control.MovePlacingStructure(worldCoord);
			}
		}

		private void btnRifle_Click(object sender, EventArgs e)
		{
			List<Unit> units = control.GetTrainingOptions();
			if(units.Count > 0)
			{
				control.TrainUnit("Rifle infantry");
			}
		}

		private void button6_Click(object sender, EventArgs e)
		{
			control.StartPlacing("Barracks");
		}

		private void btnGiveCredits_Click(object sender, EventArgs e)
		{
			control.Credits += 50;
		}

		private void btnRefinery_Click(object sender, EventArgs e)
		{
			control.StartPlacing("Refinery");
		}

		private void btnCancelTraining_Click(object sender, EventArgs e)
		{
			control.CancelTraining();
		}
	}
}