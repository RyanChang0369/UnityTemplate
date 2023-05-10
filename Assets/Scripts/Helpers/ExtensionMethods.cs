using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;

public static class ExtensionMethods
{
    private const string PROJECT_NAME = "Sunder";

    public const float PI2 = Mathf.PI * 2;
    
    /// <summary>
    /// Checks if an object either
    /// - is null
    /// - is a UnityEngine.Object that is == null, meaning that's invalid - ie.
	/// Destroyed, not assigned, or created with new
    ///
    /// Unity overloads the == operator for UnityEngine.Object, and returns true
	/// for a == null both if a is null, or if
    /// it doesn't exist in the c++ engine. This method is for checking for
	/// either of those being the case
    /// for objects that are not neccessarilly UnityEngine.Objects.
	/// This is usefull when you're using interfaces, since ==
    /// is a static method, so if you check if a member of an interface == null,
	/// it will hit the default C# == check instead
    /// of the overridden Unity check.
    /// 
    /// Source:
	/// https://forum.unity.com/threads/when-a-rigid-body-is-not-attached-component-getcomponent-rigidbody-returns-null-as-a-string.521633/
    /// </summary>
    /// <param name="obj">Object to check</param>
    /// <returns>True if the object is null, or if it's a UnityEngine.Object that has been destroyed</returns>
    public static bool IsNullOrUnityNull(this object obj)
    {
        if (obj == null)
        {
            return true;
        }

        if (obj is UnityEngine.Object @object)
        {
            if (@object == null)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any();
    }

    /// <summary>
    /// Adds component if not found in gameObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns>True if the gameobject has the component</returns>
    public static T AddComponentIfMissing<T>(this GameObject gameObject) where T : Component
    {
        if (!gameObject.HasComponent<T>(out T component))
        {
            return gameObject.AddComponent<T>();
        }
        else
        {
            return component;
        }
    }

    /// <summary>
    /// Adds component if not found in gameObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <param name="component">Either the newly added component or an existing one.</param>
    /// <returns>True if the gameobject has the component</returns>
    public static bool AddComponentIfMissing<T>(this GameObject gameObject, out T component) where T : Component
    {
        T thing = gameObject.GetComponent<T>();
        if (thing.IsNullOrUnityNull())
        {
            component = gameObject.AddComponent<T>();
            return false;
        }
        else
        {
            component = thing;
            return true;
        }
    }

    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool HasComponent<T>(this GameObject gameObject) where T : Component
    {
        return !gameObject.GetComponent<T>().IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for the component.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool HasComponent<T>(this Component self) where T : Component
    {
        return self.gameObject.HasComponent<T>();
    }

    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool HasComponent<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponent<T>();
        return !component.IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool HasComponent<T>(this Component self, out T component) where T : Component
    {
        component = self.GetComponent<T>();
        return !component.IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="errorMessage">Message to print to log.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a warning.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponent<T>(this GameObject gameObject, out T component, string errorMessage, bool doError = true) where T : Component
    {
        if (gameObject.HasComponent(out component))
        {
            return true;
        }
        else
        {
            if (doError)
                Debug.LogError(errorMessage);
            else
                Debug.LogWarning(errorMessage);

            return false;
        }
    }

    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a warning.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponentAuto<T>(this Component self, out T component, string name, bool doError = true) where T : Component
    {
        return self.gameObject.RequireComponentAuto(out component, name, doError);
    }

    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a warning.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponentAuto<T>(this GameObject gameObject, out T component, string name, bool doError = true) where T : Component
    {
        return gameObject.RequireComponent(out component,
            $"{gameObject} is missing required component {name}.",
            doError);
    }

    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="tag">Tag to search for</param>
    /// <param name="name">Name of the component that we are looking for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a warning.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponentAuto<T>(out T component, string tag, string name, bool doError = true) where T : Component
    {
        var go = GameObject.FindGameObjectWithTag(tag);

        if (!go)
        {
            Debug.LogError($"No gameobject found with {tag}");
            throw new ArgumentException($"No gameobject found with {tag}");
        }
        else
        {
            return go.RequireComponent(out component,
                $"{go} is missing required component {name}.",
                doError);
        }
    }

    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a warning.</param>
    /// <returns>True if gameObject has the specified component or if the component is already specified.</returns>
    public static bool AutofillComponent<T>(this Component self, ref T component, string name, bool doError=true) where T : Component
    {
        if (component)
            return true;
        else
            return self.RequireComponentAuto(out component, name, doError);
    }

    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">Component whose GameObject will be used to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a warning.</param>
    /// <returns>True if gameObject has the specified component or if the component is already specified.</returns>
    public static bool AutofillComponent<T>(this GameObject gameObject, ref T component, string name, bool doError=true) where T : Component
    {
        if (component)
            return true;
        else
            return gameObject.RequireComponentAuto(out component, name, doError);
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <returns>True if gameObject has the specified component in its children.</returns>
    public static bool HasComponentInChildren<T>(this GameObject gameObject) where T : Component
    {
        return !gameObject.GetComponentInChildren<T>().IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for the component.</param>
    /// <returns>True if gameObject has the specified component in its children.</returns>
    public static bool HasComponentInChildren<T>(this Component self) where T : Component
    {
        return self.gameObject.HasComponentInChildren<T>();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <returns>True if gameObject has the specified component in its children.</returns>
    public static bool HasComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponentInChildren<T>();
        return !component.IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <returns>True if gameObject has the specified component in its children.</returns>
    public static bool HasComponentInChildren<T>(this Component self, out T component) where T : Component
    {
        return self.gameObject.HasComponentInChildren<T>(out component);
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its parent.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <returns>True if gameObject has the specified component in its parent.</returns>
    public static bool HasComponentInParent<T>(this GameObject gameObject) where T : Component
    {
        return !gameObject.GetComponentInParent<T>().IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for the component.</param>
    /// <returns>True if gameObject has the specified component in its children.</returns>
    public static bool HasComponentInParent<T>(this Component self) where T : Component
    {
        return self.gameObject.HasComponentInParent<T>();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its parent.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <returns>True if gameObject has the specified component in its parent.</returns>
    public static bool HasComponentInParent<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponentInParent<T>();
        return !component.IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <returns>True if gameObject has the specified component in its children.</returns>
    public static bool HasComponentInParent<T>(this Component self, out T component) where T : Component
    {
        return self.gameObject.HasComponentInParent<T>(out component);
    }

    /// <summary>
    /// Returns an array of all objects with a certain type.
    /// </summary>
    /// <typeparam name="T">The component to look for.</typeparam>
    /// <param name="array">Array of GameObjects to look through.</param>
    /// <returns>An array of GameObjects with only the type of components specified.</returns>
    public static GameObject[] WithComponent<T>(this GameObject[] array) where T : Component
    {
        return array.Where(obj => obj.HasComponent<T>()).ToArray();
    }

    /// <summary>
    /// Returns an array of all objects with a certain tag.
    /// </summary>
    /// <param name="array">Array of GameObjects to look through.</param>
    /// <param name="tagName">Name of tag to search for.</param>
    /// <returns>An array of GameObjects with only the tags.</returns>
    public static GameObject[] WithTag(this GameObject[] array, string tagName)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        foreach (GameObject obj in array)
        {
            if (obj.CompareTag(tagName))
                gameObjects.Add(obj);
        }

        return gameObjects.ToArray();
    }

    /// <summary>
    /// Returns an angle between [-360, 360] degrees
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between [-360, 360] degrees</returns>
    public static float AsPlusMinus360(this float theta)
    {
        while (theta < -360)
            theta += 360;

        while (theta > 360)
            theta -= 360;

        return theta;
    }

    /// <summary>
    /// Returns an angle between [0, 360) degrees
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between [0, 360) degrees</returns>
    public static float AsPositiveDegrees(this float theta)
    {
        while (theta < 0)
            theta += 360;

        while (theta > 360)
            theta -= 360;

        return theta;
    }

    /// <summary>
    /// Returns an angle between [0, 2pi) radians
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between [0, 2pi) radians</returns>
    public static float AsPositiveRadians(this float theta)
    {
        while (theta < 0)
            theta += PI2;

        while (theta > PI2)
            theta -= PI2;

        return theta;
    }

    /// <summary>
    /// Returns an angle between (-360, 0] degrees
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between (-360, 0] degrees</returns>
    public static float AsNegativeDegrees(this float theta)
    {
        theta = theta.AsPositiveDegrees();

        if (theta == 0)
            return 0;
        else
            return theta - 360;
    }

    /// <summary>
    /// Returns an angle between (-2pi, 0] degrees
    /// </summary>
    /// <param name="theta">The angle to consider</param>
    /// <returns>An angle between (-2pi, 0] degrees</returns>
    public static float AsNegativeRadians(this float theta)
    {
        theta = theta.AsPositiveRadians();

        if (theta == 0)
            return 0;
        else
            return theta - PI2;
    }

    /// <summary>
    /// Returns an angle between [-180, 180] degrees
    /// </summary>
    /// <param name="theta"></param>
    /// <returns>An angle between [-180, 180] degrees</returns>
    public static float AsPlusMinus180(this float theta)
    {
        while (theta < -180)
            theta += 360;

        while (theta > 180)
            theta -= 360;

        return theta;
    }

    /// <summary>
    /// Returns an angle between [-pi, pi] radians
    /// </summary>
    /// <param name="theta"></param>
    /// <returns>An angle between [-pi, pi] radians</returns>
    public static float AsPlusMinusPi(this float theta)
    {
        while (theta < -Mathf.PI)
            theta += PI2;

        while (theta > Mathf.PI)
            theta -= PI2;

        return theta;
    }

    /// <summary>
    /// Returns an angle with a side. True is right side and false is left.
    /// <br/><br/>
    /// For example:<br/>
    /// theta = 120 returns 60 and false<br/>
    /// theta = 186 returns -6 and false<br/>
    /// theta = 16 returns 16 and true<br/>
    /// theta = 291 returns -69 and true
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static Tuple<float, bool> AsPlusMinus90AndSide(this float theta)
    {
        theta = theta.AsPlusMinus180();

        if ((theta >= 0 && theta <= 90) || (theta < 0 && theta > -90))
        {
            //Right side
            return new Tuple<float, bool>(theta, true);
        }
        else
        {
            if (theta >= 0)
                theta = 180 - theta;
            else
            {
                theta += 180;
                theta *= -1;
            }

            return new Tuple<float, bool>(theta, false);
        }
    }

    /// <summary>
    /// Returns true if both angles represents the same angle
    /// </summary>
    /// <param name="angle1"></param>
    /// <param name="angle2"></param>
    /// <returns></returns>
    public static bool AngleEqual(this float angle1, float angle2)
    {
        return Mathf.DeltaAngle(angle1, angle2) == 0;
    }

    /// <summary>
    /// Returns true if angle is between theta1 and theta2, that is, angle lies in the
    /// smallest arc formed by theta1 and theta2
    /// </summary>
    /// <param name="angle">The anlge to evaluate</param>
    /// <param name="theta1"></param>
    /// <param name="theta2"></param>
    /// <returns>An angle between -180 and 180.</returns>
    public static bool AngleIsBetween(this float angle, float theta1, float theta2)
    {
        float min = Mathf.Min(theta1, theta2);
        float max = Mathf.Max(theta1, theta2);

        //Debug.Log($"{min} {max} {angle}");
        //Debug.Log($"min.GetDeltaTheta(angle): {min.GetDeltaTheta(angle)}");
        //Debug.Log($"max.GetDeltaTheta(angle): {max.GetDeltaTheta(angle)}");

        return min.GetDeltaTheta(angle) >= 0 && max.GetDeltaTheta(angle) <= 0;
    }

    /// <summary>
    /// Returns angle if it falls within the smallest arc formed by theta1 and theta2.
    /// Else, returns either theta1 or theta2.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="theta1"></param>
    /// <param name="theta2"></param>
    /// <returns></returns>
    public static float ClampAngle(this float angle, float theta1, float theta2)
    {
        float min = Mathf.Min(theta1, theta2);
        float max = Mathf.Max(theta1, theta2);

        if (max - min == 180)
        {
            throw new ArgumentException($"Unable to clamp angle ({angle}) as theta 1 ({theta1}) and theta2 ({theta2}) " +
                $"differ by 180 degrees. There are two locations to clamp to.");
        }

        if (min.GetDeltaTheta(angle) < 0)
        {
            return min;
        }
        else if (max.GetDeltaTheta(angle) > 0)
        {
            return max;
        }
        else
        {
            return angle;
        }
    }

    /// <summary>
    /// Gets the direction of travel from actualTheta to targetTheta.
    /// If return value is -1, turn clockwise.
    /// If return value is 1, turn counterclockwise.
    /// </summary>
    /// <param name="actualTheta"></param>
    /// <param name="targetTheta"></param>
    /// <returns></returns>
    public static int DirectionToAngle(this float actualTheta, float targetTheta)
    {
        actualTheta = actualTheta.AsPositiveDegrees();
        targetTheta = targetTheta.AsPositiveDegrees();

        if (actualTheta == targetTheta)
            return 0;
        else
        {
            return (Mathf.DeltaAngle(actualTheta, targetTheta) < 0) ? 1 : -1;
        }
    }

    /// <summary>
    /// Gets the direction of travel from actualTheta to targetTheta.
    /// If return value is -1, turn clockwise.
    /// If return value is 1, turn counterclockwise.
    /// </summary>
    /// <param name="actualTheta"></param>
    /// <param name="targetTheta"></param>
    /// <returns></returns>
    public static int DirectionToAngle(this float actualTheta, float targetTheta, float margin)
    {
        actualTheta = actualTheta.AsPositiveDegrees();
        targetTheta = targetTheta.AsPositiveDegrees();

        if (actualTheta.Approx(targetTheta, margin))
            return 0;
        else
        {
            return (Mathf.DeltaAngle(actualTheta, targetTheta) < 0) ? 1 : -1;
        }
    }

    /// <summary>
    /// Returns the shortest angle between actualTheta and targetTheta.
    /// </summary>
    /// <param name="actualTheta"></param>
    /// <param name="targetTheta"></param>
    /// <returns></returns>
    public static float GetDeltaTheta(this float actualTheta, float targetTheta)
    {
        return Mathf.DeltaAngle(actualTheta, targetTheta);
    }

    /// <summary>
    /// True if a differs from b by no more than margin
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public static bool Approx(this float a, float b, float margin)
    {
        return Mathf.Abs(a - b) <= margin;
    }

    /// <summary>
    /// Alias to Mathf.Approximate
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool Approx(this float a, float b)
    {
        return Mathf.Approximately(a, b);
    }

    /// <summary>
    /// Returns true if number is in between bounds A and B, inclusive
    /// </summary>
    /// <param name="number">The number to evaluate</param>
    /// <param name="boundsA">The lower bound</param>
    /// <param name="boundsB">The upper bound</param>
    /// <param name="fixRange">Swaps bounds A and B if B < A</param>
    /// <returns></returns>
    public static bool IsBetween(this float number, float boundsA, float boundsB, bool fixRange = true)
    {
        if (fixRange)
        {
            float temp = boundsA;

            boundsA = Mathf.Min(boundsA, boundsB);
            boundsB = Mathf.Max(boundsB, temp);
        }

        return (boundsA <= number && number <= boundsB);
    }

    /// <summary>
    /// Returns sign of number.
    /// </summary>
    /// <param name="number">The sign of the number. Zero is considered positive.</param>
    /// <returns></returns>
    public static int Sign(this float number)
    {
        return number < 0 ? -1 : 1;
    }

    /// <summary>
    /// Returns either zero if number is zero or the sign of number if it is not.
    /// </summary>
    /// <param name="number">The number to evaluate.</param>
    /// <returns>Zero if number is zero, the sign of number otherwise.</returns>
    public static int ZeroOrSign(this float number)
    {
        return number == 0 ? 0 : number.Sign();
    }

    public static void Swap<T>(ref T a, ref T b)
    {
        T t = a;
        a = b;
        b = t;
    }

    /// <summary>
    /// Rotates a vector by theta degrees
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="theta">In degrees</param>
    /// <returns></returns>
    public static Vector2 RotateVector2(this Vector2 vec, float theta)
    {
        float sin = Mathf.Sin(theta * Mathf.Deg2Rad);
        float cos = Mathf.Cos(theta * Mathf.Deg2Rad);

        float oldX = vec.x, oldY = vec.y;

        return new Vector2(cos * oldX - sin * oldY, sin * oldX + cos * oldY);
    }

    /// <summary>
    /// Converts from polar coordinates
    /// </summary>
    /// <param name="r">Radius</param>
    /// <param name="theta">Angle, in radians</param>
    /// <returns></returns>
    public static Vector2 ConvertFromPolarCoords(float r, float theta)
    {
        float x = r * Mathf.Cos(theta);
        float y = r * Mathf.Sin(theta);

        return new Vector2(x, y);
    }

    /// <summary>
    /// See documentation for https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html?_ga=2.103801094.784080732.1641080817-863246645.1620669234
    /// </summary>
    /// <returns></returns>
    public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime,
        float maxSpeed, float deltaTime)
    {
        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
          0,
          0,
          Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime)
        );
    }

    public static bool Contains<T>(this T[] array, T comparison)
    {
        if (array == null)
            return false;

        foreach (T thing in array)
        {
            if (thing.Equals(comparison))
                return true;
        }

        return false;
    }

    public static T Last<T>(this T[] array)
    {
        if (array.Length > 0)
            return array[array.Length - 1];
        else
            return default;
    }

    /// <summary>
    /// Finds all of a certain type within GameObject's parent
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="thing"></param>
    /// <returns></returns>
    public static T[] GetAllInParent<T>(GameObject thing)
    {
        return thing.transform.parent.GetComponentsInChildren<T>();
    }

    public static void WriteSaveFile(string output, string fileName)
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PROJECT_NAME, fileName);

        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PROJECT_NAME));
        }

        using (StreamWriter sw = new(filePath))
        {
            sw.Write(output);
        }
    }

    public static string ReadSaveFile(string fileName)
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PROJECT_NAME, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Cannot find " + fileName);
        }

        using (StreamReader sr = new(filePath))
        {
            return sr.ReadToEnd();
        }
    }

    public static bool Contains(this string string1, string string2, StringComparison stringComparison)
    {
        return string1.IndexOf(string2, stringComparison) >= 0;
    }

    /// <summary>
    /// Returns value such that the change of value is towards target and is no greater than margin;
    /// </summary>
    /// <param name="value">The value to change.</param>
    /// <param name="target">The number to change towards.</param>
    /// <param name="margin">The maximal change.</param>
    /// <returns></returns>
    public static float GetMinimumChange(this float value, float target, float margin)
    {
        value += Mathf.Sign(target) * Mathf.Min(Mathf.Abs(target), Mathf.Abs(margin));
        return value;
    }

    public static Color GetColorFromHEX(this string hexCode)
    {
        if (!hexCode.Contains("#"))
        {
            hexCode = "#" + hexCode;
        }

        Color color;

        ColorUtility.TryParseHtmlString(hexCode, out color);

        return color;
    }

    public static Texture2D GetTexture(this Sprite sprite)
    {
        Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

    /// <summary>
    /// Returns true if a is approximately b
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">Second vector</param>
    /// <returns>True if a is approximately b, false otherwise</returns>
    public static bool Approx(this Vector3 a, Vector3 b)
    {
        return a.x.Approx(b.x) && a.y.Approx(b.y) && a.z.Approx(b.z);
    }

    /// <summary>
    /// Returns true if a is approximately zero.
    /// </summary>
    /// <param name="a">Vector</param>
    /// <returns>True if a is approximately zero, false otherwise.</returns>
    public static bool ApproxZero(this Vector3 a)
    {
        return a.Approx(Vector3.zero);
    }

    /// <summary>
    /// Returns true if a and b have the same root
    /// </summary>
    /// <param name="a">A</param>
    /// <param name="b">B</param>
    /// <returns>True if a and b have the same root, false otherwise.</returns>
    public static bool HasSameRoot(this Transform a, Transform b)
    {
        return a.root == b.root;
    }

    /// <summary>
    /// Returns true if a and b have different roots
    /// </summary>
    /// <param name="a">A</param>
    /// <param name="b">B</param>
    /// <returns>False if a and b have the same root, true otherwise.</returns>
    public static bool HasDifferentRoot(this Transform a, Transform b)
    {
        return !HasSameRoot(a, b);
    }

    /// <summary>
    /// Copies a component and adds it to destination.
    /// Adapted from http://answers.unity.com/answers/1118416/view.html
    /// </summary>
    /// <typeparam name="T">A component.</typeparam>
    /// <param name="original">Reference to the component to copy.</param>
    /// <param name="destination">Where to add the component.</param>
    /// <returns></returns>
    public static T CopyComponent<T>(this T original, GameObject destination) where T : Component
    {
        Type type = original.GetType();
        var dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst;
    }

    /// <summary>
    /// Adds addition to a dictionary with a list as its value.
    /// <typeparam name="TKey">The key.</typeparam>
    /// <typeparam name="TMem">The member of the list within the dictionary.</typeparam>
    /// <param name="dict">The dictionary to add to.</param>
    /// <param name="key">The key that holds the list to add addition to.</param>
    /// <param name="addition">What to add to the internal list.</param>
    /// </summary>
    public static void AddToDictList<TKey,TMem>(this Dictionary<TKey, List<TMem>> dict, TKey key, TMem addition)
    {
        if (!dict.ContainsKey(key))
            dict[key] = new List<TMem>();
        
        dict[key].Add(addition);
    }

    /// <summary>
    /// Shuffles the list in place. Adapted from https://stackoverflow.com/a/1262619.
    /// </summary>
    /// <typeparam name="T">Any type.</typeparam>
    /// <param name="list">List to shuffle.</param>
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = RNG.GetRandomInteger(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    /// <summary>
    /// Plays a sound using clip.
    /// </summary>
    /// <param name="audioSource">The audio source that plays the sound.</param>
    /// <param name="clip">The audio clip to use.</param>
    /// <param name="volume">Volume to play the clip at.</param>
    public static void PlaySound(this AudioSource audioSource, AudioClip clip, float volume)
    {
        if (clip)
        {
            audioSource.PlayOneShot(clip, volume); 
        }
    }

    /// <summary>
    /// Returns true if layer is in mask.
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    /// <summary>
    /// Returns true if the layer is in mask.
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool ContainsLayer(this int mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
