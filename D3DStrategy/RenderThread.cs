using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace D3DStrategy
{
	// * Modsat Java, skal man ikke arve eller implementere en tr�d.
	// * For at bruge tr�de skal man (/kan man bl.a.) lave en delegate metode, oprette et generisk tr�dobjekt
	//   og sige hvilken delegate der er start-metoden (her er det "Start()")
	// * I .NET Framework 2.0 er det ikke n�dvendigt explicit at lave en delegate til det
	class RenderThread
	{
		private bool shouldRender;
		private RenderDelegate renderCallback;

		public RenderThread(RenderDelegate callback)
		{
			this.renderCallback = callback;
			shouldRender = true;
		}

		public void Stop()
		{
			this.shouldRender = false;
		}

		// * Denne metode v�lges som startmetode for tr�den.
		public void Start()
		{
			while(shouldRender)
			{
				renderCallback();
			}
		}

	}
}
