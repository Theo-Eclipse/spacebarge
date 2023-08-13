using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlierEffect : BuffEffect
{
    [Header("Parameters Change")]
    public BasicFlier flierParameters;

    private void Reset()// Once the component created in editor.
    {
        if (!flierParameters) 
            flierParameters = new BasicFlier();

    }
}
