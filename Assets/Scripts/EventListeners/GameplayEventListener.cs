using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MineAndRefact.Core
{
    public class GameplayEventListener : MonoBehaviour, IGameplayEventListener
    {
        private UnityEvent<IResource> _pickUpResource = new UnityEvent<IResource>();
        private UnityEvent<IResource> _dropResourceInSpot = new UnityEvent<IResource>();

        public UnityEvent<IResource> PickUpResource => _pickUpResource;

        public UnityEvent<IResource> DropResourceInSpot => _dropResourceInSpot;
    }
}


