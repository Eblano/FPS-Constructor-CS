﻿using UnityEngine;
using System.Collections;

/*
 FPS Constructor - Weapons
 Copyright� Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
/*
This script does almost nothing on its own. All of the work that this script does is actually done in its editor script, 'WizardEditor'
*/

[System.Serializable]
public class EffectsAndPenetrationWizard : MonoBehaviour
{
    public enum wizardScripts
    {
        UseEffects,
        BulletPenetration
    }

    public wizardScripts selectedScript = wizardScripts.UseEffects;
    public EffectsManager effectsManager;
}