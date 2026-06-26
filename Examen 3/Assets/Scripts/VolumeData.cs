using UnityEngine;

[CreateAssetMenu(fileName = "NewVolumeData", menuName = "Audio/Volume Data")]
public class VolumeData : ScriptableObject
{
    [Header("Configuración")]
    [Range(0f, 10f)]
    public float volume = 10f;
}