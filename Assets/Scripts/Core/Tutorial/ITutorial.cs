using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ITutorial
{
    void EnableInteraction(bool enabled);
    Task PlayAnimationAsync(List<Sprite> frames);
}
