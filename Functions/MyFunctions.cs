//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyFuncs
{
    //A collection of various functions
    public static class MyFunctions
    {
        //Distance
        public static float DistanceSqr(float x1, float y1, float x2, float y2)
        {
            float diffX = x2 - x1;
            float diffY = y2 - y1;
            return diffX * diffX + diffY * diffY;
        }
        public static float Distance(float x1, float y1, float x2, float y2)
        {
            return Mathf.Sqrt(DistanceSqr(x1, y1, x2, y2));
        }

        //A generic floodfill made by me. Why does a floodfill have to be generic? I dont know. It is cool though
        //Assumes the size of the fill is not that big that hashsets become benefitial
        public static List<T> FloodFill<T>(T start, Func<T, IEnumerable<T>> getNeighbours, Func<T, bool> conditionForNeighbours)
        {
            List<T> selection = new List<T>(); //Filled by the flood
            List<T> floodFront = new List<T> { start }; //Frontal wave of the flood from where it expands

            while (floodFront.Count > 0)
            {
                T item = floodFront[floodFront.Count - 1];
                floodFront.RemoveAt(floodFront.Count - 1);
                selection.Add(item);

                foreach (var neighbour in getNeighbours(item))
                {
                    if (neighbour == null
                    || !conditionForNeighbours(neighbour)
                    || selection.Contains(neighbour)
                    || floodFront.Contains(neighbour))
                    {
                        continue;
                    }

                    floodFront.Add(neighbour);
                }
            }

            return selection;
        }
        public static List<T> FloodFill<T>(T start, Func<T, IEnumerable<T>> getNeighbours)
        {
            return FloodFill(start, getNeighbours, (T t) => { return true; });
        }

        //Qick lookAt
        public static Quaternion LookAt(Vector2 lookFrom, Vector2 lookTowards)
        {
            return Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (lookTowards - lookFrom));
        }

        //A performant smooth function for inputs between 0 and 1.
        public static float SimpleSmooth(float input)
        {
            if (input < 0)
            {
                return 0;
            }
            if (input > 1)
            {
                return 1;
            }
            float square = input * input;
            return -2 * square * input + 3 * square;
        }

        //A more smooth function.
        public static float Smooth(float input)
        {
            if (input < 0)
            {
                return 0;
            }
            if (input > 1)
            {
                return 1;
            }
            return Mathf.Sin(input * 90);
        }

        //Manhattan distance
        public static float Manhattan(Vector2 one, Vector2 two)
        {
            return Mathf.Abs(one.x - two.x) + Mathf.Abs(one.y - two.y);
        }

        //The distance of a point from the line defined by start and end. Line is assumed to be infinite in both directions.
        public static float DistanceFromLine(Vector2 start, Vector2 end, Vector2 point)
        {
            return Mathf.Abs((point.x - start.x) * (start.y - end.y) + (point.y - start.y) * (end.x - start.x)) / Vector2.Distance(start, end);
        }

        //Maps a number into the intervall 0 to 1. 
        //Is meant to represent a chance/resistance (or similar), where each point preconversion 
        //adds 1% chance/resistance multiplicativly
        public static float PercentageConversion(float preConversion)
        {
            return 1 - Mathf.Pow(0.99f, preConversion);
        }

        //The hitpoint of a line defined by start point and end point with a circle at a given position. Returns Did it hit? Out hitpoint. Just very simple Vectormath
        //Obviously there are 2 hitPoints, but this will always return the one leaaving the circle, not the one entering it.
        //This is the only hitPoint I need so far and the correct one if the startPoint is inside the circle considering direction.
        //Line is assumed infinite in one direction from the start unless allowNegativeDirections is true
        public static bool LineCircleHitPoint(Vector2 lineStart, Vector2 lineDirection, Vector2 circlePosition, float circleRadius, out Vector2 hit, bool allowNegativeDirection = false)
        {
            Vector2 relStart = lineStart - circlePosition;
            float m = lineDirection.sqrMagnitude;
            float pHalf = (relStart.x * lineDirection.x + relStart.y * lineDirection.y) / m;
            float q = (relStart.sqrMagnitude - circleRadius * circleRadius) / m;

            float D = pHalf * pHalf - q;
            if (D < 0)
            {
                hit = Vector2.zero;
                return false;
            }

            float t = -pHalf + Mathf.Sqrt(D);
            hit = lineStart + lineDirection * t;

            return allowNegativeDirection || t >= 0;
        }

        //Same as above, but now the circle is an ellipse!
        //I had to come up with the math myself but it looks like it works.
        //Line is assumed infinite in one direction from the start unless allowNegativeDirections is true
        public static bool LineEllipseHitPoint(Vector2 lineStart, Vector2 lineDirection, Vector2 ellipseMiddle, float xRadius, float yRadius, out Vector2 hit, bool allowNegativeDirection = false)
        {
            Vector2 relStart = lineStart - ellipseMiddle;
            float xRS = xRadius * xRadius;
            float yRS = yRadius * yRadius;

            float d = lineDirection.x * lineDirection.x / xRS + lineDirection.y * lineDirection.y / yRS;
            float pHalf = (relStart.x * lineDirection.x / xRS + relStart.y * lineDirection.y / yRS) / d;
            float q = (relStart.x * relStart.x / xRS + relStart.y * relStart.y / yRS) / d;

            float D = pHalf * pHalf - q;
            if (D < 0)
            {
                hit = Vector2.zero;
                return false;
            }

            float t = -pHalf + Mathf.Sqrt(D);
            hit = lineStart + lineDirection * t;

            return allowNegativeDirection || t >= 0;
        }

        //Do 2 circles touch? Pretty simple.
        public static bool CircleCircleHit(Vector2 pos1, float radius1, Vector2 pos2, float radius2)
        {
            float x = pos2.x - pos1.x;
            float y = pos2.y - pos1.y;
            float r = radius1 + radius2;

            return x * x + y * y < r * r;
        }

        //Do two lines hit, if yes where? Basic vector math. Lines are assumed infinite in one direction from the start unless allowNegativeDirections is true
        public static bool LineLineHitPoint(Vector2 start1, Vector2 dir1, Vector2 start2, Vector2 dir2, out Vector2 hit, bool allowNegativeDirections = false)
        {
            float d = dir1.y * dir2.x - dir1.x * dir2.y;
            if (d == 0)
            {
                hit = Vector2.zero;
                return false;
            }

            float t = (start2.y * dir2.x + start1.x * dir2.y + start2.x * dir2.y + start1.y * dir2.x) / d;
            hit = start1 + dir1 * t;

            return allowNegativeDirections || (t >= 0 && Vector2.Dot(hit - start2, dir2) >= 0);
        }
    }
}

