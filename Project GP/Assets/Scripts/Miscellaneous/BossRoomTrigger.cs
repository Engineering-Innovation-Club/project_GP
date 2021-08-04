using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class BossRoomTrigger : MonoBehaviour
{
    public GameObject door;
    public AudioManagerScript aScript;

    public GameObject player;
    public GameObject boss;

    public List<string> speakerList;
    public List<string> messageList;
    public List<Sprite> imageList;
    public List<PlayableAsset> animList;

    public DialogueScript dScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerHealthControl>().health <= 0)
        {
            door.SetActive(false);
        }
    }

    void Add(List<string> sList, List<string> mList, List<Sprite> iList, List<PlayableAsset> aList)
    {
        for (int i = 0; i < sList.Count; i++)
        {
            dScript.AddDialogue(sList[i], mList[i], iList[i], aList[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            door.SetActive(true);
            aScript.PlayBoss();
            boss.SetActive(true);
        }
    }
}
