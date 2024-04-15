using UnityEngine;

public interface IBehaviour
{
    string getName();
    void Jump();
    void OnBehaviourUpdate();
    void OnBehaviourFixedUpdate();
}
