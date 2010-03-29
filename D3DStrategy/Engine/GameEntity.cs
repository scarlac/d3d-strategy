using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace GameEngine
{
	public class ZIndexComparer : IComparer<GameEntity>
	{
		public int Compare(GameEntity one, GameEntity other)
		{
			if(one.ZIndex > other.ZIndex)
				return 1;
			else if(one.ZIndex < other.ZIndex)
				return -1;
			else
				return 0;
		}
	}

	public class GameEntity : IDisposable
	{
		// * Dictionary er en Hashtable med mulighed for generics.
		// * Key bruges til at genfinde animationer ved et navn, feks: run_left
		private Dictionary<string, GameSprite> sprites;
		protected GameSprite currentSprite;
		private PointF graphicPosition;
		private Point worldPosition;
		private Matrix localMatrix;
		private int zIndex;
		protected bool selectable;
		private int opacity;
		private bool isVisible;
		private bool disposeWhenDone; // * Skal entiteten slettes når animeringen er færdig? feks til gunfire animationer
		private bool[,] occupationMatrix;

		public GameEntity()
		{
			sprites = new Dictionary<string, GameSprite>();
			graphicPosition = new PointF(0, 0);
			worldPosition = new Point(0, 0);
			currentSprite = null; // * Hver entitet kan indeholde flere sprites, som hver især kan holde flere frames, hvilken sprite bruges?
			localMatrix = Matrix.Identity;
			zIndex = 100; // * Her defineres blot et tal højere end nul... det /bør/ ændres af subklasser, osv.
			selectable = true;
			isVisible = true;
			opacity = 255; // * Gennemsigtighed af entiteten
			occupationMatrix = new bool[1, 1] { { true } };
		}

		// * Bruges til at markere områder på kortet som optaget, da bygninger kan være ret store
		// * Matricen fortolkes sådan som den lagres,
		// * Dvs. man visuelt skal dreje hovedet 90 grader mod uret for at se mønstret
		public virtual bool[,] OccupationMatrix
		{
			get { return occupationMatrix; }
			set { this.occupationMatrix = value; }
		}

		public int ZIndex
		{
			get { return this.zIndex; }
			set { this.zIndex = value; }
		}

		public void InternalUpdate()
		{
			// * Oversæt 3-d matricer vores interne 2-d koordinater,
			//   da directX kun forstår 3d (groft set)
			Matrix translatedMatrix = Matrix.Translation(graphicPosition.X, graphicPosition.Y, 0);
			localMatrix = translatedMatrix;
		}

		// * Til animationer - som sker uafhængigt af game ticks
		public virtual void Animate()
		{
			if(currentSprite != null)
				currentSprite.Animate();
			// * Ellers: gør intet. Det skal være muligt at lave sprites entiteter uden sprites
			//else
			//	throw new SpriteNotFoundException("Sprite index " + currentSpriteIndex + " not found");
		}

		// * Til game events (også kaldet game ticks) - som sker uafhængigt af animationer
		public virtual void Tick()
		{
		}

		public int Opacity
		{
			get { return opacity; }
			set { this.opacity = value; }
		}

		/// <summary>
		/// Renders the entity's sprite
		/// </summary>
		public virtual void Render()
		{
			if(isVisible == false)
				return;
			if(currentSprite != null && currentSprite.GetCurrentTexture() != null)
			{
				Game.dxSprite.Begin(SpriteFlags.AlphaBlend);
				
				Game.renderDevice.Transform.World = localMatrix;
				Game.dxSprite.Draw2D(
					currentSprite.GetCurrentTexture(),
					new Point(0, 0), 0, new Point(0, 0),
					Color.FromArgb(opacity, Color.Transparent));

				Game.dxSprite.End();
			}
		}

		/// <summary>
		/// Indlæs og KOPIER en liste af sprite referencer.
		/// </summary>
		/// <param name="sprites"></param>
		public void LoadSprites(Dictionary<string, GameSprite> sprites)
		{
			// * Hvis ikke spritesne kopieres vil alle enheder bruge samme sprites,
			//   og alle animationer vil derfor med samme delay ticker. (ex: 2 mænd løber -> løbeanimation sker dobbelt så hurtigt)
			Dictionary<string, GameSprite> copy = new Dictionary<string, GameSprite>();
			foreach(KeyValuePair<string, GameSprite> pair in sprites)
			{
				copy.Add(pair.Key, pair.Value.Clone());
			}
			this.sprites = copy;
		}

		/// <summary>
		/// Angiver den aktive sprite udfra et navn, og genstarter evt. animationen
		/// </summary>
		/// <param name="spriteName"></param>
		/// <param name="restartAnimation"></param>
		public void SetSprite(string spriteName, bool restartAnimation)
		{
			if(this.sprites.ContainsKey(spriteName))
			{
				SetSprite(spriteName);
				currentSprite.Stop(); // * Stop og hop til start
				currentSprite.Play();
			}
			else
				throw new SpriteNotFoundException("Sprite not found with name: " + spriteName);
		}

		/// <summary>
		/// Angiver den aktive sprite udfra et navn (som der renderes udfra)
		/// </summary>
		/// <param name="spriteName">Opslagsnavn på spriten, som tidligere er tilføjet vha. AddSprite()</param>
		public void SetSprite(string spriteName)
		{
			if(this.sprites.ContainsKey(spriteName))
				this.currentSprite = this.sprites[spriteName];
			else
				throw new SpriteNotFoundException("Sprite not found with name: " + spriteName);
		}

		/// <summary>
		/// Tilføjer en kopi af den angivne sprite til entiteten. Spriten kan herefter bruges vha. SetSprite(spriteName)
		/// </summary>
		/// <param name="spriteName">Opslagsnavn på spriten</param>
		/// <param name="sprite">Reference til den sprite der skal kopieres</param>
		public void AddSprite(string spriteName, GameSprite sprite)
		{
			this.sprites.Add(spriteName, sprite.Clone());
		}

		public void RemoveSprite(string spriteName)
		{
			this.sprites.Remove(spriteName);
		}

		public Point WorldPosition
		{
			get { return worldPosition; }
			set { this.worldPosition = value; }
		}

		public PointF GraphicPosition
		{
			get { return graphicPosition; }
			set { this.graphicPosition = value; }
		}

		public virtual void Dispose()
		{
			// * eventuel oprydning af diverse sprites
		}

		public virtual bool IsSelectable
		{
			get { return this.selectable; }
			set { this.selectable = value; }
		}

		public bool IsVisible
		{
			get { return isVisible; }
			set { this.isVisible = value; }
		}

		public bool IsAnimationDone()
		{
			return currentSprite.IsDonePlaying();
		}

		public bool DisposeWhenDone
		{
			get { return disposeWhenDone; }
			set { this.disposeWhenDone = value; }
		}


	}

	class SpriteNotFoundException : Exception
	{
		public SpriteNotFoundException(string message)
			: base(message)
		{
		}
	}
}
