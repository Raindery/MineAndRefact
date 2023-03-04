using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MineAndRefact.Core
{
    public interface IGameplayEventListener
    {
        UnityEvent<IResource> PickUpResource { get; }
        UnityEvent<IResource> DropResourceInSpot { get; }
    }
}

