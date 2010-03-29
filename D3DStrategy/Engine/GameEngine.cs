using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectSound;
using DXGraphicsDevice = Microsoft.DirectX.Direct3D.Device;
using DXSoundDevice = Microsoft.DirectX.DirectSound.Device;

namespace GameEngine
{
	public class Game : IDisposable
	{
		public static DXGraphicsDevice renderDevice;
		public static DXSoundDevice soundDevice;
		
		public static Control renderSurface;
		public Size renderResolution;
		public static string imageResourceRoot;
		public static Sprite dxSprite;
		public List<GameEntity> entities;

		private int nextEntityZIndex = 10;
		
		public Game()
		{
			this.entities = new List<GameEntity>();
		}

		public void AddEntity(GameEntity entity)
		{
			this.entities.Add(entity);
			entity.ZIndex = nextEntityZIndex++;
		}

		public void RemoveEntity(GameEntity entity)
		{
			this.entities.Remove(entity);
		}

		public virtual void InternalUpdate()
		{
			foreach(GameEntity entity in entities)
			{
				entity.InternalUpdate();
			}
		}

		public virtual void AnimateAll()
		{
			foreach(GameEntity entity in entities)
			{
				entity.Animate();
			}
		}

		public virtual void TickAll()
		{
			foreach(GameEntity entity in entities)
			{
				entity.Tick();
			}
		}

		// * Bruges til at indlæse diverse eksterne resourcer
		public virtual void InitializeResources()
		{
		}

		public string ImageResourceRoot
		{
			get { return Game.imageResourceRoot; }
			set { Game.imageResourceRoot = value; }
		}

		public void InitializeGraphics()
		{
			PresentParameters presentParms = new PresentParameters();
			presentParms.Windowed = true;
			presentParms.SwapEffect = SwapEffect.Discard;
			
			// * Benyt default adapteren (=0)
			renderDevice = new DXGraphicsDevice(0, DeviceType.Hardware, renderSurface, CreateFlags.SoftwareVertexProcessing, presentParms);

			dxSprite = new Sprite(renderDevice);
		}

		public void InitializeSound()
		{
			soundDevice = new DXSoundDevice();
			soundDevice.SetCooperativeLevel(Game.renderSurface, CooperativeLevel.Normal);
		}

		public void Initialize(Control surface, Size resolution)
		{
			// * Opret et vindue
			renderSurface = surface;
			renderResolution = resolution;

			// * Skift kun opløsning hvis der er angivet en
			renderSurface.ClientSize = renderResolution;

			//renderSurface.Show();
		}

		public virtual void Render()
		{
			renderDevice.Clear(ClearFlags.Target, Color.Black, 1.0F, 0);
			renderDevice.BeginScene();

			foreach(GameEntity entity in entities)
			{
				entity.Render();
			}

			renderDevice.EndScene();
			renderDevice.Present(renderSurface);
		}

		public virtual void Dispose()
		{
			// * Oprydningskode for maskinen
			foreach(GameEntity e in entities)
			{
				e.Dispose();
			}
			
			renderDevice.Dispose();
			dxSprite.Dispose();
		}

	}


}
