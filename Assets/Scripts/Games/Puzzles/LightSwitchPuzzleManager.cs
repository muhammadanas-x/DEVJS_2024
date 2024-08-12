using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LigthSwitchPuzzleManager : BasePuzzle
{
    [SerializeField]
    private LightSwitch[] switches;
    public Light[] lights;

    public override bool Solved => IsSolved();

    public override BaseSwitch[] Switches => switches;

    protected override void HandleInteraction(BaseSwitch baseSwitchUsed)
    {
        LightSwitch lightSwitch = (LightSwitch)baseSwitchUsed;
        string sequenceToLight = lightSwitch.sequence;

        for (int i = 0; i < sequenceToLight.Length; i++)
        {
            if (sequenceToLight[i].ToString() == "1")
            {
                int lightIndex = i; 
                Light light = lights[lightIndex];

                if (!light.IsLighted)
                {
                    light.LightUp();
                }
                else
                {
                    light.UnLight();
                }

              
               
            }
        }
    }

    private bool IsSolved()
    {
        foreach (Light light in lights)
        {
            if (!light.IsLighted)
            {
                return false;
            }
        }

        return true;
    }

}
