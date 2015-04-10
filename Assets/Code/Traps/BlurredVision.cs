using UnityEngine;
using System.Collections;

public class BlurredVision : Trap {
	Color Dark;

	void OnTriggerEnter(Collider other){
		if (this.isTrapEnabled == true) {
			if (other.gameObject == player) {
				MakeDark();
				StartCoroutine("ComeBackToNormal");
			}
		}
	}

	void MakeDark(){
		//RenderSettings.ambientLight = ConvertColor (0, 0, 0);;
		GameObject.Find ("Flashlight").GetComponent<Light>().range /= 2f;
	}

	void ReturnVision(){
		//RenderSettings.ambientLight = ConvertColor (51, 51, 51);
		GameObject.Find ("Flashlight").GetComponent<Light>().range *= 2f;
	}

	Color ConvertColor(int r, int g, int b){
		Color c  = new Color(r/255.0f, g/255.0f, b/255.0f);
		return c;
	}

	IEnumerator ComeBackToNormal(){
		yield return new WaitForSeconds(10.0f);
		ReturnVision ();
	}
	
}
