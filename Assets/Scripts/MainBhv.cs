using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBhv : MonoBehaviour
{
    public static MainBhv Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }


        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

    public UnityPharus.UnityPharusManager PharusMgr;
    public GameObject SubjectPrefab;

    private bool _initOk = false;


    public class TrackedUnit
    {
        public UnityPharus.UnityPharusManager.Subject Subject;
        public GameObject GameObj;
    }

    public Dictionary<int, TrackedUnit> TrackedUnits;

    // Start is called before the first frame update
    void Start()
    {
        TrackedUnits = new();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_initOk)
        {
            try
            {
                PharusMgr.EventProcessor.TrackUpdated += TrackUpdated;
                PharusMgr.EventProcessor.TrackAdded += TrackAdded;
                PharusMgr.EventProcessor.TrackRemoved += TrackRemoved;
                _initOk = true;
            } catch (System.Exception e)
            {
                
            }
        }
    }

    public static void TrackUpdated(object sender, UnityPharus.UnityPharusEventProcessor.PharusEventTrackArgs args)
    {
        Debug.Log("TRACK UPD: " + args.trackRecord.trackID + " -- " + args.trackRecord.currentPos.x + " / " + args.trackRecord.currentPos.y);

        if (!MainBhv.Instance.TrackedUnits.ContainsKey(args.trackRecord.trackID))
        {
            CreateTrackedUnit(args);
        }

        MainBhv.Instance.TrackedUnits[args.trackRecord.trackID].Subject.position =  new Vector3(args.trackRecord.currentPos.x, 0, args.trackRecord.currentPos.y);
    }

    public static void CreateTrackedUnit(UnityPharus.UnityPharusEventProcessor.PharusEventTrackArgs args)
    {
        var go = GameObject.Instantiate(MainBhv.Instance.SubjectPrefab);

        UnityPharus.UnityPharusManager.Subject s = new UnityPharus.UnityPharusManager.Subject();
        s.id = args.trackRecord.trackID;
        s.position = new Vector3(args.trackRecord.currentPos.x, 0, args.trackRecord.currentPos.y);

        go.GetComponent<SubjectBhv>().Subject = s;

        TrackedUnit tu = new TrackedUnit();
        tu.GameObj = go;
        tu.Subject = s;

        MainBhv.Instance.TrackedUnits[s.id] = tu;
    }

    public static void TrackAdded(object sender, UnityPharus.UnityPharusEventProcessor.PharusEventTrackArgs args)
    {
        Debug.Log("TRACK ADD: " + args.trackRecord.trackID + " -- " + args.trackRecord.currentPos.x + " / " + args.trackRecord.currentPos.y);


        if (MainBhv.Instance.TrackedUnits.ContainsKey(args.trackRecord.trackID))
        {
            var toDel = MainBhv.Instance.TrackedUnits[args.trackRecord.trackID];
            GameObject.Destroy(toDel.GameObj);
        }

        CreateTrackedUnit(args);
        
    }

    public static void TrackRemoved(object sender, UnityPharus.UnityPharusEventProcessor.PharusEventTrackArgs args)
    {
        Debug.Log("TRACK REM: " + args.trackRecord.trackID + " -- " + args.trackRecord.currentPos.x + " / " + args.trackRecord.currentPos.y);
        if (MainBhv.Instance.TrackedUnits.ContainsKey(args.trackRecord.trackID))
        {
            var toDel = MainBhv.Instance.TrackedUnits[args.trackRecord.trackID];
            GameObject.Destroy(toDel.GameObj);

            MainBhv.Instance.TrackedUnits.Remove(args.trackRecord.trackID);
        }
    }
}
