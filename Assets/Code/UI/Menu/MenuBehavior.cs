using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Rerum.Code.Menu
{
	public class MenuBehavior : MonoBehaviour 
	{
		public void New()
		{
			Application.LoadLevel ("Main");
		}

		public void Quit(){
			Application.Quit();
		}
	}
}