using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BranchAndBound
{
    private PriorityQueue<Node> queue;
    private List<FoodClass> foods;

    public BranchAndBound(List<FoodClass> foods)
    {
        this.foods = foods;
        this.queue = new PriorityQueue<Node>();
    }

    public List<FoodClass> FindClosestCalorieCombination(double targetCalories, int nfoods)
    {
        double initialBound = CalculateInitialBound();
        for (int i = 0; i < foods.Count; i++)
        {
            queue.Enqueue(new Node(new List<FoodClass> { foods[i] }, foods[i].Calories, initialBound, i));
        }

        List<FoodClass> closestCombination = null;
        double minDifference = double.PositiveInfinity;

        while (queue.Count() > 0)
        {
            Node currentNode = queue.Dequeue();

            // If the current combination is closer to the target, update closestCombination
            double currentDifference = Math.Abs(currentNode.TotalCalories - targetCalories);
            if (currentDifference < minDifference)
            {
                closestCombination = currentNode.Foods;
                minDifference = currentDifference;
            }

            // Expand node and add children to queue
            if (currentNode.Foods.Count < nfoods)
            {
                for (int i = currentNode.LastIndex + 1; i < foods.Count; i++)
                {
                    FoodClass food = foods[i];
                    List<FoodClass> newCombination = new List<FoodClass>(currentNode.Foods);
                    newCombination.Add(food);

                    double totalCalories = currentNode.TotalCalories + food.Calories;
                    double bound = CalculateBound(newCombination, i, totalCalories, targetCalories);

                    if (bound < CurrentBound())
                    {
                        queue.Enqueue(new Node(newCombination, totalCalories, bound, i));
                    }

                }

            }

        }
        return closestCombination;
    }

    private double CalculateInitialBound()
    {
        double totalCalories = foods.Sum(food => food.Calories);
        return Math.Abs(totalCalories);
    }

    private double CalculateBound(List<FoodClass> combination, int lastIndex, double totalCalories, double targetCalories)
    {
        double remainingCalories = targetCalories - totalCalories;
        if (remainingCalories <= 0)
        {
            return Math.Abs(remainingCalories);
        }
        else
        {
            return Math.Abs(remainingCalories / (foods.Count - lastIndex));
        }
    }

    private double CurrentBound()
    {
        if (queue.Count() == 0)
        {
            return double.PositiveInfinity;
        }
        else
        {
            return queue.Peek().Bound;
        }
    }

    private class Node : IComparable<Node>
    {
        public List<FoodClass> Foods { get; }
        public double TotalCalories { get; }
        public double Bound { get; }
        public int LastIndex { get; }

        public Node(List<FoodClass> foods, double totalCalories, double bound, int lastIndex)
        {
            Foods = foods;
            TotalCalories = totalCalories;
            Bound = bound;
            LastIndex = lastIndex;
        }

        public int CompareTo(Node other)
        {
            return Bound.CompareTo(other.Bound);
        }
    }
}
