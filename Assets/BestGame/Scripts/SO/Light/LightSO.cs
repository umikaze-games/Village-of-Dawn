using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "LightSO", menuName = "Light/LightSO")]
public class LightSO : ScriptableObject
{
	public List<LightDetails> lightPatternList;

	public LightDetails GetLightDetails(LightType lightShift)
	{
		return lightPatternList.Find(l => l.lightType == lightShift);
	}

}

[System.Serializable]
public class LightDetails
{
	public LightType lightType;
	public Color lightColor;
	public float lightIntensity;
}