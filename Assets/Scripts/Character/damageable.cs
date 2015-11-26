using UnityEngine;
using System.Collections;

public interface IDamageable<T> {

     void takeDamage(T damage);

}
