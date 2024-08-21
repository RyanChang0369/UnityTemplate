using System.Collections;

/// <summary>
/// Interface used solely by the <see cref="UnityDictionary<,>"/>, to allow
/// certain methods to be used without requiring the specification of type
/// params.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public interface IUnityDictionary
{
    public UnityDictionaryErrorCode CalculateErrorCode();

    /// <summary>
    /// Resets the internal dictionary to match the internal inspector key value
    /// pairs displayed in the editor.
    /// </summary>
    /// <param name="force">Whether or not to force a reset.</param>
    public void ResetInternalDict(bool force = false);

    /// <summary>
    /// Resets the internal inspector key value pairs to match the internal
    /// dictionary.
    /// </summary>
    /// <param name="force">Whether or not to force a reset.</param>
    public void ResetInspectorKVPs(bool force = false);
    
    public IDictionary AsDictionary();
}
