using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PopupDialouge : PopupBox
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float slowDownMult;
    [SerializeField] float typerwriterSpeed;
    [SerializeField] char[] slowCharachters;
    [SerializeField] char[] skipCharachters;


    [SerializeField] float generalScale;

    private Vector2 targSize = Vector2.zero;

    private Vector3 GetPositionAbove(GameObject obj, Vector3 offset)
    {
        Bounds bounds = new Bounds();
        bool foundBounds = false;

        if (obj.TryGetComponent<Renderer>(out var renderer))
        {
            bounds = renderer.bounds;
            foundBounds = true;
        }
        else if (obj.TryGetComponent<Collider>(out var collider))
        {
            bounds = collider.bounds;
            foundBounds = true;
        }
        else if (obj.TryGetComponent<Collider2D>(out var collider2D))
        {
            bounds = collider2D.bounds;
            foundBounds = true;
        }

        if (foundBounds)
        {
            return new Vector3(
                bounds.center.x + offset.x, 
                bounds.max.y + offset.y,
                bounds.center.z + offset.z
            );
        }

        return obj.transform.position + offset;
    }

    public void Initialize(GameObject givenAttatchedObj)
    {
        if(generalScale <= 0.001f)generalScale = 1f;

        attatchedObj = givenAttatchedObj;
        transform.position = GetPositionAbove(attatchedObj,Vector3.up * generalScale);
        if(!TryGetComponent<Canvas>(out selfCanvasPlane)) 
        {
            Debug.Log(selfCanvasPlane);
            Destroy(gameObject);
        }
        panelRectTransform = selfCanvasPlane.GetComponent<RectTransform>();

        if(slowDownMult >= 0.001f) slowDownMult = 1;
        ResizeCanvas(true);

        //bleh
        EventBus.RequestEvent("CancelRightHandInspectDialouge", true).ping += cancR;
        EventBus.RequestEvent("CancelLeftHandInspectDialouge", true).ping += cancL;
        //
    }

    public override void ShowCanvas(string includes, bool forceHandsCam)
    {
        base.ShowCanvas(includes);//doesnt do anything

        if (forceHandsCam) 
        {
            gameObject.layer = LayerMask.NameToLayer("Hands");
            selfCanvasPlane.worldCamera = ServiceHub.Instance.cameras.overlayCamera;
            selfCanvasPlane.GetComponent<FaceThePlayer>().enabled = false;

            selfCanvasPlane.transform.LookAt(selfCanvasPlane.worldCamera.transform);
            selfCanvasPlane.transform.rotation *= Quaternion.Euler(0, 180, 0);
        }
        else selfCanvasPlane.worldCamera = ServiceHub.Instance.cameras.mainCamera;


        if (isTypewriterActive && typeWriter != null) StopCoroutine(typeWriter);
        typeWriter = StartCoroutine(TypewriterEffect(includes));
    }

    public override void ShowCanvas(string includes)
    {
        base.ShowCanvas(includes);//doesnt do anything

        selfCanvasPlane.worldCamera = ServiceHub.Instance.cameras.mainCamera;
        if (isTypewriterActive && typeWriter != null) StopCoroutine(typeWriter);
        typeWriter = StartCoroutine(TypewriterEffect(includes));
    }





    private RectTransform panelRectTransform;
    public void ResizeCanvas()
    {
        text.ForceMeshUpdate();
        Vector2 preferredSize = text.GetPreferredValues();
        float finalWidth = Mathf.Max(preferredSize.x + generalScale * 2, 2);
        float finalHeight = Mathf.Max(preferredSize.y + generalScale * 2, 1);
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

    ///do not do this shit.... this is the worst thing ive coded in a hot munits.. chris didnt to this part this is me
    private bool cancelLeft;
    private void cancL()
    {
        cancelLeft = true;
    }
    private bool cancelRight;
    private void cancR()
    {
        cancelRight = true;

    }
    //

    public IEnumerator TypewriterEffect(string dialogueString)
    {
        text.text = "";
        isTypewriterActive = true;

        float locSpeed = typerwriterSpeed;

        text.fontSize = generalScale * 0.3f;

        if(dialogueString.Length > 2 && dialogueString.StartsWith("/I"))
        {
            text.fontStyle = FontStyles.Bold | FontStyles.Italic;
            dialogueString = dialogueString.Substring(2);
        }

        foreach (char letter in dialogueString.ToCharArray())
        {
            //CODE VOMIT, FIX LATER
            if (attatchedObj == null || (cancelLeft && attatchedObj.name == "InspectUiTargetL"))
            {
                break;
            }
            if(attatchedObj == null || (cancelRight && attatchedObj.name == "InspectUiTargetR"))
            {
                break;
            }
            //===================
            locSpeed = typerwriterSpeed;

            if(slowCharachters.Contains(letter)) locSpeed = slowDownMult; //slowing down for periods
            if(skipCharachters.Contains(letter)) locSpeed = 0.001f; // skipping things like spaces
            if (text.text.Length % 40 == 0) text.text += "\n";
            
            text.text += letter;

            ResizeCanvas();

            if (text.text.Length > 70 && letter == '.')
            {
                Debug.Log("EEEEUUUU");
                yield return new WaitForSeconds(4);
                text.text = "";
            }

            yield return new WaitForSeconds(locSpeed);

        }

        for (int i = 0; i < 4000; i++)
        {
            //CODE VOMIT, FIX LATER
            if (attatchedObj == null) Destroy(gameObject);
            if (attatchedObj == null || (cancelLeft && attatchedObj.name == "InspectUiTargetL"))
            {
                cancelLeft = false;
                text.text = "";
                targSize = Vector2.zero;

                yield return new WaitForSeconds(0.05f);

                isTypewriterActive = false;
                Destroy(gameObject);
            }
            if (attatchedObj == null || (cancelRight && attatchedObj.name == "InspectUiTargetR"))
            {
                cancelRight = false;
                text.text = "";
                targSize = Vector2.zero;

                yield return new WaitForSeconds(0.05f);

                isTypewriterActive = false;
                Destroy(gameObject);
            }
            //===================
            yield return new WaitForSeconds(Time.deltaTime);
        }

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
