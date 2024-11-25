using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropDetails_SO", menuName = "Crop/CropDetails_SO")]
public class CropDetails_SO : ScriptableObject
{
    public List<CropDetails> cropDetailsList;
}
    