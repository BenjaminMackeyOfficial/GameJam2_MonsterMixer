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


    [SerializeField] float generalScale;

    private Vector2 targSize = Vector2.zero;
    private AudioSource audioPlayer;
    private AudioClip audioClip;

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
    private void Start()
    {
        EventBus.RequestEvent("LevelReseting", true).ping += StopTypewriter;
    }
    private void StopTypewriter()
    {
        if (isTypewriterActive && typeWriter != null) StopCoroutine(typeWriter);
    }
    public void Initialize(GameObject givenAttatchedObj)
    {
        audioPlayer = GetComponent<AudioSource>();
        if (generalScale <= 0.001f)generalScale = 1f;

        attatchedObj = givenAttatchedObj;
        transform.position = GetPositionAbove(attatchedObj,Vector3.up * (generalScale * 2));
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
    public override void ShowCanvas(string includes, bool forceHandsCam, bool holdUntilFinished)
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

        transform.SetParent(attatchedObj.transform);
        if (isTypewriterActive && typeWriter != null) StopCoroutine(typeWriter);
        typeWriter = StartCoroutine(TypewriterEffect(includes, holdUntilFinished));
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

        transform.SetParent(attatchedObj.transform);
        if (isTypewriterActive && typeWriter != null) StopCoroutine(typeWriter);
        typeWriter = StartCoroutine(TypewriterEffect(includes, false));
    }

    public override void ShowCanvas(string includes)
    {
        base.ShowCanvas(includes);//doesnt do anything

        selfCanvasPlane.worldCamera = ServiceHub.Instance.cameras.mainCamera;

        transform.SetParent(attatchedObj.transform);

        if (isTypewriterActive && typeWriter != null) StopCoroutine(typeWriter);
        typeWriter = StartCoroutine(TypewriterEffect(includes, false));
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

    public IEnumerator TypewriterEffect(string dialogueString, bool hold)
    {
        text.text = "";
        isTypewriterActive = true;

        if (audioClip == null && attatchedObj.TryGetComponent<AudioSoundDataBin>(out AudioSoundDataBin bin))
        {
            Debug.Log("clip found");
            audioClip = bin.clip;
        }

        float locSpeed = typerwriterSpeed;

        text.fontSize = generalScale * 0.3f;

        if(dialogueString.Length > 2 && dialogueString.StartsWith("/I"))
        {
            text.fontStyle = FontStyles.Bold | FontStyles.Italic;
            dialogueString = dialogueString.Substring(2);
        }
        
        bool switchCount = false;
        for (int i = 0; i < dialogueString.Length; i++)
        {
            //CODE VOMIT, FIX LATER
            if (attatchedObj == null || (cancelLeft && attatchedObj.name == "InspectUiTargetL"))
            {
                break;
            }
            if (attatchedObj == null || (cancelRight && attatchedObj.name == "InspectUiTargetR"))
            {
                break;
            }
            //===================
            locSpeed = typerwriterSpeed;

            if (dialogueString[i] == '<') //ads the whole tag so you dont see it getting typed
            {
                int tagEnd = dialogueString.IndexOf('>', i);

                if (tagEnd != -1)
                {
                    text.text += dialogueString.Substring(i, tagEnd - i + 1);
                    i = tagEnd;

                    continue;
                }
            }

            char letter = dialogueString[i];

            if (text.text.Length % 40 == 0) switchCount = true;
            if (slowCharachters.Contains(letter)) locSpeed = slowDownMult;
            if (skipCharachters.Contains(letter)) locSpeed = 0.001f;

            if (letter == ' ' && switchCount)
            {
                switchCount = false;
                text.text += "\n";
            }

            text.text += letter;
            if(audioClip!= null && text.text.Length % 3 == 0)audioPlayer.PlayOneShot(audioClip);

            ResizeCanvas();

            if (text.text.Length > 70 && letter == '.')
            {
                yield return new WaitForSeconds(Mathf.Clamp(text.text.Length * 0.05f, 0, 4));
                text.text = "";
            }

            yield return new WaitForSeconds(locSpeed);
        }

        for (int i = 0; i < 400; i++)
        {
            //CODE VOMIT, FIX LATER
            if (attatchedObj == null) DestroyPopup();
            if (attatchedObj == null || (cancelLeft && attatchedObj.name == "InspectUiTargetL"))
            {
                cancelLeft = false;
                text.text = "";
                targSize = Vector2.zero;

                yield return new WaitForSeconds(0.05f);

                isTypewriterActive = false;
                DestroyPopup();
            }
            if (attatchedObj == null || (cancelRight && attatchedObj.name == "InspectUiTargetR"))
            {
                cancelRight = false;
                text.text = "";
                targSize = Vector2.zero;

                yield return new WaitForSeconds(0.05f);

                isTypewriterActive = false;
                DestroyPopup();
            }
            //===================
            yield return new WaitForSeconds(Time.deltaTime);
        }

        
        if(!hold)
        {
            text.text = "";
            targSize = Vector2.zero;
            yield return new WaitForSeconds(0.05f);
            isTypewriterActive = false;
            DestroyPopup();
        }

    }
    //===============================================================================

    private void OnDestroy()
    {
        DestroyPopup();
    }
    public void DestroyPopup()
    {
        ServiceHub.Instance.popupManager.DialougeBoxDestroying(this);
    }

    void Update()
    {
        panelRectTransform.sizeDelta = Vector2.Lerp(panelRectTransform.sizeDelta, targSize, 20* Time.deltaTime);
    }

}
