using UnityEngine;
using System.Collections;

public interface IEvent
{
    void onSpriteCollisionEnter(GameObject other);
    void onSpriteCollisionExit(GameObject other);
}
