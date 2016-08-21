using System;
using UnityEngine;


public class RoomButtonsController : MonoBehaviour
{
	public TerraceButtonController Terrace;

	public KitchenButtonController Kitchen;

	public BathroomButtonController Bathroom;

	public BedroomButtonController Bedroom;

	public void Select(RoomButtonController roomButtonController)
	{
        //NGUITools.SetActive(this.Terrace.SelectedButtonEffect, this.Terrace == roomButtonController);
        //NGUITools.SetActive(this.Kitchen.SelectedButtonEffect, this.Kitchen == roomButtonController);
        //NGUITools.SetActive(this.Bathroom.SelectedButtonEffect, this.Bathroom == roomButtonController);
        //NGUITools.SetActive(this.Bedroom.SelectedButtonEffect, this.Bedroom == roomButtonController);
	}

	public void ForceShowAllPercentLabels()
	{
		this.Terrace.ForceShowPercent();
		this.Kitchen.ForceShowPercent();
		this.Bathroom.ForceShowPercent();
		this.Bedroom.ForceShowPercent();
	}

	public void ForceHideAllPercentLabels()
	{
		this.Terrace.ForceHidePercent();
		this.Kitchen.ForceHidePercent();
		this.Bathroom.ForceHidePercent();
		this.Bedroom.ForceHidePercent();
	}

	public void BlockPercentProgress(bool block)
	{
		this.Terrace.BlockProgress = block;
		this.Kitchen.BlockProgress = block;
		this.Bathroom.BlockProgress = block;
		this.Bedroom.BlockProgress = block;
	}
}


