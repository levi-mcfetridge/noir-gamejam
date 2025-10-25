using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PrisonScene : MonoBehaviour
{
    public static PrisonScene instance;

    public GameObject lockObject;
    public bool talkedToHog = false;
    public bool isCellLocked = true;
    public bool isFinalDoorLocked = true;
    public bool caught = false;

    private bool isHandlingCatch = false; // prevents multiple coroutines

    private void Start()
    {
        Player.instance.canMove = true;
        caught = false;
    }

    private void Awake()
    {
        instance = this; // this is what makes 'PrisonScene.instance' valid
    }

    private void Update()
    {

        if (caught && !isHandlingCatch)
        {
            isHandlingCatch = true;
            Player.instance.transform.position = Vector3.zero;
            StartCoroutine(HandleCaught());
        }
        if (!isCellLocked)
        {
            Player.instance.canMove = false;
            lockObject.SetActive(false);
        }

        if (!isFinalDoorLocked)
        {
            SceneLoader.instance.LoadNextScene();
            isFinalDoorLocked = true;
        }
    }

    private IEnumerator HandleCaught()
    {
        Player.instance.canMove = false;
        yield return new WaitForSeconds(3f);

        // If your Player uses CharacterController, you can optionally disable/enable it around the warp:
        // var cc = Player.instance.GetComponent<CharacterController>();
        // if (cc) cc.enabled = false;
        Player.instance.transform.position = Vector3.zero;
        // if (cc) cc.enabled = true;

        Player.instance.canMove = true;
        caught = false;           // consume the event so it doesn't retrigger
        isHandlingCatch = false;  // allow future catches
    }
}
