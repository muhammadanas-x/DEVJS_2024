using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2Manager : BasePuzzle
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
                Light light = lights[i];

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

        EnableAndDisableSwitchsPrompt();
    }

    private bool IsSolved()
    {
        return true;
    }

}
