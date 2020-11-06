using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangeScript : MonoBehaviour
{
    // Start is called before the first frame update
    //InspectorValues
    [SerializeField] List<Color> colors;
    [SerializeField] bool RunOnStart = true;
    [SerializeField] [Range(0f,1f)]float speed = 0.3f;
    
    Color color_present;
    Color color_target;
    bool changing_color = true;
    float treshhold = 0.1f;

    int component_has;
    Component component_color;

    bool is_running = false;
    
    void Start()
    {
        component_has = ComponentHas();
        ControlErrors();
        if (RunOnStart)
            Run();
        
    }
    public void Run()
    {
        if (!is_running)
        {
            is_running = true;
            Change_Color();
            StartCoroutine(Changing_Color());
        }
    }
    public void Stop()
    {
        changing_color = false;
    }
    IEnumerator Changing_Color()//Renk değişim işlemi
    {
        while (changing_color)
        {
            if (Mathf.Abs(color_present.r - color_target.r) + Mathf.Abs(color_present.g - color_target.g) + Mathf.Abs(color_present.b - color_target.b) > treshhold)
            {
                color_present = Color.Lerp(color_present, color_target, speed * Time.deltaTime);
            }
            else 
            {
                Change_Color();
            }
            Apply_Color();
            yield return new WaitForSeconds(Time.deltaTime);
        }
        is_running = false;
    }

    void Change_Color()//Hedef rengi güncelle
    {
        color_present = color_present.a == 0f ? colors[0] : color_target;
        color_target = color_present;
        while (color_target == color_present)
        {
            if (colors.Count <= 1)
            {
                throw new System.Exception("colors dizisi değeri 1'den büyük olmalıdır");
                return;
            }
            color_target = colors[Random.Range(0, colors.Count)];
        }
    }
    #region Component Type
    void Apply_Color()//Rengi Uygula
    {
        switch (component_has)
        {
            case 0:
                (component_color as Image).color = color_present;
                break;
            case 1:
                (component_color as SpriteRenderer).color = color_present;
                break;
            case 2:
                (component_color as Camera).backgroundColor = color_present;
                break;
            default:
                break;
        }
    }
    int ComponentHas()
    {
        if (GetComponent<Image>() != null)
        {
            component_color = GetComponent<Image>();
            return 0;
        }
        else if (GetComponent<SpriteRenderer>() != null)
        {
            component_color = GetComponent<SpriteRenderer>();
            return 1;
        }
        else if (GetComponent<Camera>() != null)
        {
            component_color = GetComponent<Camera>();
            return 2;
        }
        return -1;
    }
    void ControlErrors()
    {
        if (component_has == -1)
        {
            throw new System.Exception("Need Component SpriteRenderer , Image or Camera");
        }
        if (colors[0].a == 0f)
        {
            throw new System.Exception("First Color musn't %100 transparent. Change the first color 'a' value.");
        }
    }
    #endregion
}
