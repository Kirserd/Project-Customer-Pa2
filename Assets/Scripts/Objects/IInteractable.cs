///<summary>
/// An interface representing an interactable object in the game world.
///</summary>
public interface IInteractable : ISelectable
{
    ///<summary>
    /// Method that defines the interaction behavior of the object.
    /// Implementing classes must specify what happens when the object is interacted with.
    ///</summary>
    void Interact();
}
