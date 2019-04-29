using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommentController : MonoBehaviour
{

    [SerializeField] GameObject hospitalPannel;
    [SerializeField] GameObject pillsPannel;
    [SerializeField] GameObject moneyPannel;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject declineButton;
    [SerializeField] Animator animator;

    private string commentCache;
    private TextMeshProUGUI commentText;
    private bool isPlaying;
    private bool startToggled;
    private bool gameOverToggled;
    private bool eventToggled;

    private PanelController panelController;

    // Start is called before the first frame update
    void Start()
    {
        gameOverToggled = false;
        startToggled = false;
        eventToggled = false;
        isPlaying = false;
        animator.Play("Rest");
        PlayAnimationForward();

        panelController = moneyPannel.GetComponent<PanelController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimationForward()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            ToggleStartOrEndCommentState(false);
            if (!gameOverToggled && !eventToggled)
            {
                ToggleStartComment(true);
            }
            animator.Play("CommentPanelAnimationForward");
            pillsPannel.GetComponent<CanvasGroup>().alpha = 0;
            moneyPannel.GetComponent<CanvasGroup>().alpha = 0;
            hospitalPannel.GetComponent<CanvasGroup>().alpha = 0;
            pillsPannel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            moneyPannel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            hospitalPannel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void PlayAnimationBackward()
    {
        if(!isPlaying)
        {
            ToggleStartOrEndCommentState(false);
            if (!gameOverToggled)
            {
                ToggleStartComment(false);
            }
            isPlaying = true;
           
            animator.Play("CommentPanelAnimationBackward");
        }
    }

    public void OnAnimationForwardComplete()
    {
        isPlaying = false;
        ToggleStartOrEndCommentState(true);
       
    }

    public void ClickContinueButton()
    {
        if (gameOverToggled)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.name);

        } else
        {
            PlayAnimationBackward();
        }
       
        startToggled = false;
        gameOverToggled = false;
        eventToggled = false;
    }

    public void OnAnimationBackwardComplete()
    {
        ToggleStartOrEndCommentState(true);
        isPlaying = false;

        panelController.Toggle("PillsPanel");
        panelController.Show(moneyPannel);

        ToggleButtons(false, false);
    }

    private void ToggleStartOrEndCommentState(bool toggle)
    {
        commentText = transform.Find("CommentText").GetComponent<TextMeshProUGUI>();
        commentText.enabled = toggle;
    }

    public void SetCatchPhrase(string catchPhrase)
    {
        if (startToggled || eventToggled)
        {
            commentCache = catchPhrase;
        } else
        {
            commentText.SetText(catchPhrase);
        }
    }

    private void ToggleStartComment(bool toggle)
    {
        startToggled = toggle;
        if (toggle)
        {
            ToggleButtons(true, false);
            commentText = transform.Find("CommentText").GetComponent<TextMeshProUGUI>();
            commentCache = commentText.text;
            string startComment = "";
            GameContext.startStatements.ForEach(statement => startComment += statement + "\n\n");
            commentText.SetText(startComment);
            commentText.alignment = TMPro.TextAlignmentOptions.MidlineJustified;
        }
        else
        {
            if (commentText != null)
            {
                commentText.alignment = TMPro.TextAlignmentOptions.TopJustified;
                commentText.SetText(commentCache);
            }
        }
    }

    public void ToggleGameOverComment(bool toggle)
    {
        gameOverToggled = toggle;
        if (toggle)
        {
            ToggleButtons(true, false);
            commentText = transform.Find("CommentText").GetComponent<TextMeshProUGUI>();
            commentCache = commentText.text;
            commentText.SetText(GameContext.endStatements[UnityEngine.Random.Range(0, GameContext.endStatements.Count - 1)]);
            commentText.alignment = TMPro.TextAlignmentOptions.MidlineJustified;
            continueButton.transform.Find("ContinueButtonText").GetComponent<TextMeshProUGUI>().SetText("Replay");
        }
        else
        {
            if (commentText != null)
            {
                commentText.alignment = TMPro.TextAlignmentOptions.TopJustified;
                commentText.SetText(commentCache);
            }
        }
    }

    public void ToggleEventComment(bool toggle, string eventText = "")
    {
        eventToggled = toggle;
        if (toggle)
        {
            commentText = transform.Find("CommentText").GetComponent<TextMeshProUGUI>();
            commentCache = commentText.text;
            commentText.SetText(eventText);
            commentText.alignment = TMPro.TextAlignmentOptions.MidlineJustified;
            continueButton.transform.Find("ContinueButtonText").GetComponent<TextMeshProUGUI>().SetText("Accept");
            ToggleButtons(true, true);
        }
        else
        {
            if (commentText != null)
            {
                commentText.alignment = TMPro.TextAlignmentOptions.TopJustified;
                commentText.SetText(commentCache);
            }
        }
    }

    public void SetButtonEffect(IEvent gameEvent, PlayerHealth player)
    {
        Debug.Log("SetButtonEffect");
        continueButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => gameEvent.Accept(player));
        declineButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => gameEvent.Decline(player));
    }

    public void ResetButtonListener()
    {
        Debug.Log("Reset");
        continueButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        declineButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        continueButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ClickContinueButton());
        declineButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ClickContinueButton());
     
      
    }

    public void ToggleButtons(bool enableContinue,bool enableDecline)
    {
        if (enableContinue)
        {
            continueButton.GetComponent<CanvasGroup>().alpha = 1;
            continueButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
        } else
        {
            continueButton.GetComponent<CanvasGroup>().alpha = 0;
            continueButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
            ResetButtonListener();

        }
        if (enableDecline)
        {
            declineButton.GetComponent<CanvasGroup>().alpha = 1;
            declineButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            declineButton.GetComponent<CanvasGroup>().alpha = 0;
            declineButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
            ResetButtonListener();
        }
        
    }
}
