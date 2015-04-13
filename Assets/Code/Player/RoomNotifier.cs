using UnityEngine;
using System.Collections;

public class RoomNotifier : MonoBehaviour
{
	PlayerController pc;
	GeraldBehaviour gb;

	// Use this for initialization
	void Start (){
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		gb = GameObject.FindGameObjectWithTag ("Gerald").GetComponent<GeraldBehaviour> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			Debug.Log("Player Entered Room");
			string roomNumber = this.collider.tag;
			int currentRoomNumber = pc.GetRoomNumber();
			switch(roomNumber){
				case "Room44":
					if(currentRoomNumber != 44)
						pc.SetRoomNumber(44);
					break;
				case "Hallway1":
					if(currentRoomNumber != 1)
						pc.SetRoomNumber(1);
					break;
				case "Room39":
					if(currentRoomNumber != 39)
						pc.SetRoomNumber(39);
					break;
				case "Room33":
					if(currentRoomNumber != 33)
						pc.SetRoomNumber(33);
					break;
				case "Room34":
					if(currentRoomNumber != 34)
						pc.SetRoomNumber(34);
					break;
				case "Room18":
					if(currentRoomNumber != 18)
						pc.SetRoomNumber(18);
					break;
				case "Room32":
					if(currentRoomNumber != 32)
						pc.SetRoomNumber(32);
					break;
				case "Hallway2":
					if(currentRoomNumber != 2)
						pc.SetRoomNumber(2);
					break;
				case "Room9":
					if(currentRoomNumber != 9)
						pc.SetRoomNumber(9);
					break;
				case "Room29":
					if(currentRoomNumber != 29)
						pc.SetRoomNumber(29);
					break;
				case "Room60":
					if(currentRoomNumber != 60)
						pc.SetRoomNumber(60);
					break;
				case "Room49":
					if(currentRoomNumber != 49)
						pc.SetRoomNumber(49);
					break;
				case "Room28":
					if(currentRoomNumber != 28)
						pc.SetRoomNumber(28);
					break;
				}
		}

		if (other.CompareTag ("Gerald")) {
			Debug.Log("Gerald Entered Room");
			string roomNumber = this.collider.tag;
			int currentRoomNumber = gb.GetRoomNumber();
			switch(roomNumber){
				case "Room44":
					if(currentRoomNumber != 44)
						gb.SetRoomNumber(44);
					break;
				case "Hallway1":
					if(currentRoomNumber != 1)
						gb.SetRoomNumber(1);
					break;
				case "Room39":
					if(currentRoomNumber != 39)
						gb.SetRoomNumber(39);
					break;
				case "Room33":
					if(currentRoomNumber != 33)
						gb.SetRoomNumber(33);
					break;
				case "Room34":
					if(currentRoomNumber != 34)
						gb.SetRoomNumber(34);
					break;
				case "Room18":
					if(currentRoomNumber != 18)
						gb.SetRoomNumber(18);
					break;
				case "Room32":
					if(currentRoomNumber != 32)
						gb.SetRoomNumber(32);
					break;
				case "Hallway2":
					if(currentRoomNumber != 2)
						gb.SetRoomNumber(2);
	                    break;
	            case "Room9":
	                if(currentRoomNumber != 9)
	                    gb.SetRoomNumber(9);
	                break;
	            case "Room29":
	                if(currentRoomNumber != 29)
	                    gb.SetRoomNumber(29);
	                break;
	            case "Room60":
	                if(currentRoomNumber != 60)
	                    gb.SetRoomNumber(60);
	                break;
	            case "Room49":
	                if(currentRoomNumber != 49)
	                    gb.SetRoomNumber(49);
	                break;
	            case "Room28":
	                if(currentRoomNumber != 28)
	                    gb.SetRoomNumber(28);
	                break;
            }
        }
    }
}
