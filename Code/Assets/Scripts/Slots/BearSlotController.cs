
using System;
using UnityEngine;

public class BearSlotController : MonoBehaviour
{
	public delegate void OnUpdateItems();

	public event BearSlotController.OnUpdateItems OnUpdateItemsEvent;

	public bool Dirt = true;
}


