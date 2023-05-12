using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Contains methods that extend game objects or other unity objects.
/// </summary>
public static class UnityObjectExtensions
{
    #region Query
    #region Query Single

    #region In Self
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
    #endregion

    #region In Parent
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
    #endregion

    #region In Children

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
    #endregion

    #endregion

    #region Query Multiple
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
    #endregion
    #endregion

    #region Autoadd
    #region Add If Missing
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

    #endregion

    #region Require Component
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
    #endregion

    #region Autofill
    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a warning.</param>
    /// <returns>True if gameObject has the specified component or if the component is already specified.</returns>
    public static bool AutofillComponent<T>(this Component self, ref T component, string name, bool doError = true) where T : Component
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
    public static bool AutofillComponent<T>(this GameObject gameObject, ref T component, string name, bool doError = true) where T : Component
    {
        if (component)
            return true;
        else
            return gameObject.RequireComponentAuto(out component, name, doError);
    }
    #endregion
    #endregion

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

    public static void InstantiateSingleton<T>(this T self, ref T singleton)
        where T : MonoBehaviour
    {
        if (singleton)
        {
            GameObject.Destroy(self);
            throw new ArgumentException($"Multiple instances of {typeof(T)}.");
        }
        else
        {
            singleton = self;
        }
    }
}