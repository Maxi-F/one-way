using UnityEngine;

public interface IBehaviour
{
    string getName();
    void Move(Vector3 direction);
    void Jump();
    void LookChange();
    void OnBehaviourUpdate();
    void OnBehaviourFixedUpdate();
}
