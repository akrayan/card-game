using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController
{
    public CardStack Stack { get; }

    public void PlayTurn();

}
