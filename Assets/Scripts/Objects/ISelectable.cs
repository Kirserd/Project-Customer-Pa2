using UnityEngine;
///<summary>
/// An interface representing a selectable object in the game world.
///</summary>
public interface ISelectable
{
    ///<summary>
    /// The GameObject associated with the interactable object.
    /// Implementing classes must provide the GameObject property.
    ///</summary>
    GameObject GameObject { get; }

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
