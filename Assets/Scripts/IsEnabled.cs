using UnityEngine;

public class IsEnabled : MonoBehaviour
{
    //переменная отвечающая за количество очков которое нужно для открытия
    public int needToUnlock;
    //материал по умолчанию для закрытых цветов
    public Material blackMaterial;
  private void Start()
    {
        //проверка на достаточное количество очков для открытия расцветки
        if (PlayerPrefs.GetInt("score") < needToUnlock)
            GetComponent<MeshRenderer>().material = blackMaterial;
    }
}
