using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoomNotifier : MonoBehaviour {
	PlayerController pc;
	public static List<BugController> bc;
	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		bc = GameObject.FindGameObjectsWithTag ("BugController").Select(b => {
			return b.GetComponent<BugController>();
		}).ToList();
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			string roomNumber = this.collider.tag;
			int currentRoomNumber = pc.GetRoomNumber();
			switch(roomNumber){
				case "Room44":
					if(currentRoomNumber != 44){
						pc.SetRoomNumber(44);
					bc.ForEach(b => {b.PlayerChangedRooms(44);});
					}
					break;
				case "Hallway1":
					if(currentRoomNumber != 1){
						pc.SetRoomNumber(1);
						bc.ForEach(b => {b.PlayerChangedRooms(1);});
					}
					break;
				case "Room39":
					if(currentRoomNumber != 39){
						pc.SetRoomNumber(39);
						bc.ForEach(b => {b.PlayerChangedRooms(39);});
					}
					break;
				case "Room33":
					if(currentRoomNumber != 33){
						pc.SetRoomNumber(1);
						bc.ForEach(b => {b.PlayerChangedRooms(1);});
					}
					break;
				case "Room34":
					if(currentRoomNumber != 34){
						pc.SetRoomNumber(34);
						bc.ForEach(b => {b.PlayerChangedRooms(34);});
					}
					break;
				case "Room18":
					if(currentRoomNumber != 18){
						pc.SetRoomNumber(18);
						bc.ForEach(b => {b.PlayerChangedRooms(18);});
					}
					break;
				case "Room32":
					if(currentRoomNumber != 32){
						pc.SetRoomNumber(1);
					bc.ForEach(b => {b.PlayerChangedRooms(1);});
					}
					break;
				case "Hallway2":
					if(currentRoomNumber != 2){
						pc.SetRoomNumber(2);
					bc.ForEach(b => {b.PlayerChangedRooms(2);});
					}
					break;
				case "Room9":
					if(currentRoomNumber != 9){
						pc.SetRoomNumber(9);
					bc.ForEach(b => {b.PlayerChangedRooms(9);});
					}
					break;
				case "Room29":
					if(currentRoomNumber != 29){
						pc.SetRoomNumber(29);
					bc.ForEach(b => {b.PlayerChangedRooms(29);});
					}
					break;
				case "Room60":
					if(currentRoomNumber != 60){
						pc.SetRoomNumber(60);
					bc.ForEach(b => {b.PlayerChangedRooms(60);});
				}
					break;
				case "Room49":
					if(currentRoomNumber != 49){
						pc.SetRoomNumber(49);
					bc.ForEach(b => {b.PlayerChangedRooms(49);});
					}
					break;
				case "Room28":
					if(currentRoomNumber != 28){
						pc.SetRoomNumber(28);
					bc.ForEach(b => {b.PlayerChangedRooms(28);});
					}
					break;
				}
		}
	}
}
