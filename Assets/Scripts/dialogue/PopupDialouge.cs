using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class PopupDialouge : PopupBox
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float slowDownMult;
    [SerializeField] float typerwriterSpeed;
    [SerializeField] char[] slowCharachters;
    [SerializeField] char[] skipCharachters;

    private Vector2 targSize = Vector2.zero;

    public void Initialize(GameObject givenAttatchedObj)
    {
        attatchedObj = givenAttatchedObj;
        transform.position = (attatchedObj.transform.position + 
                            new Vector3(0, attatchedObj.transform.lossyScale.y / 2 + 5, 0 ));
        if(!TryGetComponent<Canvas>(out selfCanvasPlane)) 
        {
            Debug.Log(selfCanvasPlane);
            Destroy(gameObject);
        }
        panelRectTransform = selfCanvasPlane.GetComponent<RectTransform>();

        if(slowDownMult >= 0.001f) slowDownMult = 1;
        ResizeCanvas(true);
    }

    public override void ShowCanvas(string includes)
    {
        base.ShowCanvas(includes);//doesnt do anything
        
        if(isTypewriterActive && typeWriter != null) StopCoroutine(typeWriter);
        typeWriter = StartCoroutine(TypewriterEffect(includes));
    }

    private RectTransform panelRectTransform;
    public void ResizeCanvas()
    {
        text.ForceMeshUpdate();
        Vector2 preferredSize = text.GetPreferredValues();
        float finalWidth = Mathf.Max(preferredSize.x + 1, 2);
        float finalHeight = Mathf.Max(preferredSize.y + 1, 1);
        targSize = new Vector2(finalWidth, finalHeight);
    }

    public void ResizeCanvas(bool hardSet)
    {
        text.ForceMeshUpdate();
        Vector2 preferredSize = text.GetPreferredValues();
        float finalWidth = Mathf.Max(preferredSize.x + 1, 2);
        float finalHeight = Mathf.Max(preferredSize.y + 1, 1);
        if(hardSet)panelRectTransform.sizeDelta = new Vector2(finalWidth, finalHeight);
        else targSize = new Vector2(finalWidth, finalHeight);
    }


    //thank you chris for the code!
    //==============================================================================
    private bool isTypewriterActive = false;
    private Coroutine typeWriter;

    public IEnumerator TypewriterEffect(string dialogueString)
    {
        text.text = "";
        isTypewriterActive = true;

        float locSpeed = typerwriterSpeed;
        foreach (char letter in dialogueString.ToCharArray())
        {
            locSpeed = typerwriterSpeed;

            if(slowCharachters.Contains(letter)) locSpeed *= slowDownMult; //slowing down for periods
            if(skipCharachters.Contains(letter)) locSpeed = 0.001f; // skipping things like spaces
            
            text.text += letter;

            ResizeCanvas();
            yield return new WaitForSeconds(locSpeed);
        }

        yield return new WaitForSeconds(text.text.Length * 0.2f);

        text.text = "";
        targSize = Vector2.zero;
        
        yield return new WaitForSeconds(0.05f);

        isTypewriterActive = false;
        Destroy(gameObject);

    }
    //===============================================================================

    void Update()
    {
        panelRectTransform.sizeDelta = Vector2.Lerp(panelRectTransform.sizeDelta, targSize, 20* Time.deltaTime);
    }

}
