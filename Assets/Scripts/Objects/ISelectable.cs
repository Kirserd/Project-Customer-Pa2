using UnityEngine;
///<summary>
/// An interface representing a selectable object in the game world.
///</summary>
public interface ISelectable
{
    ///<summary>
    /// The GameObject associated with the interactable object.
    ///</summary>
    GameObject GameObject { get; }

    ///<summary>
    /// Boolean which states whether selection should be enabled or disabled.
    ///</summary>
    bool IsActive { get; }
    bool IsSelected { get; set; }

    ///<summary>
    /// Method that defines selection behavior of the object.
    /// Implementing classes must specify what happens when the object is selected.
    ///</summary>
    void Select();

    ///<summary>
    /// Method that defines deselection behavior of the object.
    /// Implementing classes must specify what happens when the object is deselected.
    ///</summary>
    void Deselect();
}
