using UnityEngine;
using UnityEngine.UI;

public class PhysicalScrollers : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject[] objects; // Čia įrašykite objektus, kuriuos norite scroll'inti
    private RectTransform contentRect;

    void Start()
    {
        // Paimame ScrollView turinio RectTransform
        contentRect = scrollRect.content.GetComponent<RectTransform>();

        // Prijungiame OnValueChanged įvykį prie mūsų metodo
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    void Update()
    {
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    // Metodas, kuris bus iškviestas kai keičiasi ScrollView reikšmė
    public void OnScrollValueChanged(Vector2 scrollPos)
    {
        // Gauti vertikalių poziciją, kurioje yra ScrollRect centravimas
        float centerY = scrollPos.y * (contentRect.rect.height - scrollRect.viewport.rect.height);

        // Einame per visus objektus ir tikriname, ar objektas yra centriniame regione
        foreach (GameObject obj in objects)
        {
            RectTransform objRect = obj.GetComponent<RectTransform>();
            float objY = objRect.localPosition.y;

            // Tikriname, ar objektas yra centriniame regione
            if (objY <= centerY && objY + objRect.rect.height >= centerY)
            {
                Debug.Log("Selected object: " + obj.name);
                // Darykite tai, ką norite su šiuo objektu
                break; // Jei norite rasti tik vieną objektą, tai nutraukite ciklą
            }
        }
    }
}
