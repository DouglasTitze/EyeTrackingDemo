using UnityEngine;

public interface RayInterface
{
    void isHit(RaycastHit hitInfo);
    void isSelected(RaycastHit hitInfo);
    void isUnselected();
}