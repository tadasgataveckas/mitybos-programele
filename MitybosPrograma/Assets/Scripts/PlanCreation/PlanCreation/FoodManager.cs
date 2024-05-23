using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodManager : MonoBehaviour
{
	// Dictionary to store foods for each day and hour
	private Dictionary<int, Dictionary<int, List<FoodItem>>> foodsByDayAndHour = new Dictionary<int, Dictionary<int, List<FoodItem>>>();

	// Currently selected hour and day
	private int selectedHour;
	private int selectedDay;

	// Function to add a food item to a specific hour and day
	public void AddFoodToHourAndDay(int day, int hour, FoodItem foodItem)
	{
		if (!foodsByDayAndHour.ContainsKey(day))
		{
			foodsByDayAndHour[day] = new Dictionary<int, List<FoodItem>>();
		}

		if (!foodsByDayAndHour[day].ContainsKey(hour))
		{
			foodsByDayAndHour[day][hour] = new List<FoodItem>();
		}

		foodsByDayAndHour[day][hour].Add(foodItem);
	}

	// Function to get the list of foods for a specific hour and day
	public List<FoodItem> GetFoodsForHourAndDay(int day, int hour)
	{
		if (foodsByDayAndHour.ContainsKey(day) && foodsByDayAndHour[day].ContainsKey(hour))
		{
			return foodsByDayAndHour[day][hour];
		}
		else
		{
			return new List<FoodItem>(); // Return an empty list if no foods for the hour and day
		}
	}

	// Function to handle drag and drop of food items
	public void OnDragFoodItem(BaseEventData eventData)
	{
		// Implement drag and drop functionality here
	}

	// Function to change the hour for a food item
	public void ChangeHourForFoodItem(int day, int oldHour, int newHour, FoodItem foodItem)
	{
		if (foodsByDayAndHour.ContainsKey(day) && foodsByDayAndHour[day].ContainsKey(oldHour))
		{
			if (foodsByDayAndHour[day][oldHour].Contains(foodItem))
			{
				foodsByDayAndHour[day][oldHour].Remove(foodItem);
				AddFoodToHourAndDay(day, newHour, foodItem);
			}
		}
	}

	// Function to adjust the time frame of the desired day
	public void AdjustTimeFrameForDay(int day, int newStartTime, int newEndTime)
	{
		// Implement adjusting time frame functionality here
	}
}