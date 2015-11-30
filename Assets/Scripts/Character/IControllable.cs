using UnityEngine;
using System.Collections;

public interface IControllable {

    void move(float axis);
    void jump();
    void doneJump();
    void shoot(float direction);
    bool isGrounded();
}
