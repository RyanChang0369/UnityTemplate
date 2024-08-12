
/// <summary>
/// Interface for the <see cref="StaticKeyedDictionary{TKey, TValue}"/>. Allows
/// certain functions to be called without knowing the specific typeparams for
/// the object.
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public interface IStaticKeyedDictionary : IUnityDictionary
{
    #region Methods
    /// <summary>
    /// Used by <see cref="StaticKeyedDictionaryPropertyDrawer"/> to assign the
    /// static keys for the editor.
    /// </summary>
    /// <param name="targetObject">The serialized object this property belongs
    /// to.</param>
    public void GenerateStaticKeys(UnityEngine.Object targetObject);

    /// <summary>
    /// Retrieve a label from the provided <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public string LabelFromKey(object key);
    #endregion
}