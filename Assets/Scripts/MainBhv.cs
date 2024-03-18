using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBhv : MonoBehaviour
{
    public UnityPharus.UnityPharusManager PharusMgr;

    private bool _initOk = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_initOk)
        {
            try
            {
                PharusMgr.EventProcessor.TrackUpdated += TrackUpdated;
                _initOk = true;
            } catch (System.Exception e)
            {
                
            }
        }
    }

    public static void TrackUpdated(object sender, UnityPharus.UnityPharusEventProcessor.PharusEventTrackArgs args)
    {
        Debug.Log("TRACK UPD: " + args.trackRecord.trackID + " -- " + args.trackRecord.currentPos.x + " / " + args.trackRecord.currentPos.y);
    }
}
